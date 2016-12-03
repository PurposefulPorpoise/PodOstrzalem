using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using SFML.Graphics;
using System.Windows;
using System.IO;

namespace ProjektObiektowe
{
	sealed class LogikaGry : INotifyPropertyChanged //interfejs do informowania okna o zmianie wlasciwosci (binding)
	{
		/// <summary>
		/// Rozpoczyna gre i wykonuje kluczowe operacje co klatke (porusza obiekty i woła rysowanie)
		/// </summary>
		#region Singleton, max 1 instancja (konstruktor ukryty), potrzebne do bindingu
		private static readonly LogikaGry instancja = new LogikaGry();
		public static LogikaGry Instancja { get { return instancja; } }
		private LogikaGry() { }
		#endregion
		#region Zmiana wlasciwosci, do bindingu
		public event PropertyChangedEventHandler PropertyChanged;
		int SzerokoscPaskaZycia;
		int _Najdluzej;
		public int PasekZyciaSzerokosc
		{
			get
			{
				return SzerokoscPaskaZycia;
			}
			set
			{
				SzerokoscPaskaZycia = value * 40;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("PasekZyciaSzerokosc"));
			}
		}
		public int Najdluzej
		{
			get
			{
				return _Najdluzej;
			}
			set
			{
				_Najdluzej = (int)Math.Max(Convert.ToInt64(CzasGryS), WynikZPliku);
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Najdluzej"));
			}
		}

		#endregion
		public ulong NrKlatkiGry = 0;
		static Stopwatch LicznikDelty;
		double _DeltaCzasu = 0.0;
		public double DeltaCzasu { get { return _DeltaCzasu; } }
		public PostacGracza Gracz;
		List<Drawable> _Rysowane = new List<Drawable>();
		public List<Drawable> Rysowane { get { return _Rysowane; } }
		List<IRuchomy> Ruchome = new List<IRuchomy>();
		List<IAnimowany> Animowane = new List<IAnimowany>();
		List<Dzialko> Dzialka = new List<Dzialko>();
		List<Pocisk> Pociski = new List<Pocisk>();
		List<Pocisk> PociskiDoUsuniecia = new List<Pocisk>();
		public event EventHandler GraSkonczona;
		double CzasGryS = 0;
		long WynikZPliku;
		public void RozpocznijGre()
		{
			Rysowane.AddRange(Plansza.sciany);
			foreach (var d in Dzialka)
				Rysowane.Add(d.sprite);
			CzasGryS = 0.0;
			Rysowanie.Start();
			Gracz = new PostacGracza(Properties.Resources.zgory_niskarozdz, 4, 5, new SFML.System.Vector2f(560, 560));
			Rysowanie.LicznikRysowania.Tick -= CoKlatke; // -= zeby uniknac duplikowanych subskrybcji
			Rysowanie.LicznikRysowania.Tick += CoKlatke;
			LicznikDelty = Stopwatch.StartNew();
			Ruchome.Add(Gracz);
			Animowane.Add(Gracz);
			Subskrybcje(Application.Current.MainWindow);
			//Application.Current.MainWindow.Closed += (s, e) => LicznikDelty.Stop();
		}
		public void Subskrybcje(Window okno)
		{
			if (LicznikDelty != null && okno != null)
			{
				okno.Deactivated -= Zminimalizowano; // -= zeby uniknac duplikowanych subskrybcji
				okno.Deactivated += Zminimalizowano;
				okno.Activated -= Przywrocono;
				okno.Activated += Przywrocono;
			}
		}
		public void CoKlatke(object s, EventArgs e) //wywolywane co okolo 23ms (idealnie 16ms), daje to ~43fps
		{
			foreach (var element in PociskiDoUsuniecia)
				Pociski.Remove(element);
			PociskiDoUsuniecia.Clear();
			foreach (var element in Ruchome)
				element.Rusz();
			foreach (var pocisk in Pociski) //foreach otwiera liste tylko do odczytu
				pocisk.ReagujNaKolizje();
			Kolizje.ReakcjaPostaciNaKolizje();
			foreach (var element in Animowane)
				element.Animuj(NrKlatkiGry);
			foreach (var dzialko in Dzialka)
				if (NrKlatkiGry > 100 && dzialko.CzyWidziGracza() && DateTime.Now - dzialko.CzasOstatniegoStrzalu >= dzialko.OdstepStrzalow)
				{
					Ruchome.Add(dzialko.Strzel(Gracz.Pozycja, 1.2f*(float)CzasGryS+230f)); //zwraca nowy pocisk i dodaje do listy rysowanych
					Pociski.Add((Pocisk)Ruchome.Last());
				}
				Rysowanie.Rysuj(_Rysowane, new Color(114, 123, 139));  //(147, 169, 131)); orginalny  (143, 143, 177)); fioletowoszary
			NrKlatkiGry++;
			_DeltaCzasu = LicznikDelty.Elapsed.TotalSeconds;
			CzasGryS += _DeltaCzasu;
			if (Gracz.zdrowie == 0)
				ZakonczGre();
			else
			{
				LicznikDelty.Reset(); //liczy czas od ostatniej klatki, z kazda klatką od nowa
				LicznikDelty.Start();
			}
		}
		public void DodajDzialko(Dzialko nowe)
		{
			Dzialka.Add(nowe);
		}
		public void DodajRysowane(Drawable element)
		{
			_Rysowane.Add(element);
		}
		public void ZniszczPocisk(Pocisk element)
		{
			_Rysowane.Remove(element.sprite);
			Ruchome.Remove(element);
			PociskiDoUsuniecia.Add(element);
		}
		public void ZakonczGre()
		{
			Rysowanie.Zakoncz(this, null);
			LicznikDelty.Stop();
			Debug.WriteLine("ZakonczGre()");
			MessageBoxResult klikniete = MessageBox.Show
				(string.Format("Umarłeś\nPrzetrwałeś {0}s", Convert.ToInt64(CzasGryS))
				, "Game over", MessageBoxButton.OK);
			if (klikniete == MessageBoxResult.OK)
			{
				if (GraSkonczona != null)
					GraSkonczona(this, null);
				NrKlatkiGry = 0;
				LicznikDelty = null;
				_DeltaCzasu = 0.0;
				_Rysowane.Clear();
				Ruchome.Clear();
				Animowane.Clear();
				//Dzialka.Clear();
				Pociski.Clear();
				PociskiDoUsuniecia.Clear();
				Gracz.zdrowie = 3;
				Wynik();
			}
		}
		public void Wynik()
		{
			try
			{
				if (!File.Exists("najlepszywynik"))
					File.CreateText("najlepszywynik").Close();
				//utrudnia reczna zamiane kodujac w systemie 64
				string wczytane = File.ReadAllText("najlepszywynik");
				if (wczytane.Length > 0)
					wczytane = Encoding.Default.GetString(Convert.FromBase64String(wczytane));
				Int64.TryParse(wczytane, out WynikZPliku);
				Najdluzej = Convert.ToInt32(CzasGryS);
				File.WriteAllText("najlepszywynik", Convert.ToBase64String(Encoding.Default.GetBytes(Najdluzej.ToString())));
			}
			catch { /*no trudno*/ }
		}
		void Zminimalizowano(object sender, EventArgs e)
		{
			if(LicznikDelty != null)
				LicznikDelty.Stop();
		}
		void Przywrocono(object sender, EventArgs e)
		{
			if (LicznikDelty != null)
				LicznikDelty.Start();
		}
	}
}
