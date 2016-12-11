using System;
using System.Windows;

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
			PrzyciskStart.IsDefault = true;
			Plansza.StworzZBitmapy(Properties.Resources.mapa, Properties.Resources.sciana);
			LogikaGry.Instancja.Wynik(); //wczytuje z pliku
			this.DataContext = LogikaGry.Instancja;
			Rysowanie.PowierzchniaRys = DrawSurface;

			tabsy.Loaded += WczytaloSie;
			LogikaGry.Instancja.GraSkonczona += ZmienKarteNaMenu;
		}

		private void WczytaloSie(object sender, RoutedEventArgs e)
		{
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
