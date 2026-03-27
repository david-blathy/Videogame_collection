using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace videojatek
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public string Description { get; set; } = "";
        public int Rating { get; set; }          
        public int Difficulty { get; set; }      
        public bool IsFavorite { get; set; }

        public Game() { }

        public Game(int id, string name, string category, string description,
                    int rating, int difficulty, bool isFavorite)
        {
            Id = id;
            Name = name;
            Category = category;
            Description = description;
            Rating = rating;
            Difficulty = difficulty;
            IsFavorite = isFavorite;
        }

        public override string ToString()
        {
            return $"{Id}: {Name} ({Category}) | Rating: {Rating} | Diff: {Difficulty} | Fav: {IsFavorite}";
        }
    }
}
