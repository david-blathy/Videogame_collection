using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideojatekGyujtemeny.Models;
using System.IO;

namespace VideojatekGyujtemeny.Services
{
    public class CsvService
    {
        public void Save(string path, List<Game> games)
        {
            var lines = games.Select(g =>
                $"{g.Id};{g.Name};{g.Genre};{g.Description};{g.Rating};{g.Year};{g.IsFavorite}");

            File.WriteAllLines(path, lines);
        }

        public List<Game> Load(string path)
        {
            var games = new List<Game>();

            if (!File.Exists(path)) return games;

            var lines = File.ReadAllLines(path);

            foreach (var l in lines)
            {
                var p = l.Split(';');

                games.Add(new Game(
                    int.Parse(p[0]),
                    p[1],
                    p[2],
                    p[3],
                    int.Parse(p[4]),
                    int.Parse(p[5]),
                    bool.Parse(p[6])
                ));
            }

            return games;
        }
    }
}
