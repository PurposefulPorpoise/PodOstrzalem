using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace ProjektObiektowe
{
	static class WektoroweFunkcje
	{
		static public Vector2f Normalizuj(Vector2f wektor) //zachowuje kierunek wektora, ale skaluje do dlugosci 1
		{
			double dlugosc = Math.Sqrt((wektor.X * wektor.X) + (wektor.Y * wektor.Y));
			return new Vector2f(wektor.X / (float)dlugosc, wektor.Y / (float)dlugosc);
		}
	}
}
