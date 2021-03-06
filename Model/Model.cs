﻿using System.Linq;

namespace Salon_samochodowy.Model
{
    using DAL.Encje;
    using DAL.Repozytoria;
    using System.Collections.ObjectModel;

    public class Model
    {
        //bazy danych w postaci kolekcji
        public ObservableCollection<Pracownik> Pracownicy { get; set; } = new ObservableCollection<Pracownik>();
        public ObservableCollection<Samochod> Samochody { get; set; } = new ObservableCollection<Samochod>();
        public ObservableCollection<Sprzedaz> Sprzedaze { get; set; } = new ObservableCollection<Sprzedaz>();

        //obiekt w któym przechowujemy dane o zalogowanym pracowniku
        public Pracownik Zalogowany { get; set; }

        public Model()
        {
            //pobieramy z MYSQLa dane do kolekcjii
            var pracownicy = RepoPracowników.PobierzWszystkichPracownikow();
            var samochody = RepoSamochody.PobierzWszystkieSamochody();
            var sprzedaze = RepoSprzedazy.PobierzWszystkieSprzedaze();

            foreach (var p in pracownicy)
            {
                Pracownicy.Add(p);
            }
            foreach (var s in samochody)
            {
                Samochody.Add(s);
            }
            foreach (var sp in sprzedaze)
            {
                Sprzedaze.Add(sp);
            }

            Zalogowany = null;
        }

        // Szukanie obiektów w bazach po ID
        private Pracownik ZnajdzPracownikaPoID(sbyte id)
        {
            return Pracownicy.FirstOrDefault(p => p.Id == id);
        }

        private Samochod ZnajdzSamochodPoID(sbyte id)
        {
            return Samochody.FirstOrDefault(s => s.Id == id);
        }

        private Sprzedaz ZnajdzSprzedazPoID(sbyte id)
        {
            return Sprzedaze.FirstOrDefault(sp => sp.IdSprzedazy == id);
        }

        public Pracownik ZnajdzPracownikaPoLoginie(string login)
        {
            return Pracownicy.FirstOrDefault(p => p.Login == login);
        }


        // Dodawanie do baz: Pracownika, Samochodu i Sprzedaży
        public bool IfPracownikInDB(Pracownik pracownik) => Pracownicy.Contains(pracownik);
        public bool DodajPracownika(Pracownik pracownik)
        {
            if (IfPracownikInDB(pracownik)) return false;
            if (!RepoPracowników.DodajPracownikaDoBazy(pracownik)) return false;
            Pracownicy.Add(pracownik);
            return true;
        }

        public bool IfSamochodInDB(Samochod samochod) => Samochody.Contains(samochod);
        public bool DodajSamochod(Samochod samochod)
        {
            if (IfSamochodInDB(samochod)) return false;
            if (!RepoSamochody.DodajSamochodDoBazy(samochod)) return false;
            Samochody.Add(samochod);
            return true;
        }

        public bool IfSprzedazInDB(Sprzedaz sprzedaz) => Sprzedaze.Contains(sprzedaz);
        public bool DodajSprzedaz(Sprzedaz sprzedaz)
        {
            if (IfSprzedazInDB(sprzedaz)) return false;
            if (!RepoSprzedazy.DodajSprzedazDoBazy(sprzedaz)) return false;
            Sprzedaze.Add(sprzedaz);
            return true;
        }


        //Edycja Pracownika i Samochodu w bazach
        public bool EdytujPracownika(Pracownik pracownik, sbyte idPracownika)
        {
            if (!RepoPracowników.EdytujPracownika(pracownik, idPracownika)) return false;
            for (int i = 0; i < Pracownicy.Count; i++)
            {
                if (Pracownicy[i].Id != idPracownika) continue;
                pracownik.Id = idPracownika;
                Pracownicy[i] = new Pracownik(pracownik);
            }

            return true;
        }

        public bool EdytujSamochod(Samochod samochod, sbyte idSamochodu)
        {
            if (!RepoSamochody.EdytujSamochod(samochod, idSamochodu)) return false;
            for (int i = 0; i < Samochody.Count; i++)
            {
                if (Samochody[i].Id != idSamochodu) continue;
                samochod.Id = idSamochodu;
                Samochody[i] = new Samochod(samochod);
            }

            return true;
        }


        //Usuwanie Pracownika i Samochodu z baz
        public bool UsunPracownika(sbyte idPracownika)
        {
            if (!RepoPracowników.UsunPracownika(idPracownika)) return false;
            for (int i = 0; i < Pracownicy.Count; i++)
            {
                if (Pracownicy[i].Id != idPracownika) continue;
                Pracownicy.RemoveAt(i);
            }
            return true;
        }

        public bool UsunSamochod(sbyte idSamochodu)
        {
            if (!RepoSamochody.UsunSamochod(idSamochodu)) return false;
            for (int i = 0; i < Samochody.Count; i++)
            {
                if (Samochody[i].Id != idSamochodu) continue;
                Samochody.RemoveAt(i);
            }
            return true;
        }


        //Wyciągnięcie ID w MYSQL po ID z listy
        public sbyte CheckIDPracownika(sbyte idZlisty)
        {
            var s = Pracownicy[idZlisty].Id;

            return (sbyte)s;
        }
        public sbyte CheckIDSamochodu(sbyte idZlisty)
        {
            var s = Samochody[idZlisty].Id;

            return (sbyte)s;
        }


    }
  
}