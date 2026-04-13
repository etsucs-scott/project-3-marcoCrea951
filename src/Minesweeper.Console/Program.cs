using System;
using Minesweeper.Core;

class Program
{
    static void Main()
    {
        var manager = new HighScoreManager("data/highscores.csv");

        while (true)
        {
            Console.WriteLine("1) 8x8\n2) 12x12\n3) 16x16");
            Console.Write("Choice: ");
            string choice = Console.ReadLine();

            int size = 8, mines = 10;

            if (choice == "2") { size = 12; mines = 25; }
            if (choice == "3") { size = 16; mines = 40; }

            Console.Write("Seed (blank = time): ");
            string seedInput = Console.ReadLine();

            int seed = string.IsNullOrWhiteSpace(seedInput)
                ? DateTime.Now.Millisecond
                : int.Parse(seedInput);

            Console.WriteLine($"Seed used: {seed}");

            var game = new Game(size, mines, seed);

            while (!game.IsGameOver)
            {
                Draw(game);

                Console.Write("> ");
                var input = Console.ReadLine().Split(' ');

                try
                {
                    if (input[0] == "q") return;

                    int r = int.Parse(input[1]);
                    int c = int.Parse(input[2]);

                    if (input[0] == "r")
                        game.Reveal(r, c);
                    else if (input[0] == "f")
                        game.Flag(r, c);
                }
                catch
                {
                    Console.WriteLine("Invalid input.");
                }
            }

            Draw(game);

            if (game.IsWin)
            {
                Console.WriteLine("You win!");

                manager.Save(new HighScore
                {
                    Size = size,
                    Seconds = game.ElapsedSeconds,
                    Moves = game.Moves,
                    Seed = seed,
                    Timestamp = DateTime.Now.ToString()
                });
            }
            else
            {
                Console.WriteLine("You hit a mine!");
            }
        }
    }

    static void Draw(Game game)
    {
        var board = game.Board;

        Console.Write("  ");
        for (int c = 0; c < board.Size; c++)
            Console.Write(c + " ");
        Console.WriteLine();

        for (int r = 0; r < board.Size; r++)
        {
            Console.Write(r + " ");

            for (int c = 0; c < board.Size; c++)
            {
                var t = board.Tiles[r, c];

                if (game.IsGameOver && t.IsMine)
                    Console.Write("b ");
                else if (!t.IsRevealed)
                    Console.Write(t.IsFlagged ? "f " : "# ");
                else if (t.AdjacentMines == 0)
                    Console.Write(". ");
                else
                    Console.Write(t.AdjacentMines + " ");
            }

            Console.WriteLine();
        }
    }
}