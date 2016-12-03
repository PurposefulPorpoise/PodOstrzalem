using System;
using System.Windows;

namespace ProjektObiektowe
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : System.Windows.Window
	{
		int SzerokoscPowierzchni;
		public MainWindow()
		{

			InitializeComponent();
			LogikaGry.Instancja.Wynik(); //wczytuje z pliku
			this.DataContext = LogikaGry.Instancja;
			Rysowanie.PowierzchniaRys = DrawSurface;
			tabsy.Loaded += WczytaloSie;
			//tabsy.SelectionChanged += TabZmieniony;
			LogikaGry.Instancja.GraSkonczona += ZmienKarteNaMenu;
		}

		private void WczytaloSie(object sender, RoutedEventArgs e)
		{
			SzerokoscPowierzchni = DrawSurface.Width;
			tabsy.SelectedIndex = 0;
		}
		private void StartWcisnieto(object sender, EventArgs e)
		{
			tabsy.SelectedItem = TabGry;
			LogikaGry.Instancja.RozpocznijGre();
		}
		private void WyjscieWcisnieto(object sender, EventArgs e)
		{
			this.Close();
		}
		public void ZmienKarteNaMenu(object o, EventArgs e)
		{
			tabsy.SelectedIndex = 0;
		}

	}
}
