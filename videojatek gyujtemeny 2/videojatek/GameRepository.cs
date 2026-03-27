using System;
using System.Collections.Generic;
using System.Linq;
using videojatek;

namespace VideogamesCollection
{
    public class GameRepository
    {
        private readonly List<Game> _games = new List<Game>();

        public IReadOnlyList<Game> Games => _games;

        public void Add(Game game)
        {
            _games.Add(game);
        }

        public void Clear()
        {
            _games.Clear();
        }

        public int NextId()
        {
            return _games.Count == 0 ? 1 : _games.Max(g => g.Id) + 1;
        }

        public IEnumerable<Game> GetAll()
        {
            return _games.OrderBy(g => g.Id);
        }

        public IEnumerable<Game> SearchByName(string name)
        {
            return _games
                .Where(g => g.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .OrderBy(g => g.Name);
        }

        public IEnumerable<Game> FilterByCategory(string category)
        {
            return _games
                .Where(g => g.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .OrderBy(g => g.Name);
        }

        public IEnumerable<string> GetCategories()
        {
            return _games
                .Select(g => g.Category)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .OrderBy(c => c);
        }

        public IEnumerable<Game> GetFavorites()
        {
            return _games.Where(g => g.IsFavorite).OrderByDescending(g => g.Rating);
        }

        public IEnumerable<Game> SortByRatingDescending()
        {
            return _games.OrderByDescending(g => g.Rating).ThenBy(g => g.Name);
        }

        public int Count => _games.Count;
    }
}
