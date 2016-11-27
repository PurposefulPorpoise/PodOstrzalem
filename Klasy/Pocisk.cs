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
		float Predkosc;
		Vector2f PozycjaPocz;
		Vector2f PozycjaGracza; //tylko pozycja z czasu wystrzlu
		public Pocisk(Bitmap tekstura, Vector2f pozycjaPocz, float predkosc, Vector2f pozycjaGracza)
			:base(tekstura, pozycjaPocz)
		{
			Predkosc = predkosc;
			PozycjaPocz = pozycjaPocz;
			PozycjaGracza = pozycjaGracza;
		}
		public void Rusz()
		{
			Pozycja += WektoroweFunkcje.Normalizuj(PozycjaGracza - PozycjaPocz)
				* Predkosc * (float)LogikaGry.Instancja.DeltaCzasu;
		}
	}
}
