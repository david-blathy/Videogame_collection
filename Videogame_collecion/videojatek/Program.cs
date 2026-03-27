using System;
using System.IO;
using System.Linq;
using videojatek;

namespace VideogamesCollection
{
    internal class Program
    {
        private static GameRepository _repo = new GameRepository();
        private const string CsvFilePath = "games.csv";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            LoadInitialData();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=== Videogames collection ===");
                Console.WriteLine("1 - Elem hozzáadása");
                Console.WriteLine("2 - Lista megjelenítése");
                Console.WriteLine("3 - Keresés név alapján");
                Console.WriteLine("4 - Szűrés kategória szerint");
                Console.WriteLine("5 - Rendezés értékelés szerint (csökkenő)");
                Console.WriteLine("6 - CSV Mentés");
                Console.WriteLine("7 - CSV Betöltés");
                Console.WriteLine("8 - HTML export");
                Console.WriteLine("0 - Kilépés");
                Console.Write("Választás: ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        AddGame();
                        break;
                    case "2":
                        ListGames();
                        break;
                    case "3":
                        SearchGames();
                        break;
                    case "4":
                        FilterByCategory();
                        break;
                    case "5":
                        ListSortedByRating();
                        break;
                    case "6":
                        SaveCsv();
                        break;
                    case "7":
                        LoadCsv();
                        break;
                    case "8":
                        ExportHtml();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Ismeretlen menüpont.");
                        break;
                }
            }
        }

        private static void LoadInitialData()
        {
            
            if (File.Exists(CsvFilePath))
            {
                var games = CsvHandler.Load(CsvFilePath);
                foreach (var g in games)
                    _repo.Add(g);
            }
            else
            {
                
                _repo.Add(new Game(_repo.NextId(), "The Witcher 3", "RPG",
                    "Open world fantasy RPG.", 10, 7, true));
                _repo.Add(new Game(_repo.NextId(), "Dark Souls III", "Action",
                    "Challenging action RPG.", 9, 9, true));
                _repo.Add(new Game(_repo.NextId(), "Minecraft", "Sandbox",
                    "Creative building and survival.", 8, 5, false));
            }
        }

        private static void AddGame()
        {
            int id = _repo.NextId();

            Console.Write("Név: ");
            string? name = Console.ReadLine() ?? "";

            Console.Write("Kategória: ");
            string? category = Console.ReadLine() ?? "";

            Console.Write("Leírás: ");
            string? description = Console.ReadLine() ?? "";

            int rating = ReadInt("Értékelés (1-10): ", 1, 10);
            int difficulty = ReadInt("Nehézség (1-10): ", 1, 10);

            Console.Write("Kedvenc? (i/n): ");
            bool isFavorite = (Console.ReadLine() ?? "").Trim().ToLower() == "i";

            var game = new Game(id, name, category, description, rating, difficulty, isFavorite);
            _repo.Add(game);

            Console.WriteLine("Játék hozzáadva.");
        }

        private static void ListGames()
        {
            Console.WriteLine("=== Összes játék ===");
            foreach (var g in _repo.GetAll())
            {
                Console.WriteLine(g);
            }

            if (!_repo.Games.Any())
                Console.WriteLine("Nincs egyetlen játék sem.");
        }

        private static void SearchGames()
        {
            Console.Write("Keresett név (részlet is lehet): ");
            string? term = Console.ReadLine() ?? "";

            var results = _repo.SearchByName(term).ToList();

            Console.WriteLine($"Találatok száma: {results.Count}");
            foreach (var g in results)
            {
                Console.WriteLine(g);
            }
        }

        private static void FilterByCategory()
        {
            Console.WriteLine("Elérhető kategóriák:");
            foreach (var c in _repo.GetCategories())
            {
                Console.WriteLine($"- {c}");
            }

            Console.Write("Kategória: ");
            string? cat = Console.ReadLine() ?? "";

            var results = _repo.FilterByCategory(cat).ToList();

            Console.WriteLine($"Találatok száma: {results.Count}");
            foreach (var g in results)
            {
                Console.WriteLine(g);
            }
        }

        private static void ListSortedByRating()
        {
            Console.WriteLine("=== Játékok értékelés szerint (csökkenő) ===");
            foreach (var g in _repo.SortByRatingDescending())
            {
                Console.WriteLine(g);
            }
        }

        private static void SaveCsv()
        {
            CsvHandler.Save(CsvFilePath, _repo.GetAll());
            Console.WriteLine($"CSV mentve: {CsvFilePath}");
        }

        private static void LoadCsv()
        {
            if (!File.Exists(CsvFilePath))
            {
                Console.WriteLine("Nincs CSV fájl.");
                return;
            }

            var games = CsvHandler.Load(CsvFilePath);
            _repo.Clear();
            foreach (var g in games)
                _repo.Add(g);

            Console.WriteLine("CSV betöltve.");
        }

        private static void ExportHtml()
        {
            try
            {
                HtmlGenerator.GenerateAll(_repo);
                Console.WriteLine("HTML oldalak generálva az 'outputs' mappába.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba a HTML generálás közben:");
                Console.WriteLine(ex.Message);
            }
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"Adj meg egy számot {min} és {max} között.");
            }
        }
    }
}
