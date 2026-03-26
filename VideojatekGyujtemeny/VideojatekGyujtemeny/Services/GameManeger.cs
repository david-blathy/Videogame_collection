using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideojatekGyujtemeny.Models;

namespace VideojatekGyujtemeny.Services
{
    public class GameManager
    {
        public List<Game> Games = new List<Game>() ;

        public void Add(Game game) => Games.Add(game);

        public List<Game> Search(string name) =>
            Games.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToList();

        public List<Game> Filter(string genre) =>
            Games.Where(g => g.Genre.ToLower() == genre.ToLower()).ToList();

        public List<Game> Favorites() =>
            Games.Where(g => g.IsFavorite).ToList();

        public void ListAll()
        {
            foreach (var g in Games)
                Console.WriteLine($"{g.Id} - {g.Name} ({g.Genre})");
        }
    }
}
