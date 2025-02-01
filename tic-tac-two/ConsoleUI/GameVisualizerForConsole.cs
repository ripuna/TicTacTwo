using System.Text;
using GameBrain;

namespace ConsoleUI;

public class GameVisualizerForConsole : IGameVisualizer
{
    public void DrawGameState(TicTacTwoBrain gameInstance, string message)
    {
        Console.Clear();
        var currentMoveBy = DrawGamePiece(gameInstance.NextMoveBy);
        Console.WriteLine($"{currentMoveBy}'s turn to Make a move");
        Console.WriteLine(message);
        DrawBoard(gameInstance);
    }


    private void DrawBoard(TicTacTwoBrain gameInstance)
{
    Console.OutputEncoding = Encoding.UTF8;
    var lineNumber = 0;
    var gridStartX = gameInstance.GridX;
    var gridStartY = gameInstance.GridY;
    var gridWidth = gameInstance.GridDimX;
    var gridSLength = gameInstance.GridDimY;
    DrawTop(gameInstance);
    for (var y = 0; y < gameInstance.BoardDimY; y++)
    {
        lineNumber++;
        Console.WriteLine();
        Console.Write(lineNumber < 10 ? $"{lineNumber} " : $"{lineNumber}");
        Console.Write(@" │");
        for (var x = 0; x < gameInstance.BoardDimX; x++)
        {
            if ((gridStartX <= x && x < gridStartX + gridWidth) &&
                (gridStartY <= y && y < gridStartY + gridSLength))
            {
                Console.Write("(");
                Console.Write(DrawGamePiece(gameInstance.GameBoard[x][y]));
                Console.Write(x != gridStartX + gridWidth - 1 ? @")┃" : @")│");
            }
            else
            {
                Console.Write(" ");
                Console.Write(DrawGamePiece(gameInstance.GameBoard[x][y]));
                Console.Write(@" │");
            }
        }

        Console.WriteLine();
        if (y >= gameInstance.BoardDimY - 1) continue;
        {
            Console.Write(@"   ├");
            for (var x = 0; x < gameInstance.BoardDimX; x++)
            {
                if ((gridStartX <= x && x < gridStartX + gridWidth) &&
                    (gridStartY <= y && y < gridStartY + gridSLength - 1))
                {
                    Console.Write(@"━━━");
                    if (x != gridStartX + gridWidth - 1)
                    {
                        Console.Write(@"╋");
                    }
                    else
                    {
                        if (x != gameInstance.BoardDimX - 1)
                        {
                            Console.Write(@"┼");
                        }
                    }
                }
                else
                {
                    Console.Write(@"───");
                    if (x < gameInstance.BoardDimX - 1)
                    {
                        Console.Write(@"┼");
                    }
                }
            }

            Console.Write(@"┤");
        }
    }

    DrawBottom(gameInstance);
    Console.WriteLine();
}

    private void DrawTop(TicTacTwoBrain gameInstance)
    {
        var lineNumber = 0;
        Console.WriteLine("   X Coordinate ------>");
        Console.Write("     ");
        for (var x = 0; x < gameInstance.BoardDimX; x++)
        {
            lineNumber++;
            if (lineNumber < 9)
            {
                Console.Write(lineNumber + "   ");
            }
            else
            {
                Console.Write(lineNumber + "  ");
            }
        }

        Console.WriteLine();
        Console.Write(@"   ┌");
        for (var x = 0; x < gameInstance.BoardDimX; x++)
        {
            Console.Write(@"───");
            if (x < gameInstance.BoardDimX - 1)
            {
                Console.Write(@"┬");
            }
        }

        Console.Write(@"┐");
    }

    private void DrawBottom(TicTacTwoBrain gameInstance)
    {
        Console.Write(@"   └");
        for (var x = 0; x < gameInstance.BoardDimX; x++)
        {
            Console.Write(@"───");
            if (x < gameInstance.BoardDimX - 1)
            {
                Console.Write(@"┴");
            }
        }

        Console.Write(@"┘");
    }

    public string DrawGamePiece(EGamePiece piece) =>
        piece switch
        {
            EGamePiece.O => "O",
            EGamePiece.X => "X",
            _ => " "
        };
}