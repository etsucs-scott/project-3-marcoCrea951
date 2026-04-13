namespace Minesweeper.Core;

public class Tile
{   
    public bool IsMine { get; set;}
    public bool IsRevealed { get; set;}
    public bool IsFlagged { get; set;}
    public int AdjacentMines {get; set;}
    }
