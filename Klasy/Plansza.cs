using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SFML.Graphics;
using SFML.System;

namespace ProjektObiektowe
{
	static class Plansza
	{
		static List<Sprite> Sciany;
		static public void StworzZBitmapy(Bitmap mapaPlanszy, Bitmap teksturaSciany) //mapa 32x16
		{
			Texture TeksturaSciany = new Texture(Rysowanie.BitmapaNaByte(teksturaSciany));
			TeksturaSciany.Smooth = true;
			Sprite temp;
			Sciany = new List<Sprite>();
			int wysokoscMapy = mapaPlanszy.Size.Height;
			int szerokoscMapy = mapaPlanszy.Size.Width;
			for (int i = 0; i < wysokoscMapy; i++)
				for (int j = 0; j < szerokoscMapy; j++)
					if (mapaPlanszy.GetPixel(j, i).GetBrightness() == 0) //plansza z mapy-obrazka
					{
						temp = new Sprite(TeksturaSciany);
						temp.Position = new SFML.System.Vector2f(j * TeksturaSciany.Size.X, i * TeksturaSciany.Size.Y);
						Rysowanie.Rysowane.Add(temp);
						Sciany.Add(temp);
					}
		}
		static public bool KolizjaZeSciana(FloatRect A, Vector2f v, FloatRect APrzed, out Kierunek Wyzeruj)
		{ //korzystajac z t=s/v znalezc sciana z najszybsza kolizja, a potem krawedz z najszybsza (min(t))
		  //przesuniecie = new Vector2f(0f, 0f);
			bool kolizja = false;
			Wyzeruj = Kierunek.NE; //niewazne
										  //float[] Odleglosci = new float[4];
										  //int IndeksNajmniejszej = 0;
			float NajmniejszyCzasOgolem = float.MaxValue;
			Dictionary<Sprite, float> CzasyKolizjiScian = new Dictionary<Sprite, float>();
			foreach (var sciana in Sciany)
			{
				FloatRect B = sciana.GetGlobalBounds();
				if (!((A.Left + A.Width) < B.Left || (B.Left + B.Width) < A.Left //Seperating Axis Theorem
					|| (A.Top + A.Height) < B.Top || (B.Top + B.Height) < A.Top)) //jesli prostokaty na siebie nachodza
				{
					kolizja = true;
					sciana.Color = SFML.Graphics.Color.Red;
					float NajmniejszyCzas = float.MaxValue;
					float temp = 0f;
					Kierunek KierunekNajmniejszego = Kierunek.NE;
					if (v.X != 0f)
					{
						temp = (B.Left - (APrzed.Left + APrzed.Width)) / v.X;
						if (Math.Abs(temp) < NajmniejszyCzas) { NajmniejszyCzas = temp; KierunekNajmniejszego = Kierunek.W; }
						temp = (APrzed.Left - (B.Left + B.Width)) / v.X;
						if (Math.Abs(temp) < NajmniejszyCzas) { NajmniejszyCzas = temp; KierunekNajmniejszego = Kierunek.E; }
					}
					if (v.Y != 0f)
					{
						temp = (APrzed.Top - (B.Top + B.Height)) / v.Y;
						if (Math.Abs(temp) < NajmniejszyCzas) { NajmniejszyCzas = temp; KierunekNajmniejszego = Kierunek.S; }
						temp = (B.Top - (A.Top + A.Height)) / v.Y;
						if (Math.Abs(temp) < NajmniejszyCzas) { NajmniejszyCzas = temp; KierunekNajmniejszego = Kierunek.N; }
					}
					if (NajmniejszyCzas < NajmniejszyCzasOgolem)
					{
						NajmniejszyCzasOgolem = NajmniejszyCzas;
						Wyzeruj = KierunekNajmniejszego;
					}
					//Odleglosci[0] = Math.Abs((A.Left + A.Width) - B.Left); //-
					//Odleglosci[1] = Math.Abs(A.Left - (B.Left + B.Width));
					//Odleglosci[2] = Math.Abs((A.Top + A.Height) - B.Top); //-
					//Odleglosci[3] = Math.Abs(A.Top - (B.Top + B.Height));
					//for (int i = 1; i < 4; i++)
					//	if (Odleglosci[i] < Odleglosci[IndeksNajmniejszej])
					//		IndeksNajmniejszej = i;
					//IndeksNajmniejszej = Array.IndexOf(Odleglosci, Odleglosci.Min());

					//System.Diagnostics.Debug.WriteLine(IndeksNajmniejszej);
					//switch (IndeksNajmniejszej)
					//{
					//case 0:
					//	//przesuniecie = new Vector2f(-Odleglosci[0], 0f);
					//	Wyzeruj = Kierunek.W;
					//	break;
					//case 1:
					//	//przesuniecie = new Vector2f(Odleglosci[1], 0f);
					//	Wyzeruj = Kierunek.E;
					//	break;
					//case 2:
					//	//przesuniecie = new Vector2f(0f, Odleglosci[2]);
					//	Wyzeruj = Kierunek.N;
					//	break;
					//case 3:
					//	//przesuniecie = new Vector2f(0f, -Odleglosci[3]);
					//	Wyzeruj = Kierunek.S;
					//	break;
					//default:
					//	throw new ArgumentOutOfRangeException()
					//}
					//return true;
				}
				else sciana.Color = SFML.Graphics.Color.White;

			}
			return kolizja;
		}
	}
}
