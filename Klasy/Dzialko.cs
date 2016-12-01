using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace ProjektObiektowe
{
	class Dzialko :JednostkaRysowana
	{
		TimeSpan _OdstepStrzalow;
		public TimeSpan OdstepStrzalow { get { return _OdstepStrzalow; } }
		DateTime _CzasOstatniegoStrzalu;
		public DateTime CzasOstatniegoStrzalu { get { return _CzasOstatniegoStrzalu; } }

		public Dzialko(Bitmap bitmapa, Vector2f pozycja, TimeSpan odstepStrzalow) : base(bitmapa, pozycja)
		{
			//poprawka pozycji, bo widocznie zmiana origina przesuwa sprite'a
			Pozycja = pozycja + new Vector2f(Sprite.Texture.Size.X / 2, Sprite.Texture.Size.Y / 2);
			Sprite.Color = SFML.Graphics.Color.Red;
			_OdstepStrzalow = odstepStrzalow;
		}
		public bool CzyWidziGracza() //TODO
		{
			return true;
		}
		public IRuchomy Strzel(Vector2f pozycjaGracza) //zwraca wystrzelony pocisk
		{
			Sprite.Rotation = (float)Math.Atan2(pozycjaGracza.Y - Sprite.Position.Y, pozycjaGracza.X - Sprite.Position.X)
				* 180f / (float)Math.PI + 90f; //z radianow na stopnie (sfml uzywa stopni), +90 bo atan2 daje kat od osi x, a sfml uzywa od osi y
			_CzasOstatniegoStrzalu = DateTime.Now;
			return new Pocisk(Properties.Resources.pocisk, Pozycja, 200f, LogikaGry.Instancja.Gracz.Pozycja);
		}
	}
}
