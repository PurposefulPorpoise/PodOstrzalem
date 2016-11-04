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
		static public List<Drawable> Rysowane = new List<Drawable>();
		static private RenderWindow OknoRenderowania;
		static public System.Windows.Forms.Timer LicznikRysowania = new System.Windows.Forms.Timer();
		static public Stopwatch DeltaCzasu = new Stopwatch();
		static public ulong NrKlatki = 0;
		static public SfmlDrawingSurface PowierzchniaRys;


		public static void Start()
		{
			StworzOknoRenderowania();
			LicznikRysowania.Interval = 1000 / 60; //dla czestotliwosci odswiezania 60Hz
			LicznikRysowania.Tick += Rysuj; //na tyle niedokładne że wychodzi średnio 22ms zamiast 16
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

		private static void StworzOknoRenderowania()
		{
			if (OknoRenderowania != null)
			{
				OknoRenderowania.SetActive(false);
				OknoRenderowania.Dispose(); //usuwa obecne okno, np przy zmianie rozmiaru
			}

			ContextSettings Context = new ContextSettings(); //{ AntialiasingLevel = 4 };
			OknoRenderowania = new RenderWindow(PowierzchniaRys.Handle, Context);
			OknoRenderowania.SetVerticalSyncEnabled(true);
			//OknoRenderowania.SetFramerateLimit(60);
			OknoRenderowania.SetView(new View(new Vector2f(1280/2,720/2),new Vector2f(1280,720)));
		}
		static private void Surface_SizeChanged(object s, EventArgs e)
		{
			StworzOknoRenderowania(); //tworzy nowe okno renderowania przy zmianie rozmiaru okna
											  //zeby uniknac bledow
		}
		static private void Rysuj(object s, EventArgs e)
		{
			OknoRenderowania.DispatchEvents(); //przetwarza wydarzenia
			OknoRenderowania.Clear(SFML.Graphics.Color.White); //czysci ekran

			foreach (Drawable d in Rysowane) //rysuje
				if (d != null) OknoRenderowania.Draw(d);
			OknoRenderowania.Display();

			//if (!File.Exists("DeltaCzasu.csv"))
			//	File.CreateText("DeltaCzasu.csv").Close();
			//File.AppendAllText("DeltaCzasu.csv", String.Format("{0};{1}\n", NrKlatki, DeltaCzasu.Elapsed.TotalSeconds));

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
			DeltaCzasu.Stop();
			OknoRenderowania.Close();
		}

	}
}
