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
			this.Loaded += Rysowanie.UstawWidok;
			LogikaGry.Instancja.RozpocznijGre(); //to pewnie trzeba bedzie przeniesc do OnClick przycisku start w menu glownym
		}

		//Vector2f PozycjaMyszy()
		//{
		//	Vector2i ScreenMouse = SFML.Window.Mouse.GetPosition();
		//	return new Vector2f((float)PointFromScreen(new Point(ScreenMouse.X, ScreenMouse.Y)).X,
		//		(float)PointFromScreen(new Point(ScreenMouse.X, ScreenMouse.Y)).Y);
		//}
	}
}
