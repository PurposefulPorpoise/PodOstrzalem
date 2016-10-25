using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SFML.Graphics;
using SFML.System;

namespace ProjektObiektowe
{
	class PostacGracza
	{
		public enum StanPostaci { Stoi, IdziePrawo, IdzieLewo };
		public StanPostaci Stan;
		Sprite SpriteGlowny;
		Sprite SpriteBroni;
		Image Animacja;
		uint kolumnAnim;
		uint wierszyAnim;
		uint LicznikKlatekAnim = 0;
		public float PredkoscChodzenia;
		int DzielnikPredkosciAnim = 3;
		public PostacGracza(System.Drawing.Bitmap bitmapa, uint kolumny, uint wiersze)
		{
			Stan = StanPostaci.Stoi;
			kolumnAnim = kolumny;
			wierszyAnim = wiersze;
			Animacja = new Image(Rysowanie.BitmapaNaByte(bitmapa));
			SpriteGlowny = new Sprite(new Texture(Animacja));
			SpriteGlowny.Texture.Smooth = true;
			SpriteGlowny.Position = new Vector2f(600, 200);
			//SpriteGlowny.Scale = new Vector2f(0.6f, 0.6f);
			if (!Rysowanie.Rysowane.Contains(SpriteGlowny)) Rysowanie.Rysowane.Add(SpriteGlowny);
			Rysowanie.LicznikRysowania.Tick += CoKlatke;
			Application.Current.MainWindow.KeyDown += Wcisnieto;
			Application.Current.MainWindow.KeyUp += Puszczono;
		}
		public void Rusz()
		{
			//TODO: tu podmienic spritesheet na animacje chodzenia
			if (Stan == StanPostaci.IdziePrawo)
				SpriteGlowny.Position +=
					new Vector2f((float)(PredkoscChodzenia * Rysowanie.DeltaCzasu.Elapsed.TotalSeconds), 0f);
			else if (Stan == StanPostaci.IdzieLewo)
				SpriteGlowny.Position +=
					new Vector2f((float)(-PredkoscChodzenia * Rysowanie.DeltaCzasu.Elapsed.TotalSeconds), 0f);

		}
		private void CoKlatke(object s, EventArgs e)
		{
			//Rusz();
			if(Rysowanie.NrKlatki % (ulong)DzielnikPredkosciAnim==0) //nastepka klatka anim co np 4 klatki gry
				SpriteGlowny.TextureRect = KolejnaKlatkaAnim();
		}

		private void Wcisnieto(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.D) Stan = StanPostaci.IdziePrawo;
			else if (e.Key == System.Windows.Input.Key.A) Stan = StanPostaci.IdzieLewo;
		}
		private void Puszczono(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.D ||
				e.Key == System.Windows.Input.Key.A) Stan = StanPostaci.Stoi;
		}
		private IntRect KolejnaKlatkaAnim()
		{
			IntRect NowyObszar = new IntRect(
				(int)((Animacja.Size.X / kolumnAnim)*(LicznikKlatekAnim%kolumnAnim)),
				(int)((Animacja.Size.Y / wierszyAnim)*((LicznikKlatekAnim/kolumnAnim)%wierszyAnim)),
				(int)(Animacja.Size.X / kolumnAnim), (int)(Animacja.Size.Y / wierszyAnim));
				//0, 700, 700, 555);
			LicznikKlatekAnim = (LicznikKlatekAnim + 1) % (kolumnAnim * wierszyAnim);
			return NowyObszar;
		}
	}
}
