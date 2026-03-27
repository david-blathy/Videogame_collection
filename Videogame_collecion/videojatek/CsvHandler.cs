using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using videojatek;

namespace VideogamesCollection
{
    public static class CsvHandler
    {
        

        public static void Save(string filePath, IEnumerable<Game> games)
        {
            using var writer = new StreamWriter(filePath);
            writer.WriteLine("Id;Name;Category;Description;Rating;Difficulty;IsFavorite");

            foreach (var g in games)
            {
                string line = string.Join(";",
                    g.Id,
                    Escape(g.Name),
                    Escape(g.Category),
                    Escape(g.Description),
                    g.Rating,
                    g.Difficulty,
                    g.IsFavorite ? "true" : "false"
                );
                writer.WriteLine(line);
            }
        }

        public static List<Game> Load(string filePath)
        {
            var result = new List<Game>();

            if (!File.Exists(filePath))
                return result;

            using var reader = new StreamReader(filePath);
            string? header = reader.ReadLine(); 

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = SplitCsvLine(line);
                if (parts.Length < 7) continue;

                try
                {
                    int id = int.Parse(parts[0]);
                    string name = Unescape(parts[1]);
                    string category = Unescape(parts[2]);
                    string description = Unescape(parts[3]);
                    int rating = int.Parse(parts[4], CultureInfo.InvariantCulture);
                    int difficulty = int.Parse(parts[5], CultureInfo.InvariantCulture);
                    bool isFavorite = bool.Parse(parts[6]);

                    result.Add(new Game(id, name, category, description, rating, difficulty, isFavorite));
                }
                catch
                {
                    // hibás sor
                }
            }

            return result;
        }

        private static string Escape(string value)
        {
            if (value.Contains(';') || value.Contains('"'))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }
            return value;
        }

        private static string Unescape(string value)
        {
            value = value.Trim();
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
                value = value.Replace("\"\"", "\"");
            }
            return value;
        }

        private static string[] SplitCsvLine(string line)
        {
            var parts = new List<string>();
            bool inQuotes = false;
            var current = new System.Text.StringBuilder();

            foreach (char c in line)
            {
                if (c == ';' && !inQuotes)
                {
                    parts.Add(current.ToString());
                    current.Clear();
                }
                else if (c == '"')
                {
                    inQuotes = !inQuotes;
                    current.Append(c);
                }
                else
                {
                    current.Append(c);
                }
            }

            parts.Add(current.ToString());
            return parts.ToArray();
        }
    }
}
