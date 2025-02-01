using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Config : BaseEntity
{
    [MaxLength(128)] public string ConfigName { get; set; } = default!;

    public int BoardSizeWidth { get; init; }
    public int BoardSizeHeight { get; init; }
    public int GridSizeWidth { get; init; }
    public int GridSizeHeight { get; init; }
    public int WinCondition { get; set; }
    public int MoveGridAfterNMoves { get; init; }
    public int PieceCount { get; set; }
    public bool MoveablePieces { get; init; }

    public ICollection<Game>? Games { get; set; }
}