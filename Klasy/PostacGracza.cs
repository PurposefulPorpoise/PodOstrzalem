using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SFML.Graphics;
using SFML.System;
using System.ComponentModel;

namespace ProjektObiektowe
{
	//enum StanPostaci { Stoi, Idzie };
	enum Kierunek { N, NE, E, SE, S, SW, W, NW };
	class PostacGracza : INotifyPropertyChanged
	{
		/// <summary>
		/// Gracz
		/// </summary

		public bool Idzie;
		public Kierunek KierunekPostaci;
		Sprite SpritePostaci;
		Image Animacja;
		uint kolumnAnim;
		public uint Kolumnyy
		{
			get { return kolumnAnim; }
			set
			{
				kolumnAnim = value;
				RaisePropertyChanged("Kolumnyy");
			}
		}
		uint wierszyAnim;
		uint LicznikKlatekAnim = 0;
		public float PredkoscChodzenia;
		int DzielnikPredkosciAnim = 1; //szybkosc zmiany klatek = 1/Dzielnik
		public PostacGracza(System.Drawing.Bitmap bitmapa, uint kolumny, uint wiersze) //konstruktor
		{
			//Stan = StanPostaci.Stoi;
			PredkoscChodzenia = 400f;
			kolumnAnim = kolumny;
			Kolumnyy = kolumny;
			wierszyAnim = wiersze;
			Animacja = new Image(Rysowanie.BitmapaNaByte(bitmapa)); //konwersja Bitmapy .net na Image sfml'a
			SpritePostaci = new Sprite(new Texture(Animacja));
			//ustawia oś transformacji na srodek //domyslne to lewy gorny rog, co przesuwa przy odbiciu lustrzanym
			SpritePostaci.Origin = new Vector2f(Animacja.Size.X / kolumny / 2, Animacja.Size.Y / wiersze / 2);
			SpritePostaci.Texture.Smooth = true; // filtrowanie tekstury
			SpritePostaci.Position = new Vector2f(560, 560);
			SpritePostaci.Scale = new Vector2f(0.7f, 0.7f);
			SpritePostaci.TextureRect = KolejnaKlatkaAnim(); //pierwsza klatka
			if (!Rysowanie.Rysowane.Contains(SpritePostaci)) Rysowanie.Rysowane.Add(SpritePostaci);
			Rysowanie.LicznikRysowania.Tick += CoKlatke; //wywolywanie CoKlatke co okolo 16ms
		}
		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private void CoKlatke(object s, EventArgs e)
		{
			StanZKlawiatury();
			Rusz();

		}
		public void Rusz()
		{
			Idzie = (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.A)
			|| Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.W));

			float odlegloscPrzesuniecia = PredkoscChodzenia *
						(float)Rysowanie.DeltaCzasu.Elapsed.TotalSeconds;
			Vector2f PozycjaPoprzednia = SpritePostaci.Position;
			FloatRect Przed = SpritePostaci.GetGlobalBounds();
			Vector2f Przesuniecie = new Vector2f(0f, 0f);
			if (Idzie)
				switch (KierunekPostaci)
				{
				case Kierunek.N:
					Przesuniecie = new Vector2f(0f, -odlegloscPrzesuniecia);
					break;
				case Kierunek.NE:
					Przesuniecie = new Vector2f(odlegloscPrzesuniecia, -odlegloscPrzesuniecia) / 1.41f;
					break;
				case Kierunek.E:
					Przesuniecie = new Vector2f(odlegloscPrzesuniecia, 0f);
					break;
				case Kierunek.SE:
					Przesuniecie = new Vector2f(odlegloscPrzesuniecia, odlegloscPrzesuniecia) / 1.41f;
					break;
				case Kierunek.S:
					Przesuniecie = new Vector2f(0f, odlegloscPrzesuniecia);
					break;
				case Kierunek.SW:
					Przesuniecie = new Vector2f(-odlegloscPrzesuniecia, odlegloscPrzesuniecia) / 1.41f;
					break;
				case Kierunek.W:
					Przesuniecie = new Vector2f(-odlegloscPrzesuniecia, 0f);
					break;
				case Kierunek.NW:
					Przesuniecie = new Vector2f(-odlegloscPrzesuniecia, -odlegloscPrzesuniecia) / 1.41f;
					break;
				}
			//float ObrotPoprzedni = SpriteGlowny.Rotation;
			SpritePostaci.Rotation = (int)KierunekPostaci * 45f;
			//Kierunek DoWyzerowania;
			SpritePostaci.Position += Przesuniecie;
			//Vector2f V = SpriteGlowny.Position - PozycjaPoprzednia;
			//if (Plansza.KolizjaZeSciana(SpriteGlowny.GetGlobalBounds(), V, Przed, out DoWyzerowania)) //(Plansza.KolizjaZeSciana(SkalowanyProstokat(SpriteGlowny.GetGlobalBounds(), 0.7f), out DoWyzerowania))
			//{
			//	//SpriteGlowny.Position += new V;
			//	switch(DoWyzerowania)
			//	{
			//	case Kierunek.N:
			//	case Kierunek.S:
			//		Przesuniecie = new Vector2f(Przesuniecie.X, 0f);
			//		break;
			//	case Kierunek.W:
			//	case Kierunek.E:
			//		Przesuniecie = new Vector2f(0f, Przesuniecie.Y);
			//		break;
			//	default:
			//		throw new ArgumentOutOfRangeException(); //obecnie rzuca jesli v=0 i kolizja
			//	}
			//	SpriteGlowny.Position = PozycjaPoprzednia + Przesuniecie;
			//}
			//Vector2u WynikKolizji = Plansza.KolizjaZeSciana(SpriteGlowny.GetGlobalBounds(), V, Przed);
			//Przesuniecie = new Vector2f(Przesuniecie.X * (float)WynikKolizji.X, Przesuniecie.Y * (float)WynikKolizji.Y);
			SpritePostaci.Position = Plansza.KolizjaZeSciana(SkalowanyProstokat(SpritePostaci.GetGlobalBounds(), 0.8f), SpritePostaci.Position);
			if (Idzie) //nastepna klatka jesli nie stoi
			{
				if (Rysowanie.NrKlatki % (ulong)DzielnikPredkosciAnim == 0) //nastepka klatka anim co np 2 klatki gry
					SpritePostaci.TextureRect = KolejnaKlatkaAnim();
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
		//private void OdwrocSpritePoziomo()
		//{
		//	SpriteGlowny.Scale = new Vector2f(-SpriteGlowny.Scale.X, 1f);
		//}
		private FloatRect SkalowanyProstokat(FloatRect oryg, float skala)
		{
			return new FloatRect(new Vector2f(oryg.Left + ((1f - skala) / 2) * oryg.Width, oryg.Top + ((1f - skala) / 2) * oryg.Height),
				new Vector2f(oryg.Width * skala, oryg.Height * skala));
		}
		//private float DlugoscWektora(Vector2f wektor)
		//{
		//	double suma = wektor.X * wektor.X + wektor.Y * wektor.Y;
		//	return Convert.ToSingle(Math.Sqrt(suma));
		//}
	}
}
