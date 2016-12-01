using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SFML.Graphics;
using SFML.System;

namespace ProjektObiektowe
{
	enum Kierunek { N, NE, E, SE, S, SW, W, NW };
	class PostacGracza : JednostkaRysowana, IRuchomy, IAnimowany, ISmiertelny
	{
		/// <summary>
		/// Postac gracza - poruszanie, animowanie i zdrowie
		/// </summary


		private int _zdrowie;
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

		
		public void PrzyjmijObrazenia(int obrazenia)
		{
			zdrowie = zdrowie - obrazenia;
			LogikaGry.Instancja.PasekZyciaSzerokosc = zdrowie;
		}

		bool Idzie;
		Kierunek KierunekRuchu;

		public FloatRect ProstokatKolizji
		{ get { return Narzedzia.SkalowanyProstokat(Sprite.GetGlobalBounds(), 0.7f); } }

		private Animacja Anim;
		float PredkoscChodzenia;

		public PostacGracza(System.Drawing.Bitmap bitmapa, int kolumny, int wiersze, Vector2f pozycja)
			:base(bitmapa, pozycja)
		{
			zdrowie = 3;
			LogikaGry.Instancja.PasekZyciaSzerokosc = zdrowie;
			PredkoscChodzenia = 250f;
			Anim = new Animacja((int)SpriteSheet.Size.X, (int)SpriteSheet.Size.Y, kolumny, wiersze);
			//ustawia oś obrotu na srodek //domyslne to lewy gorny rog
			Sprite.Origin = new Vector2f(SpriteSheet.Size.X / kolumny / 2, SpriteSheet.Size.Y / wiersze / 2);
			Sprite.Scale = new Vector2f(0.7f, 0.7f);
			Sprite.TextureRect = Anim.ObecnaKlatka(0); //pierwsza klatka
		}
		public void Rusz()
		{
			StanZKlawiatury();
			float odlegloscPrzesuniecia = PredkoscChodzenia *
						(float)LogikaGry.Instancja.DeltaCzasu; //odl zalezna od czasu nie od fps //cos nietak z deltą, im dluzej sie gra tym wolniej chodzi?
			Vector2f PozycjaPoprzednia = Pozycja;
			Vector2f Przesuniecie = new Vector2f(0f, 0f);
			if (Idzie)
				switch (KierunekRuchu)
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
			Pozycja += Przesuniecie;
			Sprite.Rotation = (int)KierunekRuchu * 45f;
		}
		public void Animuj(ulong numerKlatkiGry)
		{
			if (Idzie)
			{
				Sprite.TextureRect = Anim.ObecnaKlatka(0);
			}
		}
		private void StanZKlawiatury()
		{
				if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
			{
					 if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W))
					KierunekRuchu = Kierunek.NE;
					 else if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S)) 
					KierunekRuchu = Kierunek.SE;
				else KierunekRuchu = Kierunek.E;
			}
				else if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A))
			{
					 if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W))
					KierunekRuchu = Kierunek.NW;
					 else if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S))
					KierunekRuchu = Kierunek.SW;
				else KierunekRuchu = Kierunek.W;
			}
				else if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W))
				KierunekRuchu = Kierunek.N;
				else if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S))
				KierunekRuchu = Kierunek.S;

			Idzie = (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.Up) 
						  || Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.W));
		}
	}
}