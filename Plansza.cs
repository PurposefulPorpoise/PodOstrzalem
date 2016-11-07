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
		static public bool KolizjaZeSciana(FloatRect A, out Vector2f przesuniecie)
		{
			przesuniecie = new Vector2f(0f, 0f);
			float[] Odleglosci = new float[4];
			int IndeksNajwiekszej;
			foreach (var sciana in Sciany)
			{
				FloatRect B = sciana.GetGlobalBounds();
				if (!((A.Left + A.Width) < B.Left || (B.Left + B.Width) < A.Left
					|| (A.Top + A.Height) < B.Top || (B.Top + B.Height) < A.Top)) //jesli prostokaty na siebie nachodza
				{

					Odleglosci[0] = Math.Abs((A.Left + A.Width) - B.Left); //-
					Odleglosci[1] = Math.Abs(A.Left - (B.Left + B.Width));
					Odleglosci[2] = Math.Abs((A.Top + A.Height) - B.Top); //-
					Odleglosci[3] = Math.Abs(A.Top - (B.Top + B.Height));
					IndeksNajwiekszej = Array.IndexOf(Odleglosci, Odleglosci.Max());
					System.Diagnostics.Debug.WriteLine(IndeksNajwiekszej);
					switch(IndeksNajwiekszej)
					{
					case 0:
						przesuniecie = new Vector2f(-Odleglosci[0], 0f);
						break;
					case 1:
						przesuniecie = new Vector2f(Odleglosci[1], 0f);
						break;
					case 2:
						przesuniecie = new Vector2f(0f, Odleglosci[2]);
						break;
					case 3:
						przesuniecie = new Vector2f(0f, -Odleglosci[3]);
						break;
					default:
						throw new ArgumentOutOfRangeException();
							
					}
					return true;
				}

			}
			return false;
		}
	}
}
