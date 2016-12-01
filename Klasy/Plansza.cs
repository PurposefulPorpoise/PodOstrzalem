using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SFML.Graphics;
using SFML.System;

namespace ProjektObiektowe
{
	static class Plansza
	{
		private static List<Sprite> Sciany = new List<Sprite>();
		public static List<Sprite> sciany { get { return Sciany; } }
		public static void StworzZBitmapy(Bitmap mapaPlanszy, Bitmap teksturaSciany) //mapa 32x16
		{
			Texture TeksturaSciany = new Texture(Rysowanie.BitmapaNaByte(teksturaSciany));
			TeksturaSciany.Smooth = true;
			Sprite temp;
			Sciany = new List<Sprite>();
			int wysokoscMapy = mapaPlanszy.Size.Height;
			int szerokoscMapy = mapaPlanszy.Size.Width;
			for (int i = 0; i < wysokoscMapy; i++)
				for (int j = 0; j < szerokoscMapy; j++)
				{
					Vector2f pozycja = new Vector2f(j * TeksturaSciany.Size.X, i * TeksturaSciany.Size.Y);
					int kolor = mapaPlanszy.GetPixel(j, i).ToArgb();
					if (kolor == System.Drawing.Color.Black.ToArgb()) //plansza z mapy-obrazka
					{
						temp = new Sprite(TeksturaSciany);
						temp.Position = pozycja;
						//Rysowanie.Rysowane.Add(temp);
						LogikaGry.Instancja.DodajRysowane(temp);
						Sciany.Add(temp);
					}
					else if (kolor == System.Drawing.Color.Red.ToArgb())
					{

						LogikaGry.Instancja.DodajDzialko(new Dzialko(Properties.Resources.sciana, pozycja, new TimeSpan(0, 0, 0, 0, 400)));
					}

				}
		}

	}
}
