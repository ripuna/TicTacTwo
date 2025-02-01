using ConsoleUI;
using DAL;
using GameBrain;
using MenuSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApp.Pages.Game;

public class TicTacTwo : PageModel
{

    public TicTacTwoBrain GameBrain { get; set; } = default!;
    public string GameStateJson { get; set; } = default!;
    public IGameVisualizer GameVisualizer { get; set; } = GameController.GameController.GetGameVisualizer();
    public IGameRepository GameRepository { get; set; } = GameController.GameController.GetGameRepository();

    public string Message { get; set; } = default!;

    public EGamePiece CurrentMover { get; set; } = default!;

    public bool GameOver { get; set; } = false;

    public string Command { get; set; } = default!;
    public string LastCommand { get; set; } = default!;
    public string GameName { get; set; } = default!;


    public EGamePiece EPiece { get; set; } = EGamePiece.Empty;

    public void OnGet()
    {
        var selection = int.Parse(Request.Query["nr"]!);
        if (HttpContext.Request.Query["mode"] == "conf")
        {
            GameBrain = GameMethods.CreateNewGameInstance(selection);
        }

        if (HttpContext.Request.Query["mode"] == "load")
        {
            GameBrain = GameMethods.CreateSavedGameInstance(selection);
        }

        CurrentMover = GameBrain.NextMoveBy;

        if (GameMethods.GameOverCheck(GameBrain) != EGamePiece.Empty)
        {
            GameOver = true;
            Message = "GameOver! " + GameMethods.GameOverCheck(GameBrain) + " wins";
        }
    }

    public void OnPost()
    {
        Command = HttpContext.Request.Form["command"]!;
        LastCommand = HttpContext.Request.Form["lastcommand"]!;
        GameStateJson = HttpContext.Request.Form["gamestate"]!;
        var gameState = JsonSerializer.Deserialize<GameState>(GameStateJson);
        GameBrain = new TicTacTwoBrain(gameState!);
        GameOver = bool.Parse(HttpContext.Request.Form["gameover"]!);
        CurrentMover = (!GameOver) ? GameBrain.NextMoveBy : GameMethods.GameOverCheck(GameBrain);
        switch (Command)
        {
            case "move":

                if (!GameOver)
                {
                    var x = int.Parse(HttpContext.Request.Form["x"]!);
                    var y = int.Parse(HttpContext.Request.Form["y"]!);
                    var piece = HttpContext.Request.Form["piece"]!;
                    EPiece = (piece == "X") ? EGamePiece.X : ((piece == "O") ? EGamePiece.O : EGamePiece.Empty);
                    if (LastCommand != "movegrid")
                    {
                        if (EPiece == EGamePiece.Empty)
                        {
                            if (GameBrain.GetMoveCounts()[GameBrain.NextMoveBy] >= GameBrain.PieceCount)
                            {
                                Message = "No free pieces left to play";
                            }
                            else
                            {
                                GameBrain.MakeAMove(x, y);
                            }
                        }
                        else
                        {
                            if (GameBrain.MoveablePieces)
                            {
                                if (CurrentMover == EPiece)
                                {
                                    GameBrain.EmptyPlace(x, y);
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
                            if (GameBrain.GridDimX != GameBrain.BoardDimX && GameBrain.GridDimY != GameBrain.BoardDimY)
                            {
                                if (GameBrain.GetMoveCounts()[GameBrain.NextMoveBy] < GameBrain.MoveAfterNMoves)
                                {
                                    Message =
                                        $"Grid can be moved after you have placed {GameBrain.MoveAfterNMoves} pieces";
                                }
                                else
                                {
                                    GameBrain.MoveTheGrid(x, y);
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

                break;
            case "aftersave":
                GameName = HttpContext.Request.Form["GameName"]!;
                GameRepository.SaveGame(GameStateJson, GameName);
                Message = "TicTacTwo saved successfully";
                break;
            case "reset":
                GameBrain.ResetGame();
                GameOver = false;
                break;
        }

        if (Message != "")
        {
            if (GameMethods.GameOverCheck(GameBrain) != EGamePiece.Empty)
            {
                GameOver = true;
                Message = "GameOver! " + GameMethods.GameOverCheck(GameBrain) + " wins";
            }
        }

        GameStateJson = JsonSerializer.Serialize(GameBrain.GameBoard);
    }
}