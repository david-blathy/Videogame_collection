using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideojatekGyujtemeny.Models;
using System.IO;

namespace VideojatekGyujtemeny.Services
{
    public class HtmlService
    {
        string template = File.ReadAllText("index.html");

        public void GenerateAll(List<Game> games)
        {
            Generate("index.html", "Főoldal", $"<p>Elemek száma: {games.Count}</p>");

            Generate("items.html", "Összes játék",
                string.Join("", games.Select(g =>
                    $"<div class='card'><h2>{g.Name}</h2><p>{g.Description}</p></div>")));

            Generate("favorites.html", "Kedvencek",
                string.Join("", games.Where(g => g.IsFavorite).Select(g =>
                    $"<div class='card'><h2>{g.Name}</h2></div>")));
        }

        void Generate(string file, string title, string content)
        {
            var html = template
                .Replace("{{TITLE}}", title)
                .Replace("{{CONTENT}}", content);

            File.WriteAllText($"output/{file}", html);
        }
    }
}
