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
	class PostacGracza
	{
		public enum StanPostaci { Stoi, IdziePrawo, IdzieLewo };
		public enum Kierunek { Prawo, Lewo};
		public StanPostaci Stan;
		public Kierunek KierunekSprita;
		Sprite SpriteGlowny;
		Sprite SpriteBroni; //na przyszlosc
		Image Animacja;
		uint kolumnAnim;
		uint wierszyAnim;
		uint LicznikKlatekAnim = 0;
		public float PredkoscChodzenia;
		int DzielnikPredkosciAnim = 4;
		public PostacGracza(System.Drawing.Bitmap bitmapa, uint kolumny, uint wiersze)
		{
			Stan = StanPostaci.Stoi;
			kolumnAnim = kolumny;
			wierszyAnim = wiersze;
			Animacja = new Image(Rysowanie.BitmapaNaByte(bitmapa)); //konwersja Bitmapy .net na Image sfml'a
			SpriteGlowny = new Sprite(new Texture(Animacja));
			//ustawia oś transformacji na srodek //domyslne to lewy gorny rog, co przesuwa przy odbiciu lustrzanym
			SpriteGlowny.Origin = new Vector2f(Animacja.Size.X / kolumny / 2, Animacja.Size.Y / wiersze / 2);
			SpriteGlowny.Texture.Smooth = true; // filtrowanie tekstury
			SpriteGlowny.Position = new Vector2f(400, 500);
			SpriteGlowny.TextureRect = KolejnaKlatkaAnim(); //pierwsza klatka
			KierunekSprita = Kierunek.Prawo; //domyslny obrot sprite'a
			if (!Rysowanie.Rysowane.Contains(SpriteGlowny)) Rysowanie.Rysowane.Add(SpriteGlowny);
			Rysowanie.LicznikRysowania.Tick += CoKlatke; //wywolywanie CoKlatke co okolo 16ms
			//Application.Current.MainWindow.KeyDown += Wcisnieto;
			//Application.Current.MainWindow.KeyUp += Puszczono;
			Application.Current.MainWindow.Deactivated += Zminimalizowano;
		}
		
		private void CoKlatke(object s, EventArgs e)
		{
			StanZKlawiatury();
			Rusz();
			if (Stan != StanPostaci.Stoi) //HACK: pauzuje animacje jesli stoi
			{
				if (Rysowanie.NrKlatki % (ulong)DzielnikPredkosciAnim == 0) //nastepka klatka anim co np 4 klatki gry
					SpriteGlowny.TextureRect = KolejnaKlatkaAnim();
			}

			//if(System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.A))

		}
		public void Rusz()
		{
			//TODO: tu podmienic spritesheet na animacje chodzenia ze stania
			if (Stan == StanPostaci.IdziePrawo)
			{
				SpriteGlowny.Position +=
					new Vector2f((float)(PredkoscChodzenia * Rysowanie.DeltaCzasu.Elapsed.TotalSeconds), 0f);
				if (KierunekSprita == Kierunek.Lewo)
				{
					OdwrocSpritePoziomo();
					KierunekSprita = Kierunek.Prawo;
				}
			}
			else if (Stan == StanPostaci.IdzieLewo)
			{
				SpriteGlowny.Position +=
					new Vector2f((float)(-PredkoscChodzenia * Rysowanie.DeltaCzasu.Elapsed.TotalSeconds), 0f);
				if (KierunekSprita == Kierunek.Prawo)
				{
					OdwrocSpritePoziomo();
					KierunekSprita = Kierunek.Lewo;
				}
			}
		}
		private void StanZKlawiatury()
		{

			if (!(Stan == StanPostaci.IdzieLewo) && Keyboard.IsKeyDown(Key.D)) Stan = StanPostaci.IdziePrawo;
			else if (!(Stan == StanPostaci.IdziePrawo) && Keyboard.IsKeyDown(Key.A)) Stan = StanPostaci.IdzieLewo;
			else Stan = StanPostaci.Stoi;

		}

		private IntRect KolejnaKlatkaAnim()
		{
			IntRect NowyObszar = new IntRect(
				(int)((Animacja.Size.X / kolumnAnim) * (LicznikKlatekAnim % kolumnAnim)),
				(int)((Animacja.Size.Y / wierszyAnim) * ((LicznikKlatekAnim / kolumnAnim) % wierszyAnim)),
				(int)(Animacja.Size.X / kolumnAnim), (int)(Animacja.Size.Y / wierszyAnim));
			//0, 700, 700, 555);
			LicznikKlatekAnim = (LicznikKlatekAnim + 1) % (kolumnAnim * wierszyAnim);
			return NowyObszar;
		}
		private void OdwrocSpritePoziomo()
		{
			SpriteGlowny.Scale = new Vector2f(-SpriteGlowny.Scale.X, 1f);
		}
		private void Zminimalizowano(object sender, EventArgs e)
		{
			Stan = StanPostaci.Stoi;
		}
	}
}
