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
		PostacGracza Gracz;
		public MainWindow() //poczatek programu
		{
			InitializeComponent();
			Plansza.StworzZBitmapy(Properties.Resources.mapa, Properties.Resources.sciana);
			Gracz = new PostacGracza(Properties.Resources.zgory_niskarozdz, 4, 5);
			Rysowanie.PowierzchniaRys = DrawSurface;
			Rysowanie.Start();
			this.Closed += MainWindow_Closed;
			Rysowanie.LicznikRysowania.Tick += LicznikRysowania_Tick;
			System.Diagnostics.Trace.WriteLine(Rysowanie.Rysowane.Count);
		}

		private void LicznikRysowania_Tick(object sender, EventArgs e)
		{
			//TestLabel.Content = Gracz.KierunekSprita.ToString();
		}

		private void MainWindow_Closed(object sender, EventArgs e)
		{
			Rysowanie.Zakoncz();
		}

		private Vector2f PozycjaMyszy()
		{
			Vector2i ScreenMouse = SFML.Window.Mouse.GetPosition();
			return new Vector2f((float)PointFromScreen(new Point(ScreenMouse.X, ScreenMouse.Y)).X,
				(float)PointFromScreen(new Point(ScreenMouse.X, ScreenMouse.Y)).Y);
		}
	}
}
