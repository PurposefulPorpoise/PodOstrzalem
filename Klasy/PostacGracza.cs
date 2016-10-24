using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace ProjektObiektowe
{
	class PostacGracza
	{
		Sprite SpriteGlowny;
		Sprite SpriteBroni;
		Image Animacja;
		Texture ObecnaKlatka;
		public PostacGracza(System.Drawing.Bitmap bitmapa) //przekazuje bitmape do konstruktora Strzelca
		{
			Animacja = new Image(Rysowanie.BitmapaNaByte(bitmapa));
		}
	}
}
