using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace ProjektObiektowe
{
	class Dzialko :JednostkaRysowana, IRuchomy
	{
		public Dzialko(Bitmap bitmapa, Vector2f pozycja) : base(bitmapa, pozycja)
		{
			Sprite.Color = SFML.Graphics.Color.Yellow;
		}
		public void Rusz()
		{ }
	}
}
