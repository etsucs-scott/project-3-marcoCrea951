using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Minesweeper.Core
{
    public class HighScoreManager
    {
        private string filePath;

        public HighScoreManager(string path)
        {
            filePath = path;

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "size,seconds,moves,seed,timestamp\n");
            }
        }

        public List<HighScore> Load()
        {
            var scores = new List<HighScore>();

            try
            {
                var lines = File.ReadAllLines(filePath).Skip(1);

                foreach (var line in lines)
                {
                    var parts = line.Split(',');

                    scores.Add(new HighScore
                    {
                        Size = int.Parse(parts[0]),
                        Seconds = int.Parse(parts[1]),
                        Moves = int.Parse(parts[2]),
                        Seed = int.Parse(parts[3]),
                        Timestamp = parts[4]
                    });
                }
            }
            catch
            {
                Console.WriteLine("Error reading high scores file.");
            }

            return scores;
        }

        public void Save(HighScore newScore)
        {
            var scores = Load();
            scores.Add(newScore);

            // Keep top 5 per size
            var grouped = scores
                .GroupBy(s => s.Size)
                .SelectMany(g => g
                    .OrderBy(s => s.Seconds)
                    .ThenBy(s => s.Moves)
                    .Take(5))
                .ToList();

            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("size,seconds,moves,seed,timestamp");

                    foreach (var s in grouped)
                    {
                        writer.WriteLine($"{s.Size},{s.Seconds},{s.Moves},{s.Seed},{s.Timestamp}");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error saving high scores.");
            }
        }
    }
}