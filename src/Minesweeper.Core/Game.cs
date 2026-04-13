using System;

namespace Minesweeper.Core
{
    public class Game
    {
        public Board Board { get; }
        public bool IsGameOver { get; private set; }
        public bool IsWin { get; private set; }

        public int Moves { get; private set; }

        private DateTime startTime;

        public int ElapsedSeconds => (int)(DateTime.Now - startTime).TotalSeconds;

        public Game(int size, int mines, int seed)
        {
            Board = new Board(size, mines, seed);
            startTime = DateTime.Now;
        }

        public void Reveal(int r, int c)
        {
            if (IsGameOver) return;

            var tile = Board.Tiles[r, c];

            if (tile.IsFlagged) return;

            Moves++;
            Board.Reveal(r, c);

            if (tile.IsMine)
            {
                IsGameOver = true;
                IsWin = false;
                return;
            }

            if (Board.AllSafeTilesRevealed())
            {
                IsGameOver = true;
                IsWin = true;
            }
        }

        public void Flag(int r, int c)
        {
            if (IsGameOver) return;

            Board.ToggleFlag(r, c);
        }
    }
}