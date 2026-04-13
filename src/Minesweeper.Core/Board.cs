using System;

namespace Minesweeper.Core
{
    public class Board
    {
        public int Size { get; }
        public int MineCount { get; }
        public Tile[,] Tiles { get; }

        private Random random;

        public Board(int size, int mineCount, int seed)
        {
            Size = size;
            MineCount = mineCount;
            Tiles = new Tile[size, size];
            random = new Random(seed);

            Initialize();
        }

        private void Initialize()
        {
            // Create tiles
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    Tiles[r, c] = new Tile();
                }
            }

            PlaceMines();
            CalculateAdjacency();
        }

        private void PlaceMines()
        {
            int placed = 0;

            while (placed < MineCount)
            {
                int r = random.Next(Size);
                int c = random.Next(Size);

                if (!Tiles[r, c].IsMine)
                {
                    Tiles[r, c].IsMine = true;
                    placed++;
                }
            }
        }

        private void CalculateAdjacency()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    if (Tiles[r, c].IsMine) continue;

                    int count = 0;

                    for (int dr = -1; dr <= 1; dr++)
                    {
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            int nr = r + dr;
                            int nc = c + dc;

                            if (InBounds(nr, nc) && Tiles[nr, nc].IsMine)
                                count++;
                        }
                    }

                    Tiles[r, c].AdjacentMines = count;
                }
            }
        }

        public bool InBounds(int r, int c)
        {
            return r >= 0 && r < Size && c >= 0 && c < Size;
        }

        public void Reveal(int r, int c)
        {
            if (!InBounds(r, c)) return;

            Tile tile = Tiles[r, c];

            if (tile.IsRevealed || tile.IsFlagged) return;

            tile.IsRevealed = true;

            // Cascade
            if (tile.AdjacentMines == 0 && !tile.IsMine)
            {
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        Reveal(r + dr, c + dc);
                    }
                }
            }
        }

        public void ToggleFlag(int r, int c)
        {
            if (!InBounds(r, c)) return;

            Tile tile = Tiles[r, c];

            if (!tile.IsRevealed)
                tile.IsFlagged = !tile.IsFlagged;
        }

        public bool AllSafeTilesRevealed()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    var t = Tiles[r, c];
                    if (!t.IsMine && !t.IsRevealed)
                        return false;
                }
            }
            return true;
        }
    }
}