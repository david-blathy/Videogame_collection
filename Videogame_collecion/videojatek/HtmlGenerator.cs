using System;
using System.IO;
using System.Linq;
using System.Text;

namespace VideogamesCollection
{
    public static class HtmlGenerator
    {
        private const string TemplatesFolder = "templates";
        private const string OutputFolder = "outputs";

        public static void GenerateAll(GameRepository repo)
        {
            Directory.CreateDirectory(OutputFolder);
            
            // Másoljuk a CSS-t az output mappába, hogy működjön a kinézet
            if (File.Exists(Path.Combine(TemplatesFolder, "style.css")))
                File.Copy(Path.Combine(TemplatesFolder, "style.css"), Path.Combine(OutputFolder, "style.css"), true);

            GenerateIndex(repo);
            GenerateItems(repo);
            GenerateFavourites(repo);
        }

        private static void GenerateIndex(GameRepository repo)
        {
            string html = File.ReadAllText(Path.Combine(TemplatesFolder, "index.html"));
            html = html.Replace("{{DESCRIPTION}}", "Your personal library of the best titles.");
            html = html.Replace("{{TOTAL_COUNT}}", repo.Count.ToString());
            html = html.Replace("{{CATEGORY_COUNT}}", repo.GetCategories().Count().ToString());
            File.WriteAllText(Path.Combine(OutputFolder, "index.html"), html);
        }

        private static void GenerateItems(GameRepository repo)
        {
            string html = File.ReadAllText(Path.Combine(TemplatesFolder, "items.html"));
            StringBuilder sb = new StringBuilder();
            foreach (var g in repo.GetAll())
            {
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{g.Id:D2}</td>");
                sb.AppendLine($"<td class='game-name-cell'>{g.Name}</td>");
                sb.AppendLine($"<td><span class='cat-tag'>{g.Category}</span></td>");
                sb.AppendLine($"<td>{g.Description}</td>");
                sb.AppendLine($"<td class='rating-cell'>{(g.Rating >= 8 ? "rating-high" : "rating-mid")}'>{g.Rating} / 10</td>");
                sb.AppendLine("<td><div class='diff-cell'>");
                for (int i = 1; i <= 10; i++) 
                    sb.AppendLine($"<div class='diff-dot {(i <= g.Difficulty ? "filled" : "empty")}'></div>");
                sb.AppendLine("</div></td>");
                sb.AppendLine($"<td>{(g.IsFavorite ? "<span class='fav-star'>★</span>" : "")}</td>");
                sb.AppendLine("</tr>");
            }
            File.WriteAllText(Path.Combine(OutputFolder, "items.html"), html.Replace("{{TABLE_ROWS}}", sb.ToString()));
        }

        private static void GenerateFavourites(GameRepository repo)
        {
            string html = File.ReadAllText(Path.Combine(TemplatesFolder, "favourites.html"));
            StringBuilder sb = new StringBuilder();
            foreach (var g in repo.GetFavorites())
            {
                sb.AppendLine($@"
                <div class='card'>
                    <div class='card-body'>
                        <div class='fav-badge'>★</div>
                        <h3>{g.Name}</h3>
                        <div class='card-meta'>
                            <div class='card-rating'><span>{g.Rating}</span> / 10</div>
                            <span class='cat-tag'>{g.Category}</span>
                        </div>
                        <p class='card-desc'>{g.Description}</p>
                    </div>
                </div>");
            }
            File.WriteAllText(Path.Combine(OutputFolder, "favourites.html"), html.Replace("{{FAVOURITE_CARDS}}", sb.ToString()));
        }
    }
}