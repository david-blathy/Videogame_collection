using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideojatekGyujtemeny.Models;
using VideojatekGyujtemeny.Services;

namespace VideojatekGyujtemeny
{
    public class Program
    {
        static void Main(string[] args)
        {
            var manager = new GameManager();
            var csv = new CsvService();
            var html = new HtmlService();

            int choice;

            do
            {
                Console.WriteLine("\n1 Hozzáadás");
                Console.WriteLine("2 Lista");
                Console.WriteLine("3 Keresés");
                Console.WriteLine("4 Szűrés");
                Console.WriteLine("5 Mentés");
                Console.WriteLine("6 Betöltés");
                Console.WriteLine("7 HTML");
                Console.WriteLine("0 Kilépés");

                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Név: ");
                        var name = Console.ReadLine();

                        manager.Add(new Game(1, name, "Action", "Leírás", 9, 2020, true));
                        break;

                    case 2:
                        manager.ListAll();
                        break;

                    case 5:
                        csv.Save("games.csv", manager.Games);
                        break;

                    case 6:
                        manager.Games = csv.Load("games.csv");
                        break;

                    case 7:
                        html.GenerateAll(manager.Games);
                        break;
                }

            } while (choice != 0);
        }
    }
}
