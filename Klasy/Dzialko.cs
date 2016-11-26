using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace ProjektObiektowe
{
	class Dzialko :JednostkaRysowana, IRuchomy //wlasciwie obracany, ale to kwestia implementacji Rusz()
	{
		public Dzialko(Bitmap bitmapa, Vector2f pozycja) : base(bitmapa, pozycja)
		{
			Sprite.Color = SFML.Graphics.Color.Red;
		}
		public void Rusz()
		{
			if(CzyWidziGracza())
			{
			//tu obrot dzialka w strone gracza

			}
		}
		public bool CzyWidziGracza()
		{
			return true;
		}
		public IRuchomy Strzel()
		{
			return new Pocisk(Properties.Resources.pocisk, Pozycja, 0f, 10f);
		}
	}
}
