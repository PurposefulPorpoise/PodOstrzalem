using SFML.Graphics;
using System;

namespace ProjektObiektowe
{
	class Animacja
	{
		int DzielnikPredkosciAnim; //szybkosc zmiany klatek to 1/Dzielnik
		int Szerokosc;
		int Wysokosc;
		int LKolumn;
		int LWierszy;
		IntRect Klatka;
		int LicznikKlatek;
		public Animacja(int szerokosc, int wysokosc, int kolumny, int wiersze)
		{
			Szerokosc = szerokosc;
			Wysokosc = wysokosc;
			LKolumn = kolumny;
			LWierszy = wiersze;
			DzielnikPredkosciAnim = 1;
			LicznikKlatek = 0;
		}
		public Animacja(int szerokosc, int wysokosc, int kolumny, int wiersze, int dzielnikPredkosci)
			: this(szerokosc, wysokosc, kolumny, wiersze) //przekazuje do pierwszego konstruktora
		{
			DzielnikPredkosciAnim = dzielnikPredkosci;
		}
		public IntRect ObecnaKlatka(ulong numerKlatkiGry)
		{
			if (numerKlatkiGry % (ulong)DzielnikPredkosciAnim == 0)
			{
				Klatka = KolejnaKlatka();
			}
			return Klatka;
		}
		private IntRect KolejnaKlatka()
		{
			IntRect NowyObszar = new IntRect(
				((Szerokosc / LKolumn) * (LicznikKlatek % LKolumn)),
				((Wysokosc / LWierszy) * ((LicznikKlatek / LKolumn) % LWierszy)),
				(Szerokosc / LKolumn), (Wysokosc / LWierszy));
			LicznikKlatek = (LicznikKlatek + 1) % (LKolumn * LWierszy);
			return NowyObszar;
		}
	}
}
