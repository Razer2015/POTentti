using POData;
using POLuokat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSovellus
{
    class Program
    {
        static AsiakasRepository asiakasRepo;

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

            var yhteysAsetukset = ConfigurationManager.ConnectionStrings["DB"];
            asiakasRepo = new AsiakasRepository(yhteysAsetukset.ConnectionString);

            AloitusNakyma();
        }

        static void TulostaOtsikko() {
            string otsikko = "Northwind-asiakkaat";
            Console.Clear();
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - otsikko.Length) / 2, Console.CursorTop);
            Console.WriteLine(otsikko);
            Console.WriteLine();
        }

        static void AloitusNakyma() {
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
                }
            }
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
                List<Asiakas> asiakkaat = null;
                switch (ulos) {
                    case 1:
                        asiakkaat = asiakasRepo.HaeByNimi(haku);
                        break;
                    case 2:
                        asiakkaat = asiakasRepo.HaeByCity(haku);
                        break;
                    case 3:
                        asiakkaat = asiakasRepo.HaeByCountry(haku);
                        break;
                }

                Console.WriteLine($"Löydetyt asiakkaat: {asiakkaat.Count} kpl");
                for (int i = 0; i < asiakkaat.Count; i++) {
                    var asiakas = asiakkaat[i];
                    Console.WriteLine($"{i + 1}. asiakas: {asiakas.CompanyName}, {asiakas.City} {asiakas.Country}");
                    var tilaukset = asiakas.Tilaukset;
                    tilaukset.ForEach(x => Console.WriteLine("  Tilaus: {0}, tuotteita {1}, arvo yhteensä {2:0.00}",
                        x.Id, x.TilausRivit.Count, x.TilausRivit.Sum(y => (y.UnitPrice * y.Quantity))));

                    if (((i + 1) < asiakkaat.Count)) {
                        Console.WriteLine("Seuraava painamalla Enter.");
                        Console.ReadKey();
                    }
                }

                Console.WriteLine("Paina Enter.");
                Console.ReadKey();
            }

            AloitusNakyma();
        }

        static void Lisaa() {
            TulostaOtsikko();

            Console.WriteLine("Uusi asiakas");
            Console.Write("Anna tunnus: ");
            var tunnus = Console.ReadLine();
            Console.Write("Anna nimi: ");
            var nimi = Console.ReadLine();
            Console.Write("Anna maa: ");
            var maa = Console.ReadLine();
            Console.Write("Anna kaupunki: ");
            var kaupunki = Console.ReadLine();

            try {
                var asiakas = new Asiakas();
                asiakas.Id = tunnus;
                asiakas.CompanyName = nimi;
                asiakas.Country = maa;
                asiakas.City = kaupunki;

                if (asiakasRepo.Lisaa(asiakas)) {
                    Console.WriteLine("Asiakas lisätty.");
                }
                else {
                    Console.WriteLine("Asiakasta ei lisätty.");
                }
            }
            catch (ApplicationException ex) {
                Console.WriteLine("Asiakasta ei lisätty.");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Paina Enter.");
            Console.ReadKey();

            AloitusNakyma();
        }

        static void Muuta() {
            TulostaOtsikko();

            Console.WriteLine("Asiakkaan muuttaminen");
            Console.Write("Anna muutettavan asiakkaan tunnus: ");
            var tunnus = Console.ReadLine();
            
            try {
                Asiakas asiakas = asiakasRepo.Hae(tunnus);

                Console.WriteLine("Asiakkaan tiedot: {0} {1}, {2} {3}",
                    asiakas.Id, asiakas.CompanyName,
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
                    if (asiakasRepo.Muuta(asiakas)) {
                        Console.WriteLine("Asiakas muutettu.");
                    }
                    else {
                        Console.WriteLine("Asiakasta ei muutettu.");
                    }
                }
                catch (ApplicationException ex) {
                    Console.WriteLine("Asiakasta ei muutettu.");
                    Console.WriteLine(ex.Message);
                }
            }
            catch (ApplicationException) {
                Console.WriteLine("Asiakasta ei löytynyt.");
            }
            catch (Exception e) {
                Console.WriteLine($"Error: {e.Message}");
            }

            Console.WriteLine("Paina Enter.");
            Console.ReadKey();

            AloitusNakyma();
        }

        static void Poista() {
            TulostaOtsikko();

            Console.WriteLine("Asiakkaan poistaminen");

            var asiakkaat = asiakasRepo.HaeKaikki();
            var tilaamattomat = asiakkaat.Where(x => x.Tilaukset.Count <= 0).ToList();
            tilaamattomat.ForEach(x => Console.WriteLine("{0} {1}, {2} {3}",
                    x.Id, x.CompanyName,
                    x.City, x.Country));

            Console.Write("Anna poistettavan tunnus: ");
            var tunnus = Console.ReadLine();

            if (tilaamattomat.Any(x => x.Id.Equals(tunnus.PadRight(5), StringComparison.OrdinalIgnoreCase))) {
                try {
                    Console.Write($"Haluatko varmasti poistaa asiakkaan {tunnus} (k/e)? ");
                    var vahvistus = Console.ReadLine();
                    if (vahvistus.Equals("k", StringComparison.OrdinalIgnoreCase)) {
                        if (asiakasRepo.Poista(tunnus)) {
                            Console.WriteLine("Asiakas poistettu.");
                        }
                        else {
                            Console.WriteLine("Asiakkaan poisto epäonnistui.");
                        }
                    }
                    else {
                        Console.WriteLine("Asiakkaan poisto keskeytetty.");
                    }
                }
                catch (ApplicationException ex) {
                    Console.WriteLine("Asiakkaan poisto epäonnistui.");
                    Console.WriteLine(ex.Message);
                }
            }
            else {
                Console.WriteLine("Kyseistä asiakasta ei ole tai sitä ei voi poistaa.");
            }

            Console.WriteLine("Paina Enter.");
            Console.ReadKey();

            AloitusNakyma();
        }
    }
}
