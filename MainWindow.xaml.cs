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
		private Sprite PlayerSprite = new Sprite();
		//private readonly DispatcherTimer Timer = new DispatcherTimer(DispatcherPriority.Send);

		public MainWindow()
		{
			InitializeComponent();
			Rysowanie.PowierzchniaRys = DrawSurface;
			Rysowanie.Rysowane.Add(PlayerSprite);
			Rysowanie.Start();
			DrawSurface.MouseMove += DrawSurface_MouseMove;
			this.KeyDown += MainWindow_KeyDown;
			this.KeyUp += MainWindow_KeyUp;
			this.Closed += MainWindow_Closed;
			TestLabel.Content = "Start";
		}

		private void MainWindow_Closed(object sender, EventArgs e)
		{
			Rysowanie.Zakoncz();
		}

		private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.D) Rysowanie.PrzesunSprite(PlayerSprite, new Vector2f(300f,0f));
			else if(e.Key == Key.A) Rysowanie.PrzesunSprite(PlayerSprite, new Vector2f(-300f, 0f));
		}
		private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.D || e.Key == Key.A) Rysowanie.ZatrzymajSprite(PlayerSprite);
		}

		private void DrawSurface_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//Sprite.Position = GetMousePosition();
		}

		private Vector2f PozycjaMyszy()
		{
			Vector2i ScreenMouse = SFML.Window.Mouse.GetPosition();
			return new Vector2f((float)PointFromScreen(new Point(ScreenMouse.X, ScreenMouse.Y)).X,
				(float)PointFromScreen(new Point(ScreenMouse.X, ScreenMouse.Y)).Y);
		}
	}
}
