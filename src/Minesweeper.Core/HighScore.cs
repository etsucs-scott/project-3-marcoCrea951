namespace Minesweeper.Core
{
    public class HighScore
    {
        public int Size { get; set; }
        public int Seconds { get; set; }
        public int Moves { get; set; }
        public int Seed { get; set; }
        public string Timestamp { get; set; }
    }
}