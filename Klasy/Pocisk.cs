using SFML.Graphics;
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
			PozycjaGracza = pozycjaGracza;
			PozycjaPocz = pozycjaPocz + (Narzedzia.Normalizuj(pozycjaGracza - pozycjaPocz) * 20f);
			Sprite.Position = PozycjaPocz;
		}
		public void Rusz()
		{
			Pozycja +=	Narzedzia.Normalizuj(PozycjaGracza - PozycjaPocz)
				* Predkosc * (float)LogikaGry.Instancja.DeltaCzasu;
		}
		public void ReagujNaKolizje()
		{
			FloatRect[] dotknieteSciany; //metoda wymaga, ale nieuzywane
			if (Kolizje.Kolizja(Sprite.GetGlobalBounds(), LogikaGry.Instancja.Gracz.ProstokatKolizji))
			{
				LogikaGry.Instancja.Gracz.PrzyjmijObrazenia(1);
				System.Diagnostics.Debug.WriteLine("Gracz przyjal obrazenia");
				ZniszczSie();
			}
			else if (Kolizje.KolizjaZeScianami(Sprite.GetGlobalBounds(), out dotknieteSciany))
				ZniszczSie();
		}
		void ZniszczSie()
		{
			LogikaGry.Instancja.ZniszczPocisk(this);
		}
	}
}
