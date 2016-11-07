using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SFML.Graphics;
using SFML.System;

namespace ProjektObiektowe
{
	//enum StanPostaci { Stoi, Idzie };
	enum Kierunek { N, NE, E, SE, S, SW, W, NW };
	class PostacGracza
	{
		/// <summary>
		/// Gracz
		/// </summary

		public bool Idzie;
		public Kierunek KierunekPostaci;
		Sprite SpriteGlowny;
		//Sprite SpriteBroni; //na przyszlosc
		Image Animacja;
		uint kolumnAnim;
		uint wierszyAnim;
		uint LicznikKlatekAnim = 0;
		public float PredkoscChodzenia;
		int DzielnikPredkosciAnim = 1; //szybkosc zmiany klatek = 1/Dzielnik
		public PostacGracza(System.Drawing.Bitmap bitmapa, uint kolumny, uint wiersze) //konstruktor
		{
			//Stan = StanPostaci.Stoi;
			PredkoscChodzenia = 220f;
			kolumnAnim = kolumny;
			wierszyAnim = wiersze;
			Animacja = new Image(Rysowanie.BitmapaNaByte(bitmapa)); //konwersja Bitmapy .net na Image sfml'a
			SpriteGlowny = new Sprite(new Texture(Animacja));
			//ustawia oś transformacji na srodek //domyslne to lewy gorny rog, co przesuwa przy odbiciu lustrzanym
			SpriteGlowny.Origin = new Vector2f(Animacja.Size.X / kolumny / 2, Animacja.Size.Y / wiersze / 2);
			SpriteGlowny.Texture.Smooth = true; // filtrowanie tekstury
			SpriteGlowny.Position = new Vector2f(560, 560);
			//SpriteGlowny.Scale = new Vector2f(0.3f, 0.3f);
			SpriteGlowny.TextureRect = KolejnaKlatkaAnim(); //pierwsza klatka
			if (!Rysowanie.Rysowane.Contains(SpriteGlowny)) Rysowanie.Rysowane.Add(SpriteGlowny);
			Rysowanie.LicznikRysowania.Tick += CoKlatke; //wywolywanie CoKlatke co okolo 16ms
		}

		private void CoKlatke(object s, EventArgs e)
		{
			StanZKlawiatury();
			Rusz();
			
			//if (Plansza.KolizjaZeSciana(SkalowanyProstokat(SpriteGlowny.GetGlobalBounds(),0.7f) ))
			//	SpriteGlowny.Color = Color.Red;
			//else SpriteGlowny.Color = Color.White;

		}
		public void Rusz()
		{
			Idzie = (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.A)
			|| Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.W));

			float odlegloscPrzesuniecia = PredkoscChodzenia *
						(float)Rysowanie.DeltaCzasu.Elapsed.TotalSeconds;
			Vector2f PozycjaPoprzednia = SpriteGlowny.Position;
			if (Idzie)
				switch (KierunekPostaci)
				{
				case Kierunek.N:
					SpriteGlowny.Position += new Vector2f(0f, -odlegloscPrzesuniecia);
					break;
				case Kierunek.NE:
					SpriteGlowny.Position += new Vector2f(odlegloscPrzesuniecia, -odlegloscPrzesuniecia)/1.41f;
					break;
				case Kierunek.E:
					SpriteGlowny.Position += new Vector2f(odlegloscPrzesuniecia, 0f);
					break;
				case Kierunek.SE:
					SpriteGlowny.Position += new Vector2f(odlegloscPrzesuniecia, odlegloscPrzesuniecia) / 1.41f;
					break;
				case Kierunek.S:
					SpriteGlowny.Position += new Vector2f(0f, odlegloscPrzesuniecia);
					break;
				case Kierunek.SW:
					SpriteGlowny.Position += new Vector2f(-odlegloscPrzesuniecia, odlegloscPrzesuniecia) / 1.41f;
					break;
				case Kierunek.W:
					SpriteGlowny.Position += new Vector2f(-odlegloscPrzesuniecia, 0f);
					break;
				case Kierunek.NW:
					SpriteGlowny.Position += new Vector2f(-odlegloscPrzesuniecia, -odlegloscPrzesuniecia) / 1.41f;
					break;
				} // zmienic: nie cofniecie o klatke, tylko wybranie najmniejszego kierunku do usuniecia nachodzenia BB i przesunąć w tą strone
				  //float ObrotPoprzedni = SpriteGlowny.Rotation;
				  SpriteGlowny.Rotation = (int)KierunekPostaci * 45f;
			Vector2f Przesuniecie;
			if (Plansza.KolizjaZeSciana(SkalowanyProstokat(SpriteGlowny.GetGlobalBounds(), 0.7f), out Przesuniecie))
			{
				SpriteGlowny.Position += Przesuniecie;
			}
			if (Idzie) //nastepna klatka jesli nie stoi
			{
				if (Rysowanie.NrKlatki % (ulong)DzielnikPredkosciAnim == 0) //nastepka klatka anim co np 2 klatki gry
					SpriteGlowny.TextureRect = KolejnaKlatkaAnim();
			}
		}
		private void StanZKlawiatury()
		{
			if (Keyboard.IsKeyDown(Key.D))
			{
				if (Keyboard.IsKeyDown(Key.W))
					KierunekPostaci = Kierunek.NE;
				else if (Keyboard.IsKeyDown(Key.S))
					KierunekPostaci = Kierunek.SE;
				else KierunekPostaci = Kierunek.E;
			}
			else if (Keyboard.IsKeyDown(Key.A))
			{
				if (Keyboard.IsKeyDown(Key.W))
					KierunekPostaci = Kierunek.NW;
				else if (Keyboard.IsKeyDown(Key.S))
					KierunekPostaci = Kierunek.SW;
				else KierunekPostaci = Kierunek.W;
			}
			else if (Keyboard.IsKeyDown(Key.W))
				KierunekPostaci = Kierunek.N;
			else KierunekPostaci = Kierunek.S;


		}

		private IntRect KolejnaKlatkaAnim()
		{
			IntRect NowyObszar = new IntRect(
				(int)((Animacja.Size.X / kolumnAnim) * (LicznikKlatekAnim % kolumnAnim)),
				(int)((Animacja.Size.Y / wierszyAnim) * ((LicznikKlatekAnim / kolumnAnim) % wierszyAnim)),
				(int)(Animacja.Size.X / kolumnAnim), (int)(Animacja.Size.Y / wierszyAnim));
			LicznikKlatekAnim = (LicznikKlatekAnim + 1) % (kolumnAnim * wierszyAnim);
			return NowyObszar;
		}
		private void OdwrocSpritePoziomo()
		{
			SpriteGlowny.Scale = new Vector2f(-SpriteGlowny.Scale.X, 1f);
		}
		private FloatRect SkalowanyProstokat(FloatRect oryg, float skala)
		{
			return new FloatRect(new Vector2f(oryg.Left+((1f-skala)/2)*oryg.Width, oryg.Top+((1f-skala)/2)*oryg.Height),
				new Vector2f(oryg.Width * skala, oryg.Height * skala));
		}
		private float DlugoscWektora(Vector2f wektor)
		{
			double suma = wektor.X * wektor.X + wektor.Y * wektor.Y;
			return Convert.ToSingle(Math.Sqrt(suma)); 
		}
	}
}
