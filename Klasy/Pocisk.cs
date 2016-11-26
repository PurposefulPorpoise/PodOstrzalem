using SFML.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektObiektowe
{
	class Pocisk :JednostkaRysowana, IRuchomy
	{
		float Kierunek; //jako kąt
		float Predkosc;
		public Pocisk(Bitmap tekstura, Vector2f pozycjaPocz, float kierunek, float predkosc)
			:base(tekstura, pozycjaPocz)
		{
			Kierunek = kierunek;
			Predkosc = predkosc;
		}
		public void Rusz()
		{

		}
	}
}
