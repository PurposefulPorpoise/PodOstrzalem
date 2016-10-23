using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace ProjektObiektowe
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : System.Windows.Window
	{
		private readonly CircleShape Kolko;
		private Sprite Sprite = new Sprite();
		//private readonly DispatcherTimer Timer = new DispatcherTimer(DispatcherPriority.Send);

		public MainWindow()
		{
			InitializeComponent();
			Rendering.Surface = DrawSurface;
			Rendering.ToDraw.Add(Sprite);
			Rendering.Initialize();
			DrawSurface.MouseMove += DrawSurface_MouseMove;
			this.KeyDown += MainWindow_KeyDown;
			this.KeyUp += MainWindow_KeyUp;
			DebugLabel.Content = "Start";
			Kolko = new CircleShape(40f);
			Kolko.FillColor = SFML.Graphics.Color.Black;
			Texture tex = new Texture("obrazek.png");
			tex.Smooth = true;
			Sprite.Texture = tex;
			Sprite.Scale = new Vector2f(0.1f, 0.1f);
			Sprite.Origin = (Vector2f)tex.Size / 2;
			Sprite.Position = new Vector2f(80f, 580f);
		}

		private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.D) Rendering.MoveSprite(Sprite, new Vector2f(400f,0f));
			else if(e.Key == Key.A) Rendering.MoveSprite(Sprite, new Vector2f(-400f, 0f));
		}
		private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.D || e.Key == Key.A) Rendering.StopSprite(Sprite);
		}

		private void DrawSurface_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//Sprite.Position = GetMousePosition();
		}

		private Vector2f GetMousePosition()
		{
			Vector2i ScreenMouse = SFML.Window.Mouse.GetPosition();
			return new Vector2f((float)PointFromScreen(new Point(ScreenMouse.X, ScreenMouse.Y)).X,
				(float)PointFromScreen(new Point(ScreenMouse.X, ScreenMouse.Y)).Y);
		}

	}
}
