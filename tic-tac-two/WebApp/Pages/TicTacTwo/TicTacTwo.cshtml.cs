using ConsoleUI;
using DAL;
using GameBrain;
using MenuSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.TicTacTwo;

public class TicTacTwo : PageModel
{
    //TODO: Reset Button
    //TODO: Game Over

    public string Password { get; set; } = default!;
    public EGamePiece MyPiece { get; set; } = default!;
    public string GameIndex { get; set; } = default!;
    public TicTacTwoBrain GameBrain { get; set; } = default!;
    public IGameVisualizer GameVisualizer { get; set; } = GameController.GameController.GetGameVisualizer();
    public IGameRepository GameRepository { get; set; } = GameController.GameController.GetGameRepository();
    public string Message { get; set; } = default!;

    public int XCoordinate { get; set; } = default!;
    public int YCoordinate { get; set; } = default!;
    public string Command { get; set; } = default!;
    public EGamePiece PressedPiece { get; set; } = default!;
    public EGamePiece Winner { get; set; } = default!;

    public string LastCommand { get; set; } = default!;

    public bool GameOver { get; set; } = default!;

    public bool Ai { get; set; } = default!;


    public void End()
    {
        GameRepository.SaveGame(GameBrain.GetGameStateJson(), GameBrain.GameName);
        Winner = IsGameOver(GameBrain);
    }

    public void OnGet()
    {
        Start();
        End();
    }

    public EGamePiece IsGameOver(TicTacTwoBrain brain)
    {
        var winner = GameMethods.GameOverCheck(GameBrain);
        GameOver = winner != EGamePiece.Empty;

        return winner;
    }

    public void Start()
    {
        GameIndex = HttpContext.Request.Query["index"]!;
        GameBrain = GameMethods.CreateSavedGameInstance(GameIndex);
        Password = HttpContext.Request.Query["pass"]!;
        Ai = GameBrain.Ai;
        var allSaves = GameRepository.GetGameSaveNames();
        for (var i = 0; i < allSaves.Count; i++)
        {
            if (GameMethods.CreateSavedGameInstance(i).GetGameStateJson() != GameBrain.GetGameStateJson()) continue;
            GameRepository.SaveGame(GameBrain.GetGameStateJson(), allSaves[i]);
            break;
        }

        if (GameBrain.OPass == Password)
        {
            MyPiece = EGamePiece.O;
        }
        else if (GameBrain.XPass == Password)
        {
            MyPiece = EGamePiece.X;
        }
        else
        {
            Message = "SOMETHING WENT WRONG!";
        }

        Winner = IsGameOver(GameBrain);
        }


    public void AiMove()
    {
        var random = new Random();
        var maxX = GameBrain.BoardDimX;
        var maxY = GameBrain.BoardDimY;
        var randomMove = random.Next(0, 3);
        int randomX;
        int randomY;
        if (randomMove == 2)
        {
            if (GameBrain.GetMoveCounts()[GameBrain.NextMoveBy] >= GameBrain.MoveAfterNMoves && (GameBrain.GridDimX != GameBrain.BoardDimX || GameBrain.GridDimY != GameBrain.BoardDimY))
            {
                var loop = true;
                while (loop)
                {
                    randomX = random.Next(0, maxX);
                    randomY = random.Next(0, maxY);
                    try
                    {
                        GameBrain.MoveTheGrid(randomX,randomY);
                        Message = $"\"Ai\" moved the grid to {randomX}, {randomY}";
                        loop = false;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            else
            {
                AiMove();
            }
        }
        else
        {
            if (GameBrain.GetMoveCounts()[GameBrain.NextMoveBy] >= GameBrain.PieceCount)
            {
                do
                {
                    randomX = random.Next(0, maxX);
                    randomY = random.Next(0, maxY);
                } while (GameBrain.GameBoard[randomX][randomY] != GameBrain.NextMoveBy);

                GameBrain.EmptyPlace(randomX, randomY);
            }

            do
            {
                randomX = random.Next(0, maxX);
                randomY = random.Next(0, maxY);
            } while (GameBrain.GameBoard[randomX][randomY] != EGamePiece.Empty);

            GameBrain.MakeAMove(randomX, randomY);
            Message = $"\"Ai\" made a move in {randomX}, {randomY}";
        }
    }

    public void OnPost()
    {
        Start();
        Command = HttpContext.Request.Form["command"]!;

        if (GameOver)
        {
            if (Command != "reset") return;
            GameBrain.ResetGame();
            End();
            return;
        }

        if (Command == "ai")
        {
            AiMove();
        }
        else if (GameBrain.NextMoveBy == MyPiece)
        {
            if (Command == "move")
            {
                LastCommand = HttpContext.Request.Form["lastcommand"]!;
                XCoordinate = int.Parse(HttpContext.Request.Form["x"]!);
                YCoordinate = int.Parse(HttpContext.Request.Form["y"]!);
                PressedPiece = (HttpContext.Request.Form["piece"] == "O")
                    ? EGamePiece.O
                    : ((HttpContext.Request.Form["piece"] == "X") ? EGamePiece.X : EGamePiece.Empty);
                if (LastCommand != "movegrid")
                {
                    if (PressedPiece == EGamePiece.Empty)
                    {
                        if (GameBrain.GetMoveCounts()[GameBrain.NextMoveBy] >= GameBrain.PieceCount)
                        {
                            Message = "No free pieces left to play";
                        }
                        else
                        {
                            GameBrain.MakeAMove(XCoordinate, YCoordinate);
                        }
                    }
                    else
                    {
                        if (GameBrain.MoveablePieces)
                        {
                            if (MyPiece == PressedPiece)
                            {
                                GameBrain.EmptyPlace(XCoordinate, YCoordinate);
                            }
                            else
                            {
                                Message = "Not your Piece to move!";
                            }
                        }
                        else
                        {
                            Message = "Pieces aren't moveable";
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (GameBrain.GridDimX != GameBrain.BoardDimX || GameBrain.GridDimY != GameBrain.BoardDimY)
                        {
                            if (GameBrain.GetMoveCounts()[GameBrain.NextMoveBy] < GameBrain.MoveAfterNMoves)
                            {
                                Message =
                                    $"Grid can be moved after you have placed {GameBrain.MoveAfterNMoves} pieces";
                            }
                            else
                            {
                                GameBrain.MoveTheGrid(XCoordinate, YCoordinate);
                            }
                        }
                        else
                        {
                            Message = "Grid is not moveable! (Where would even you move it??)";
                        }
                    }
                    catch (Exception)
                    {
                        Message = "Grid can't be placed there";
                    }
                }
            }
            else if (Command == "reset")
            {
                GameBrain.ResetGame();
            }
        }

        End();
    }
}