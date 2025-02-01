using System.Text.Json;

namespace GameBrain;

public class GameState(EGamePiece[][] gameBoard, GameConfiguration gameConfiguration, GameGrid gameGrid)
{
    public EGamePiece[][] GameBoard { get; set; } = gameBoard;

    public EGamePiece NextMoveMadeBy { get; set; } = EGamePiece.X;

    public GameConfiguration GameConfiguration { get; set; } = gameConfiguration;

    public GameGrid GameGrid { get; set; } = gameGrid;

    public Dictionary<EGamePiece, int> MoveCounts { get; set; } = new()
    {
        { EGamePiece.O, 0 },
        { EGamePiece.X, 0 }
    };

    public string XPass { get; set; } = "x";

    public string OPass { get; set; } = "o";

    public string Name { get; set; } = default!;

    public bool Ai { get; set; } = default!;


    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}