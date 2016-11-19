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
	enum Kierunek { N, NE, E, SE, S, SW, W, NW };
	class PostacGracza :IAnimowany, ISmiertelny
	{
		/// <summary>
		/// Gracz
		/// </summary


		  private int _zdrowie = 3;
		  public int zdrowie
		  {
				get
				{
					 return _zdrowie;
				}
				set
				{
					 if (value <= 0)
					 {
						  _zdrowie = 0;
						  
					 }
					 else if (value >= 3)
					 {
						  _zdrowie = 3;
					 }
					 else
					 {
						  _zdrowie = value;
					 }
				}
		  }

		  //jesli pocisk dotknie gracza to wywolac ta metode(obrazenia zawsze = 1)?
		  public void PrzyjmijObrazenia(int obrazenia)
		  {
			  zdrowie = zdrowie - obrazenia;
		  }

		  public void Umrzyj()
		  {
				//znikniecie postaci? nie ma sensu byc w interfejsie
		  } 


		public bool Idzie;
		public Kierunek KierunekPostaci;
		Sprite SpritePostaci;
		Image SpriteSheet;
		private Animacja Anim;
		public Animacja animacja
		{
			get { return Anim;}
		}
		public float PredkoscChodzenia;
		
		public PostacGracza(System.Drawing.Bitmap bitmapa, int kolumny, int wiersze) //konstruktor
		{
			PredkoscChodzenia = 140f;
			SpriteSheet = new Image(Rysowanie.BitmapaNaByte(bitmapa)); //konwersja Bitmapy .Net na Image sfml'a
			Anim = new Animacja((int)SpriteSheet.Size.X, (int)SpriteSheet.Size.Y, kolumny, wiersze);
			SpritePostaci = new Sprite(new Texture(SpriteSheet));
			//ustawia oś obrotu na srodek //domyslne to lewy gorny rog
			SpritePostaci.Origin = new Vector2f(SpriteSheet.Size.X / kolumny / 2, SpriteSheet.Size.Y / wiersze / 2);
			SpritePostaci.Texture.Smooth = true; // filtrowanie tekstury
			SpritePostaci.Position = new Vector2f(560, 560);
			SpritePostaci.Scale = new Vector2f(0.7f, 0.7f);
			SpritePostaci.TextureRect = Anim.ObecnaKlatka(Rysowanie.NrKlatki); //pierwsza klatka
			if (!Rysowanie.Rysowane.Contains(SpritePostaci)) Rysowanie.Rysowane.Add(SpritePostaci);
		}
		public void Rusz()
		{
			StanZKlawiatury();
			float odlegloscPrzesuniecia = PredkoscChodzenia *
						(float)Rysowanie.DeltaCzasu.Elapsed.TotalSeconds; //odl zalezna od czasu nie od fps
			Vector2f PozycjaPoprzednia = SpritePostaci.Position;
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
			SpritePostaci.Position += Przesuniecie;
			SpritePostaci.Rotation = (int)KierunekPostaci * 45f;
			SpritePostaci.Position = Plansza.ReakcjaNaKolizje(SkalowanyProstokat(SpritePostaci.GetGlobalBounds(), 0.8f), SpritePostaci.Position);
			if (Idzie) //nastepna klatka jesli nie stoi
			{
				SpritePostaci.TextureRect = Anim.ObecnaKlatka(Rysowanie.NrKlatki);
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
			else if (Keyboard.IsKeyDown(Key.S))
				KierunekPostaci = Kierunek.S;

			Idzie = (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.A)
						|| Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.W));
		}
		private FloatRect SkalowanyProstokat(FloatRect oryg, float skala)
		{
			return new FloatRect(new Vector2f(oryg.Left + ((1f - skala) / 2) * oryg.Width, oryg.Top + ((1f - skala) / 2) * oryg.Height),
				new Vector2f(oryg.Width * skala, oryg.Height * skala));
		}
	}
}
