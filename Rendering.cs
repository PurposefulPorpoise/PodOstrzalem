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
	static class Rendering
	{
		public static List<Drawable> ToDraw = new List<Drawable>();
		static private Dictionary<Transformable, Vector2f> ToMove = new Dictionary<Transformable, Vector2f>();
		static private RenderWindow RenderWindow;
		static private System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();
		static public Stopwatch DeltaTime = new Stopwatch();
		static public SfmlDrawingSurface Surface;

		public static void Initialize()
		{
			CreateRenderWindow();
			Timer.Interval = 1000 / 60;
			Timer.Tick += Render;
			Surface.SizeChanged += Surface_SizeChanged;
			Timer.Start();
			DeltaTime.Start();
		}
		private static void CreateRenderWindow()
		{
			if (RenderWindow != null)
			{
				RenderWindow.SetActive(false);
				RenderWindow.Dispose(); //usuwa obecne okno
			}

			var Context = new ContextSettings() { AntialiasingLevel = 4 };
			RenderWindow = new RenderWindow(Surface.Handle, Context);
			RenderWindow.SetVerticalSyncEnabled(true);

			RenderWindow.SetActive(true);
		}
		static private void Surface_SizeChanged(object s, EventArgs e)
		{
			CreateRenderWindow(); //tworzy nowe okno renderowania przy zmianie rozmiaru okna
												//zeby uniknac bledow
		}
		static private void Render(object s, EventArgs e)
		{
			RenderWindow.DispatchEvents(); //przetwarza wydarzenia
			//Trace.WriteLine(DeltaTime.Elapsed.TotalSeconds);
			//DebugLabel.Content = DeltaTime.Elapsed.TotalSeconds;
			RenderWindow.Clear(SFML.Graphics.Color.White); //czysci ekran
			foreach(KeyValuePair<Transformable, Vector2f> pair in ToMove) //przesuwa wszystkie zadane dla tej klatki
				pair.Key.Position += pair.Value*(float)DeltaTime.Elapsed.TotalSeconds;
			for(int i=0; i<ToDraw.Count; i++) //rysuje
				RenderWindow.Draw(ToDraw[i]);
			RenderWindow.Display();
			DeltaTime.Reset();
			DeltaTime.Start();
			//ToMove.Clear();
			//PrevTime = TimeSinceStart.ElapsedTime;
		}
		static public void MoveSprite(Transformable obj, Vector2f speed)
		{
			if(!ToMove.ContainsKey(obj)) ToMove.Add(obj, speed);
		}
		static public void StopSprite(Transformable obj)
		{
			if(ToMove.ContainsKey(obj)) ToMove.Remove(obj);
		}

	}
}
