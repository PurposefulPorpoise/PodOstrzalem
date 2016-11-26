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
		
		public MainWindow() 
		{
			InitializeComponent();
			this.DataContext = LogikaGry.Instancja;
			
			Rysowanie.PowierzchniaRys = DrawSurface;
			this.Loaded += MainWindow_Loaded;
						//dopiero po calkowitym zaladowaniu powierzchniarys ma wlasciwy rozmiar
			LogikaGry.Instancja.RozpocznijGre(); //to pewnie trzeba bedzie przeniesc do OnClick przycisku start w menu glownym
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			grid.RowDefinitions[0].Height = new GridLength(DrawSurface.Width * 640 / 1280f);
			//this.Height = this.Width * 640 / 1 -40;
		}

		//Vector2f PozycjaMyszy()
		//{
		//	Vector2i ScreenMouse = SFML.Window.Mouse.GetPosition();
		//	return new Vector2f((float)PointFromScreen(new Point(ScreenMouse.X, ScreenMouse.Y)).X,
		//		(float)PointFromScreen(new Point(ScreenMouse.X, ScreenMouse.Y)).Y);
		//}
	}
}
