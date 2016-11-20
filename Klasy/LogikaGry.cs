using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ProjektObiektowe
{
	sealed class LogikaGry :INotifyPropertyChanged //interfejs do informowania okna o zmianie wlasciwosci (binding)
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
		public ulong NrKlatkiGry = 0;
		public PostacGracza Gracz;
		List<IRuchomy> Ruchome = new List<IRuchomy>();
		List<IAnimowany> Animowane = new List<IAnimowany>();
		List<Dzialko> Dzialka = new List<Dzialko>();
		public void RozpocznijGre()
		{
			Rysowanie.LicznikRysowania.Tick += CoKlatke;
			Gracz = new PostacGracza(Properties.Resources.zgory_niskarozdz, 4, 5, new SFML.System.Vector2f(560,560));
			Plansza.StworzZBitmapy(Properties.Resources.mapa, Properties.Resources.sciana);
			Ruchome.Add(Gracz);
			Animowane.Add(Gracz);
		}
		public void CoKlatke(object s, EventArgs e) //wywolywane co okolo 22ms (idealnie 16ms)
		{
			foreach (var element in Ruchome)
				element.Rusz();
			Kolizje.ReakcjaPostaciNaKolizje();
			foreach (var element in Animowane)
				element.Animuj(NrKlatkiGry);
			Rysowanie.Rysuj();
			NrKlatkiGry++;
		}
		public void DodajDzialko(Dzialko nowe)
		{
			Dzialka.Add(nowe);
		}

	}
}
