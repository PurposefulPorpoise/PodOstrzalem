using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ProjektObiektowe
{
	sealed class LogikaGry :INotifyPropertyChanged //interfejs do informowania okna o zmianie wlasciwosci
	{
		#region Singleton, max 1 instancja (konstruktor ukryty), potrzebne do bindingu
		private static readonly LogikaGry instancja = new LogikaGry();
		public static LogikaGry Instancja { get { return instancja; } }
		private LogikaGry() { }
		#endregion
		#region Zmiana wlasciwosci, do bindingu
		public event PropertyChangedEventHandler PropertyChanged;
		private void OglosZmianeWlasciwosci(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
		PostacGracza Gracz;
		public void RozpocznijGre()
		{
			Rysowanie.LicznikRysowania.Tick += CoKlatke;
			Gracz = new PostacGracza(Properties.Resources.zgory_niskarozdz, 4, 5);
			Plansza.StworzZBitmapy(Properties.Resources.mapa, Properties.Resources.sciana);
		}
		public void CoKlatke(object s, EventArgs e)
		{
			Gracz.Rusz();

			Rysowanie.Rysuj();
		}

	}
}
