using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SFML.Graphics;

namespace ProjektObiektowe
{
	class Plansza
	{
		List<Sprite> Sciany;
		public Plansza()
		{	
			//tworzy pustą plansze
		}
		public Plansza(Bitmap mapaPlanszy, Bitmap teksturaSciany) //mapa 32x18
		{
			System.Diagnostics.Debug.WriteLine("ctor planszy");
			Texture TeksturaSciany = new Texture(Rysowanie.BitmapaNaByte(teksturaSciany));
			Sprite temp;
			Sciany = new List<Sprite>();
			int wysokoscMapy = mapaPlanszy.Size.Height;
			int szerokoscMapy = mapaPlanszy.Size.Width;
			for (int i = 0; i < wysokoscMapy; i++)
				for (int j = 0; j < szerokoscMapy; j++)
					if (mapaPlanszy.GetPixel(j, i).GetBrightness() == 0)
					{
						temp = new Sprite(TeksturaSciany);
						temp.Position = new SFML.System.Vector2f(j * TeksturaSciany.Size.X, i * TeksturaSciany.Size.Y);
						Rysowanie.Rysowane.Add(temp);
						Sciany.Add(temp);
						System.Diagnostics.Debug.WriteLine(szerokoscMapy);
					}

		}
	}
}
