using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.IO;

namespace ProjektObiektowe
{
	static class Rysowanie
	{
		static private RenderWindow OknoRenderowania;
		public static System.Windows.Forms.Timer LicznikRysowania = new System.Windows.Forms.Timer();
		public static SfmlDrawingSurface PowierzchniaRys;


		public static void Start()
		{
			StworzOknoRenderowania();
			LicznikRysowania.Interval = 1000 / 60; //fps 60
			PowierzchniaRys.SizeChanged += Surface_SizeChanged;
			LicznikRysowania.Start();
			Application.Current.MainWindow.Deactivated += Zminimalizowano;
			Application.Current.MainWindow.Activated += Powrot;
			Application.Current.MainWindow.Closed += Zakoncz;
		}

		private static void Powrot(object sender, EventArgs e)
		{
			LicznikRysowania.Start();
		}

		private static void Zminimalizowano(object sender, EventArgs e)
		{
			LicznikRysowania.Stop();
		}

		private static void StworzOknoRenderowania()
		{
			if (OknoRenderowania != null)
			{
				OknoRenderowania.SetActive(false);
				OknoRenderowania.Dispose(); //usuwa obecne okno, np przy zmianie rozmiaru
			}

			ContextSettings Context = new ContextSettings();
			OknoRenderowania = new RenderWindow(PowierzchniaRys.Handle, Context);
			OknoRenderowania.SetVerticalSyncEnabled(true);
			//OknoRenderowania.SetFramerateLimit(60); //nigdy oba naraz
			View widok = new View(new Vector2f(1280f / 2, 640f / 2),
				new Vector2f(1280, 640));
			OknoRenderowania.SetView(widok);
		}
		static private void Surface_SizeChanged(object s, EventArgs e)
		{
			StworzOknoRenderowania(); //tworzy nowe okno renderowania przy zmianie rozmiaru okna
											  //zeby uniknac bledow
		}
		public static void Rysuj(List<Drawable> rysowane, Color kolorTla)
		{
			OknoRenderowania.DispatchEvents(); //przetwarza wydarzenia sfml
			OknoRenderowania.Clear(kolorTla); //czysci ekran

			foreach (Drawable d in rysowane) //rysuje
				if (d != null) OknoRenderowania.Draw(d);
			OknoRenderowania.Display(); //przenosi z drugiego bufora na ekran (podwojne buforowanie)
		}
		public static void UstawWidok(double szerokosc, double wysokosc)
		{

			//OknoRenderowania.SetView(widok);

		}
		public static byte[] BitmapaNaByte(System.Drawing.Bitmap img) //konwersja Bitmapy .Net na Image sfml'a
		{
			System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
			return (byte[])converter.ConvertTo(img, typeof(byte[]));
		}
		public static void Zakoncz(object s, EventArgs e)
		{
			LicznikRysowania.Stop();
			//LogikaGry.LicznikDelty.Stop();
			OknoRenderowania.Close();
		}
	}
}