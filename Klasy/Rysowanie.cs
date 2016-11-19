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
			LicznikRysowania.Interval = 1000 / 60; //fps 60
			//LicznikRysowania.Tick += Rysuj; //na tyle niedokładne że wychodzi średnio 22ms zamiast 16
			PowierzchniaRys.SizeChanged += Surface_SizeChanged;
			LicznikRysowania.Start();
			DeltaCzasu.Start();
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
			OknoRenderowania.SetView(new View(new Vector2f(PowierzchniaRys.Size.Width / 2, PowierzchniaRys.Height / 2),
				new Vector2f(PowierzchniaRys.Size.Width, PowierzchniaRys.Size.Height)));
		}
		static private void Surface_SizeChanged(object s, EventArgs e)
		{
			StworzOknoRenderowania(); //tworzy nowe okno renderowania przy zmianie rozmiaru okna
											  //zeby uniknac bledow
		}
		public static void Rysuj()
		{
			Color KolorTla = new Color(147, 169, 131);
			OknoRenderowania.DispatchEvents(); //przetwarza wydarzenia sfml
			OknoRenderowania.Clear(KolorTla); //czysci ekran

			foreach (Drawable d in Rysowane) //rysuje
				if (d != null) OknoRenderowania.Draw(d);
			OknoRenderowania.Display();

			DeltaCzasu.Reset(); //liczy czas od ostatniej klatki, z kazda klatką od nowa
			DeltaCzasu.Start();
			NrKlatki++;
		}
		public static byte[] BitmapaNaByte(System.Drawing.Bitmap img) //konwersja Bitmapy .Net na Image sfml'a
		{
			System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
			return (byte[])converter.ConvertTo(img, typeof(byte[]));
		}
		public static void Zakoncz(object s, EventArgs e)
		{
			LicznikRysowania.Stop();
			DeltaCzasu.Stop();
			OknoRenderowania.Close();
		}
	}
}