using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektObiektowe
{
	abstract class JednostkaRysowana //postac gracza i działka
	{
		protected Sprite Sprite;
		public Sprite sprite { get { return Sprite; } }
		protected Image SpriteSheet;
		public Vector2f Pozycja
		{
			get { return Sprite.Position; }
			set { Sprite.Position = value; }
		}
		protected JednostkaRysowana(System.Drawing.Bitmap bitmapa, Vector2f pozycja)
		{
			SpriteSheet = new Image(Rysowanie.BitmapaNaByte(bitmapa)); //konwersja Bitmapy .Net na Image sfml'a
			Sprite = new Sprite(new Texture(SpriteSheet));
			//domyslnie obrot wokol srodka, ale postac gracza wyjatkowo inaczej bo ma spritesheet
			Sprite.Origin = new Vector2f(Sprite.Texture.Size.X / 2, Sprite.Texture.Size.Y / 2);
			Sprite.Texture.Smooth = true; //wygladzanie tekstury
			Pozycja = pozycja;
			if (!Rysowanie.Rysowane.Contains(Sprite)) Rysowanie.Rysowane.Add(Sprite);
		}
	}
}
