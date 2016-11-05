using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SFML.Graphics;

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
		static public bool KolizjaZeSciana(FloatRect A)
		{
			bool kolizja = false;
			foreach (var sciana in Sciany)
			{
				FloatRect B = sciana.GetGlobalBounds();
				if(!((A.Left+A.Width)<B.Left || (B.Left+B.Width)<A.Left 
					|| (A.Top+A.Height)<B.Top || (B.Top+B.Height)<A.Top))
						kolizja = true;
			}
			return kolizja;
		}
	}
}
