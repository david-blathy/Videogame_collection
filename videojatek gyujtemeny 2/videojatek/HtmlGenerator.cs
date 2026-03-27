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

            GenerateIndex(repo);
            GenerateItems(repo);
            GenerateFavourites(repo);

            
            string srcCss = Path.Combine(TemplatesFolder, "style.css");
            string dstCss = Path.Combine(OutputFolder, "style.css");
            if (File.Exists(srcCss) && !File.Exists(dstCss))
            {
                File.Copy(srcCss, dstCss, overwrite: true);
            }
        }

        private static void GenerateIndex(GameRepository repo)
        {
            string templatePath = Path.Combine(TemplatesFolder, "index.html");
            string outputPath = Path.Combine(OutputFolder, "index.html");

            string html = File.ReadAllText(templatePath, Encoding.UTF8);

            string description = "A saját videojáték gyűjteményed – keresés, szűrés, kedvencek és statisztikák.";
            int totalCount = repo.Count;
            int categoryCount = repo.GetCategories().Count();

            html = html.Replace("{{DESCRIPTION}}", description);
            html = html.Replace("{{TOTAL_COUNT}}", totalCount.ToString());
            html = html.Replace("{{CATEGORY_COUNT}}", categoryCount.ToString());

            File.WriteAllText(outputPath, html, Encoding.UTF8);
        }

        private static void GenerateItems(GameRepository repo)
        {
            string templatePath = Path.Combine(TemplatesFolder, "items.html");
            string outputPath = Path.Combine(OutputFolder, "items.html");

            string html = File.ReadAllText(templatePath, Encoding.UTF8);

            var rowsBuilder = new StringBuilder();

            foreach (var g in repo.GetAll())
            {
                rowsBuilder.AppendLine("<tr>");
                rowsBuilder.AppendLine($"  <td>{g.Id}</td>");
                rowsBuilder.AppendLine($"  <td>{EscapeHtml(g.Name)}</td>");
                rowsBuilder.AppendLine($"  <td>{EscapeHtml(g.Category)}</td>");
                rowsBuilder.AppendLine($"  <td>{EscapeHtml(g.Description)}</td>");
                rowsBuilder.AppendLine($"  <td>{g.Rating}</td>");
                rowsBuilder.AppendLine($"  <td>{g.Difficulty}</td>");
                rowsBuilder.AppendLine($"  <td>{(g.IsFavorite ? "★" : "")}</td>");
                rowsBuilder.AppendLine("</tr>");
            }

            html = html.Replace("{{TABLE_ROWS}}", rowsBuilder.ToString());

            File.WriteAllText(outputPath, html, Encoding.UTF8);
        }

        private static void GenerateFavourites(GameRepository repo)
        {
            string templatePath = Path.Combine(TemplatesFolder, "favourites.html");
            string outputPath = Path.Combine(OutputFolder, "favourites.html");

            string html = File.ReadAllText(templatePath, Encoding.UTF8);

            var cardsBuilder = new StringBuilder();

            foreach (var g in repo.GetFavorites())
            {
                cardsBuilder.AppendLine("<div class=\"card\">");
                cardsBuilder.AppendLine($"  <h3>{EscapeHtml(g.Name)}</h3>");
                cardsBuilder.AppendLine($"  <p><strong>Category:</strong> {EscapeHtml(g.Category)}</p>");
                cardsBuilder.AppendLine($"  <p><strong>Rating:</strong> {g.Rating} / 10</p>");
                cardsBuilder.AppendLine($"  <p><strong>Difficulty:</strong> {g.Difficulty} / 10</p>");
                cardsBuilder.AppendLine($"  <p>{EscapeHtml(g.Description)}</p>");
                cardsBuilder.AppendLine("</div>");
            }

            html = html.Replace("{{FAVOURITE_CARDS}}", cardsBuilder.ToString());

            File.WriteAllText(outputPath, html, Encoding.UTF8);
        }

        private static string EscapeHtml(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;");
        }
    }
}
