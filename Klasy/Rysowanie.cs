using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace ProjektObiektowe
{
	static class Rysowanie
	{
		static public List<Drawable> Rysowane = new List<Drawable>();
		static private Dictionary<Transformable, Vector2f> DoPrzesuniecia = new Dictionary<Transformable, Vector2f>();
		static private RenderWindow RenderWindow;
		static private System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();
		static private Stopwatch DeltaCzasu = new Stopwatch();
		static public SfmlDrawingSurface PowierzchniaRys;

		public static void Start()
		{
			StworzRenderWindow();
			Timer.Interval = 1000 / 60;
			Timer.Tick += Render;
			PowierzchniaRys.SizeChanged += Surface_SizeChanged;
			Timer.Start();
			DeltaCzasu.Start();
		}
		private static void StworzRenderWindow()
		{
			if (RenderWindow != null)
			{
				RenderWindow.SetActive(false);
				RenderWindow.Dispose(); //usuwa obecne okno
			}

			var Context = new ContextSettings(24,8,4); //{ AntialiasingLevel = 4 };
			RenderWindow = new RenderWindow(PowierzchniaRys.Handle, Context);
			RenderWindow.SetVerticalSyncEnabled(true);

			RenderWindow.SetActive(true);
		}
		static private void Surface_SizeChanged(object s, EventArgs e)
		{
			StworzRenderWindow(); //tworzy nowe okno renderowania przy zmianie rozmiaru okna
												//zeby uniknac bledow
		}
		static private void Render(object s, EventArgs e)
		{
			RenderWindow.DispatchEvents(); //przetwarza wydarzenia
			//Trace.WriteLine(DeltaTime.Elapsed.TotalSeconds);
			//DebugLabel.Content = DeltaTime.Elapsed.TotalSeconds;
			RenderWindow.Clear(SFML.Graphics.Color.White); //czysci ekran
			foreach(KeyValuePair<Transformable, Vector2f> para in DoPrzesuniecia) //przesuwa wszystkie zadane dla tej klatki
				para.Key.Position += para.Value*(float)DeltaCzasu.Elapsed.TotalSeconds;
			for(int i=0; i<Rysowane.Count; i++) //rysuje
				RenderWindow.Draw(Rysowane[i]);
			RenderWindow.Display();
			DeltaCzasu.Reset();
			DeltaCzasu.Start();
			//ToMove.Clear();
			//PrevTime = TimeSinceStart.ElapsedTime;
		}
		static public void PrzesunSprite(Transformable obj, Vector2f speed)
		{
			if(!DoPrzesuniecia.ContainsKey(obj)) DoPrzesuniecia.Add(obj, speed);
		}
		static public void ZatrzymajSprite(Transformable obj)
		{
			if(DoPrzesuniecia.ContainsKey(obj)) DoPrzesuniecia.Remove(obj);
		}
		public static byte[] BitmapaNaByte(System.Drawing.Bitmap img)
		{
			System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
			return (byte[])converter.ConvertTo(img, typeof(byte[]));
		}
		public static void Zakoncz()
		{
			Timer.Stop();
			Timer.Dispose();
			RenderWindow.Close();
		}

	}
}
