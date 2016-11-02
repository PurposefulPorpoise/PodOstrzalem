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
			System.Diagnostics.Debug.WriteLine("ctor gracza" + Rysowanie.Rysowane.Count);
			Rysowanie.LicznikRysowania.Tick += CoKlatke; //wywolywanie CoKlatke co okolo 16ms
			
		}

		private void CoKlatke(object s, EventArgs e)
		{
			StanZKlawiatury();
			Rusz();
			if (Idzie) //pauzuje animacje jesli stoi
			{
				if (Rysowanie.NrKlatki % (ulong)DzielnikPredkosciAnim == 0) //nastepka klatka anim co np 4 klatki gry
					SpriteGlowny.TextureRect = KolejnaKlatkaAnim();
			}

			//if(System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.A))

		}
		public void Rusz()
		{
			//SpriteGlowny.Position +=
			//new Vector2f((Convert.ToSingle(Keyboard.IsKeyDown(Key.D)) //x
			//				 - Convert.ToSingle(Keyboard.IsKeyDown(Key.A))) * PredkoscChodzenia
			//				 * (float)Rysowanie.DeltaCzasu.Elapsed.TotalSeconds,
			//				 (Convert.ToSingle(Keyboard.IsKeyDown(Key.S)) //y
			//				 - Convert.ToSingle(Keyboard.IsKeyDown(Key.W))) * PredkoscChodzenia
			//				 * (float)Rysowanie.DeltaCzasu.Elapsed.TotalSeconds)
			Idzie = (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.A)
			|| Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.W));

			float odlegloscPrzesuniecia = PredkoscChodzenia *
						(float)Rysowanie.DeltaCzasu.Elapsed.TotalSeconds;
			if (Idzie)
				switch (KierunekPostaci)
				{
				case Kierunek.N:
					SpriteGlowny.Position += new Vector2f(0f, -odlegloscPrzesuniecia);
					SpriteGlowny.Rotation = 0f;
					break;
				case Kierunek.NE:
					SpriteGlowny.Position += new Vector2f(odlegloscPrzesuniecia, -odlegloscPrzesuniecia)/1.41f;
					SpriteGlowny.Rotation = 45f; //w stopniach
					break;
				case Kierunek.E:
					SpriteGlowny.Position += new Vector2f(odlegloscPrzesuniecia, 0f);
					SpriteGlowny.Rotation = 90f;
					break;
				case Kierunek.SE:
					SpriteGlowny.Position += new Vector2f(odlegloscPrzesuniecia, odlegloscPrzesuniecia) / 1.41f;
					SpriteGlowny.Rotation = 135f;
					break;
				case Kierunek.S:
					SpriteGlowny.Position += new Vector2f(0f, odlegloscPrzesuniecia);
					SpriteGlowny.Rotation = 180f;
					break;
				case Kierunek.SW:
					SpriteGlowny.Position += new Vector2f(-odlegloscPrzesuniecia, odlegloscPrzesuniecia) / 1.41f;
					SpriteGlowny.Rotation = 225f;
					break;
				case Kierunek.W:
					SpriteGlowny.Position += new Vector2f(-odlegloscPrzesuniecia, 0f);
					SpriteGlowny.Rotation = 270f;
					break;
				case Kierunek.NW:
					SpriteGlowny.Position += new Vector2f(-odlegloscPrzesuniecia, -odlegloscPrzesuniecia) / 1.41f;
					SpriteGlowny.Rotation = 315f;
					break;
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
	}
}
