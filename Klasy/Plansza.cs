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
		static List<Sprite> Sciany = new List<Sprite>();
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
		static public Vector2f ReakcjaNaKolizje(FloatRect A, Vector2f pozycja)//Vector2f v, FloatRect APrzed) //nie dziala przy kolizji z gorną śc. przy ruchu skosnym (tzn v.X==v.Y)
		{
			float[] Odleglosci = new float[4];
			int IndeksNajmniejszej = 0;
			Vector2f Wypchniecie = new Vector2f(0f, 0f);
			Vector2f NoweWypchniecie = new Vector2f(0f, 0f);

			foreach (var sciana in Sciany)
			{
				FloatRect B = sciana.GetGlobalBounds();
				if (Kolizja(A,B))
				{
					sciana.Color = SFML.Graphics.Color.Red;

					Odleglosci[0] = B.Left - (A.Left + A.Width); //new Vector2f(B.Left - (A.Left + A.Width), 0f);
					Odleglosci[1] = (B.Left + B.Width) - A.Left; //new Vector2f((B.Left + B.Width) - A.Left, 0f);
					Odleglosci[2] = B.Top - (A.Top + A.Height); //new Vector2f(0f, B.Top - (A.Top + A.Height));
					Odleglosci[3] = (B.Top + B.Height) - A.Top; //new Vector2f(0f, (B.Top + B.Height) - A.Top);

					IndeksNajmniejszej = 0;
					for (int i = 0; i < 4; i++)
						if (Math.Abs(Odleglosci[i]) < Math.Abs(Odleglosci[IndeksNajmniejszej])) IndeksNajmniejszej = i;


					NoweWypchniecie = IndeksNajmniejszej <= 1 ? new Vector2f(Odleglosci[IndeksNajmniejszej], 0f)
						: new Vector2f(0f, Odleglosci[IndeksNajmniejszej]);
					if (Math.Abs(NoweWypchniecie.X) > Math.Abs(Wypchniecie.X)) Wypchniecie.X = NoweWypchniecie.X;
					if (Math.Abs(NoweWypchniecie.Y) > Math.Abs(Wypchniecie.Y)) Wypchniecie.Y = NoweWypchniecie.Y;

				}
				else sciana.Color = SFML.Graphics.Color.White;

			}
			return pozycja + Wypchniecie;
		}
		static bool Kolizja(FloatRect a, FloatRect b)
		{
			//true jesli prostakaty na siebie nachodzą, czyli nie mozna ich rozdzielić linią pion./poziom.
			return (!((a.Left + a.Width) < b.Left || (b.Left + b.Width) < a.Left 
					|| (a.Top + a.Height) < b.Top || (b.Top + b.Height) < a.Top)); //Twierdzenie o osi rozdzielającej (SAT)
		}
		static bool FloatRownyZero(float a)
		{
			return Math.Abs(a) < 0.001;
		}
	}
}
