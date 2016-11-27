using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace ProjektObiektowe
{
	static class Narzedzia
	{
		public static Vector2f Normalizuj(Vector2f wektor) //zachowuje kierunek wektora, ale skaluje do dlugosci 1
		{
			double dlugosc = Math.Sqrt((wektor.X * wektor.X) + (wektor.Y * wektor.Y));
			return new Vector2f(wektor.X / (float)dlugosc, wektor.Y / (float)dlugosc);
		}
		public static FloatRect SkalowanyProstokat(FloatRect oryg, float skala)
		{
			return new FloatRect(
				new Vector2f(
					oryg.Left + ((1f - skala) / 2) * oryg.Width,
					oryg.Top + ((1f - skala) / 2) * oryg.Height),
				new Vector2f(oryg.Width * skala, oryg.Height * skala));
		}
	}
}
