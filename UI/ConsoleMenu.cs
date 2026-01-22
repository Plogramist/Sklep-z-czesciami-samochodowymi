using System;
using SklepSamochodowy.Data;

namespace SklepSamochodowy.UI
{
    public class ConsoleMenu
    {
        private readonly CarPartRepository _repo;

        public ConsoleMenu(CarPartRepository repo)
        {
            _repo = repo;
        }

        public void Run()
        {
            bool exit = false;

            while (!exit)
            {
                PrintHeader();
                PrintMenu();

                Console.Write("Wybierz opcję: ");
                string choice = (Console.ReadLine() ?? "").Trim();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddPart();
                            break;
                        case "2":
                            ShowAll();
                            break;
                        case "3":
                            EditPart();
                            break;
                        case "4":
                            DeletePart();
                            break;
                        case "5":
                            SearchByName();
                            break;
                        case "6":
                            ShowStats();
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Nieprawidłowa opcja.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd: {ex.Message}");
                }

                if (!exit)
                {
                    Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private void PrintHeader()
        {
            Console.WriteLine("=== Sklep z częściami samochodowymi (CRUD) ===\n");
        }

        private void PrintMenu()
        {
            Console.WriteLine("1 - Dodaj część (Create)");
            Console.WriteLine("2 - Wyświetl wszystkie części (Read)");
            Console.WriteLine("3 - Edytuj część (Update)");
            Console.WriteLine("4 - Usuń część (Delete)");
            Console.WriteLine("5 - Wyszukaj część po nazwie");
            Console.WriteLine("6 - Statystyki magazynu");
            Console.WriteLine("0 - Wyjście\n");
        }

        private void AddPart()
        {
            Console.WriteLine("--- Dodawanie części ---");
            string name = ReadNonEmpty("Nazwa: ");
            string category = ReadNonEmpty("Kategoria: ");
            decimal price = ReadDecimal("Cena (PLN): ");
            int qty = ReadInt("Ilość: ");

            var part = _repo.Add(name, category, price, qty);
            Console.WriteLine("\nDodano:");
            Console.WriteLine(part);
        }

        private void ShowAll()
        {
            Console.WriteLine("--- Lista części ---");
            var parts = _repo.GetAll();

            if (parts.Count == 0)
            {
                Console.WriteLine("Brak danych (pamięć programu jest pusta).");
                return;
            }

            foreach (var p in parts)
                Console.WriteLine(p);
        }

        private void EditPart()
        {
            Console.WriteLine("--- Edycja części ---");
            int id = ReadInt("Podaj ID: ");

            var part = _repo.GetById(id);
            if (part == null)
            {
                Console.WriteLine("Nie znaleziono części o takim ID.");
                return;
            }

            Console.WriteLine("Aktualne dane:");
            Console.WriteLine(part);

            string name = ReadNonEmpty("Nowa nazwa: ");
            string category = ReadNonEmpty("Nowa kategoria: ");
            decimal price = ReadDecimal("Nowa cena (PLN): ");
            int qty = ReadInt("Nowa ilość: ");

            bool ok = _repo.Update(id, name, category, price, qty);
            Console.WriteLine(ok ? "Zaktualizowano." : "Nie udało się zaktualizować.");
        }

        private void DeletePart()
        {
            Console.WriteLine("--- Usuwanie części ---");
            int id = ReadInt("Podaj ID: ");

            var part = _repo.GetById(id);
            if (part == null)
            {
                Console.WriteLine("Nie znaleziono części o takim ID.");
                return;
            }

            Console.WriteLine("Do usunięcia:");
            Console.WriteLine(part);

            Console.Write("Potwierdź usunięcie (t/n): ");
            string confirm = (Console.ReadLine() ?? "").Trim().ToLower();

            if (confirm != "t")
            {
                Console.WriteLine("Anulowano.");
                return;
            }

            bool ok = _repo.Delete(id);
            Console.WriteLine(ok ? "Usunięto." : "Nie udało się usunąć.");
        }

        private void SearchByName()
        {
            Console.WriteLine("--- Wyszukiwanie ---");
            string phrase = ReadNonEmpty("Wpisz fragment nazwy: ");

            var results = _repo.SearchByName(phrase);
            if (results.Count == 0)
            {
                Console.WriteLine("Brak wyników.");
                return;
            }

            Console.WriteLine("\nWyniki:");
            foreach (var p in results)
                Console.WriteLine(p);
        }

        private void ShowStats()
        {
            Console.WriteLine("--- Statystyki ---");
            var stats = _repo.GetStats();

            Console.WriteLine($"Liczba pozycji: {stats.items}");
            Console.WriteLine($"Łączna ilość sztuk: {stats.totalQty}");
            Console.WriteLine($"Łączna wartość magazynu: {stats.totalValue:0.00} PLN");

            if (stats.mostExpensive != null)
            {
                Console.WriteLine($"Najdroższa część: {stats.mostExpensive.Name} ({stats.mostExpensive.Price:0.00} PLN)");
            }
        }

        private string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim();

                if (input.Length > 0)
                    return input;

                Console.WriteLine("Pole nie może być puste.");
            }
        }

        private int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim();

                if (int.TryParse(input, out int value) && value >= 0)
                    return value;

                Console.WriteLine("Wpisz poprawną liczbę całkowitą (>= 0).");
            }
        }

        private decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? "").Trim().Replace(',', '.');

                if (decimal.TryParse(input, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out decimal value) && value >= 0)
                    return value;

                Console.WriteLine("Wpisz poprawną liczbę (>= 0).");
            }
        }
    }
}
