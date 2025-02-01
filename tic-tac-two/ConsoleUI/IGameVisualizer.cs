using GameBrain;

namespace ConsoleUI;

public interface IGameVisualizer
{
    public void DrawGameState(TicTacTwoBrain gameInstance, string message)
    {
    }

    string DrawGamePiece(EGamePiece piece);
}