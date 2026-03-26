using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideojatekGyujtemeny.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int Year { get; set; }
        public bool IsFavorite { get; set; }

        public Game(int id, string name, string genre, string description, int rating, int year, bool isFavorite)
        {
            Id = id;
            Name = name;
            Genre = genre;
            Description = description;
            Rating = rating;
            Year = year;
            IsFavorite = isFavorite;
        }
    }
}
