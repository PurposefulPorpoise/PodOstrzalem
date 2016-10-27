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

namespace ProjektObiektowe
{
	static class Rysowanie
	{
		static public List<Drawable> Rysowane = new List<Drawable>();
		static private RenderWindow RenderWindow;
		static public System.Windows.Forms.Timer LicznikRysowania = new System.Windows.Forms.Timer();
		static public Stopwatch DeltaCzasu = new Stopwatch();
		static public ulong NrKlatki = 0;
		static public SfmlDrawingSurface PowierzchniaRys;

		public static void Start()
		{
			StworzRenderWindow();
			LicznikRysowania.Interval = 1000 / 60; //dla czestotliwosci odswiezania 60Hz
			LicznikRysowania.Tick += Rysuj;
			PowierzchniaRys.SizeChanged += Surface_SizeChanged;
			LicznikRysowania.Start();
			DeltaCzasu.Start();
			Application.Current.MainWindow.Deactivated += Zminimalizowano;
			Application.Current.MainWindow.Activated += Powrot;
		}

		private static void Powrot(object sender, EventArgs e)
		{
			LicznikRysowania.Start();
		}

		private static void Zminimalizowano(object sender, EventArgs e)
		{
			LicznikRysowania.Stop();
		}

		private static void StworzRenderWindow()
		{
			if (RenderWindow != null)
			{
				RenderWindow.SetActive(false);
				RenderWindow.Dispose(); //usuwa obecne okno, np przy zmianie rozmiaru
			}

			ContextSettings Context = new ContextSettings(); //{ AntialiasingLevel = 4 };
			RenderWindow = new RenderWindow(PowierzchniaRys.Handle, Context);
			RenderWindow.SetVerticalSyncEnabled(true);
			//RenderWindow.SetFramerateLimit(6);

			RenderWindow.SetActive(true);
		}
		static private void Surface_SizeChanged(object s, EventArgs e)
		{
			StworzRenderWindow(); //tworzy nowe okno renderowania przy zmianie rozmiaru okna
												//zeby uniknac bledow
		}
		static private void Rysuj(object s, EventArgs e)
		{
			RenderWindow.DispatchEvents(); //przetwarza wydarzenia
			//Trace.WriteLine(DeltaTime.Elapsed.TotalSeconds);
			//DebugLabel.Content = DeltaTime.Elapsed.TotalSeconds;
			RenderWindow.Clear(SFML.Graphics.Color.White); //czysci ekran

			foreach (Drawable d in Rysowane) //rysuje
			{
				if (d != null) RenderWindow.Draw(d);
				Trace.WriteLine("Drawing d");
			}
			RenderWindow.Display();
			DeltaCzasu.Reset();
			DeltaCzasu.Start();
			NrKlatki++;
		}
		public static byte[] BitmapaNaByte(System.Drawing.Bitmap img)
		{
			System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
			return (byte[])converter.ConvertTo(img, typeof(byte[]));
		}
		public static void Zakoncz()
		{
			LicznikRysowania.Stop();
			LicznikRysowania.Dispose(); //zeby nie wywolywal rysowania po zamknieciu okna
			RenderWindow.Close();
		}

	}
}
