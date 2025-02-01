namespace GameBrain;

public record struct GameConfiguration()
{
    public string? Name { get; init; } = default!;
    public int BoardSizeWidth { get; init; } = 5;
    public int BoardSizeHeight { get; init; } = 5;
    public int GridSizeWidth { get; init; } = 3;
    public int GridSizeHeight { get; init; } = 3;
    public int WinCondition { get; set; } = 3;
    public int MoveGridAfterNMoves { get; init; } = 2;
    public int PieceCount { get; set; } = 4;
    public bool MoveablePieces { get; init; } = true;



    public override string ToString() => $"Board {BoardSizeWidth}x{BoardSizeHeight}, "
                                         + $"to win: {WinCondition}, "
                                         + $"can move pieces after {MoveGridAfterNMoves} moves";
}