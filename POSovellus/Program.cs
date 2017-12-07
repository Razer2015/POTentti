using POData;
using POLuokat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace POSovellus
{
    class Program
    {
        static CustomerRepository customerRepo;

        static void AsetaDataDirectory() {
            // Asetetaan muuttuja DataDirectory, jota käytetään yhteysmerkkijonossa  
            // tiedostossa App.config

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relative = @"..\..\App_Data\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);
        }

        static void Main(string[] args) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            AsetaDataDirectory();

            var connectionSettings = ConfigurationManager.ConnectionStrings["DB"];
            customerRepo = new CustomerRepository(connectionSettings.ConnectionString);

            // Looppaillaan aloitusnäkymää
            while (AloitusNakyma());
        }

        /// <summary>
        /// Tulostaa ohjelman otsikon
        /// </summary>
        static void TulostaOtsikko() {
            string otsikko = "Northwind-asiakkaat";
            Console.Title = otsikko;
            Console.Clear();
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - otsikko.Length) / 2, Console.CursorTop);
            Console.WriteLine(otsikko);
            Console.WriteLine();
        }

        /// <summary>
        /// Tulostaa virheellinen syöte virhetekstin (Ei ollut tehtävänannossa)
        /// </summary>
        static void TulostaVirhe(string virheTeksti = "Antamasi syöte on virheellinen. Ole hyvä ja yritä uudelleen.", bool odotaKayttajaa = false) {
            var variEnnen = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(virheTeksti);
            Console.ForegroundColor = variEnnen;
            if (odotaKayttajaa) {
                Console.WriteLine("Paina Enter.");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Tulostaa tekstin vihreällä värillä (Ei ollut tehtävänannossa)
        /// </summary>
        static void TulostaOnnistunut(string teksti) {
            var variEnnen = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(teksti);
            Console.ForegroundColor = variEnnen;
        }

        /// <summary>
        /// Tulostaa aloitusvalikon
        /// </summary>
        /// <returns>true -> continue and false -> exit</returns>
        static bool AloitusNakyma() {
            TulostaOtsikko();

            Console.WriteLine("1. Hae");
            Console.WriteLine("2. Lisää");
            Console.WriteLine("3. Muuta");
            Console.WriteLine("4. Poista");
            Console.WriteLine("5. Lopeta");
            Console.Write("Valitse: ");

            var syote = Console.ReadLine();
            if (int.TryParse(syote, out var ulos)) {
                switch (ulos) {
                    case 1:
                        Haku();
                        break;
                    case 2:
                        Lisaa();
                        break;
                    case 3:
                        Muuta();
                        break;
                    case 4:
                        Poista();
                        break;
                    case 5:
                        // Lopetetaan ohjelma
                        return (false);
                    default:
                        // Annettu arvo ei ollut arvoalueella
                        TulostaVirhe(odotaKayttajaa: true);
                        break;
                }
            }
            else { // Annettu arvo ei ollut luku
                TulostaVirhe(odotaKayttajaa: true);
            }

            return (true);
        }

        static void Haku() {
            TulostaOtsikko();

            Console.WriteLine("Asiakastietojen haku");
            Console.WriteLine("Valitse hakukriteeri");
            Console.WriteLine("1. Nimi");
            Console.WriteLine("2. Kaupunki");
            Console.WriteLine("3. Maa");
            Console.WriteLine("4. Palaa takaisin");
            Console.Write("Valitse: ");

            var syote = Console.ReadLine();
            if (int.TryParse(syote, out var ulos) && ulos > 0 && ulos <= 3) {
                Console.Write("Anna haettavien alku: ");
                var haku = Console.ReadLine();
                List<Customer> asiakkaat = null;
                switch (ulos) {
                    case 1:
                        asiakkaat = customerRepo.SearchByName(haku);
                        break;
                    case 2:
                        asiakkaat = customerRepo.SearchByCity(haku);
                        break;
                    case 3:
                        asiakkaat = customerRepo.SearchByCountry(haku);
                        break;
                }

                Console.WriteLine($"Löydetyt asiakkaat: {asiakkaat.Count} kpl");
                for (int i = 0; i < asiakkaat.Count; i++) {
                    var asiakas = asiakkaat[i];
                    Console.WriteLine($"{i + 1}. asiakas: {asiakas.CompanyName}, {asiakas.City} {asiakas.Country}");
                    var tilaukset = asiakas.Orders;
                    tilaukset.ForEach(x => Console.WriteLine("  Tilaus: {0}, tuotteita {1}, arvo yhteensä {2:0.00}",
                        x.OrderID, x.OrderDetails.Count, x.OrderDetails.Sum(y => (y.UnitPrice * y.Quantity))));

                    if (((i + 1) < asiakkaat.Count)) {
                        Console.WriteLine("Seuraava painamalla Enter.");
                        Console.ReadKey();
                    }
                }

                Console.WriteLine("Paina Enter.");
                Console.ReadKey();
            }
        }

        static void Lisaa() {
            TulostaOtsikko();

            Console.WriteLine("Uusi asiakas");
            Console.Write("Anna tunnus: ");
            var tunnus = Console.ReadLine();
            // Pientä virheentarkistusta
            if (tunnus.Length <= 0 || tunnus.Length > 5) {
                TulostaVirhe(virheTeksti: "Tunnus ei saa olla tyhjä ja täytyy olla maksimissaan 5 merkkiä pitkä.", odotaKayttajaa: true);
                return;
            }
            Console.Write("Anna nimi: ");
            var nimi = Console.ReadLine();
            Console.Write("Anna maa: ");
            var maa = Console.ReadLine();
            Console.Write("Anna kaupunki: ");
            var kaupunki = Console.ReadLine();

            try {
                var asiakas = new Customer();
                asiakas.CustomerID = tunnus;
                asiakas.CompanyName = nimi;
                asiakas.Country = maa;
                asiakas.City = kaupunki;

                if (customerRepo.Add(asiakas)) {
                    TulostaOnnistunut("Asiakas lisätty.");
                }
                else {
                    TulostaVirhe("Asiakasta ei lisätty.");
                }
            }
            catch (ApplicationException ex) {
                TulostaVirhe("Asiakasta ei lisätty.");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Paina Enter.");
            Console.ReadKey();
        }

        static void Muuta() {
            TulostaOtsikko();

            Console.WriteLine("Asiakkaan muuttaminen");
            Console.Write("Anna muutettavan asiakkaan tunnus: ");
            var tunnus = Console.ReadLine();
            
            try {
                Customer asiakas = customerRepo.Search(tunnus);

                Console.WriteLine("Asiakkaan tiedot: {0} {1}, {2} {3}",
                    asiakas.CustomerID, asiakas.CompanyName,
                    asiakas.City, asiakas.Country);

                Console.Write("Anna uusi nimi tai tyhjä: ");
                var nimi = Console.ReadLine();
                Console.Write("Anna uusi kaupunki tai tyhjä: ");
                var kaupunki = Console.ReadLine();
                Console.Write("Anna uusi maa tai tyhjä: ");
                var maa = Console.ReadLine();

                if (!string.IsNullOrEmpty(nimi)) {
                    asiakas.CompanyName = nimi;
                }
                if (!string.IsNullOrEmpty(maa)) {
                    asiakas.Country = maa;
                }
                if (!string.IsNullOrEmpty(kaupunki)) {
                    asiakas.City = kaupunki;
                }

                try {
                    if (customerRepo.Change(asiakas)) {
                        TulostaOnnistunut("Asiakas muutettu.");
                    }
                    else {
                        TulostaVirhe("Asiakasta ei muutettu.");
                    }
                }
                catch (ApplicationException ex) {
                    TulostaVirhe("Asiakasta ei muutettu.");
                    Console.WriteLine(ex.Message);
                }
            }
            catch (ApplicationException) {
                TulostaVirhe("Asiakasta ei löytynyt.");
            }
            catch (Exception e) {
                Console.WriteLine($"Error: {e.Message}");
            }

            Console.WriteLine("Paina Enter.");
            Console.ReadKey();
        }

        static void Poista() {
            TulostaOtsikko();

            Console.WriteLine("Asiakkaan poistaminen");

            var asiakkaat = customerRepo.SearchAll();
            var tilaamattomat = asiakkaat.Where(x => x.Orders.Count <= 0).ToList();
            tilaamattomat.ForEach(x => Console.WriteLine("{0} {1}, {2} {3}",
                    x.CustomerID, x.CompanyName,
                    x.City, x.Country));

            Console.Write("Anna poistettavan tunnus: ");
            var tunnus = Console.ReadLine();

            if (tilaamattomat.Any(x => x.CustomerID.Equals(tunnus.PadRight(5), StringComparison.OrdinalIgnoreCase))) {
                try {
                    Console.Write($"Haluatko varmasti poistaa asiakkaan {tunnus} (k/e)? ");
                    var vahvistus = Console.ReadLine();
                    if (vahvistus.Equals("k", StringComparison.OrdinalIgnoreCase)) {
                        if (customerRepo.Delete(tunnus)) {
                            TulostaOnnistunut("Asiakas poistettu.");
                        }
                        else {
                            TulostaVirhe("Asiakkaan poisto epäonnistui.");
                        }
                    }
                    else {
                        TulostaVirhe("Asiakkaan poisto keskeytetty.");
                    }
                }
                catch (ApplicationException ex) {
                    TulostaVirhe("Asiakkaan poisto epäonnistui.");
                    Console.WriteLine(ex.Message);
                }
            }
            else {
                TulostaVirhe("Kyseistä asiakasta ei ole tai sitä ei voi poistaa.");
            }

            Console.WriteLine("Paina Enter.");
            Console.ReadKey();
        }
    }
}
