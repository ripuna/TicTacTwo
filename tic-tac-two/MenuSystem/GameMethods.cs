using Abstracts;
using ConsoleUI;
using DAL;
using GameBrain;
using GameController;

namespace MenuSystem;

public abstract class GameMethods : CommonMethods
{
    private static readonly IGameVisualizer BoardVisualizer = GameController.GameController.GetGameVisualizer();
    private static readonly IMenuVisualizer MenuVisualizer = GameController.GameController.GetMenuVisualizer();
    private static readonly IGameRepository GameRepository = GameController.GameController.GetGameRepository();
    private static readonly IConfigRepository ConfigRepository = GameController.GameController.GetConfigRepository();



    public static string NewGame()
    {
        var gameOver = false;
        var configMenu = CreateConfigMenu();
        var chosenConfigShortCut = configMenu.Run();

        if (!int.TryParse(chosenConfigShortCut, out var configNo))
        {
            return chosenConfigShortCut;
        }

        var gameInstance = CreateNewGameInstance(configNo);
        var mainMessage = "";
        PlayGameLoop(gameInstance, ref gameOver, ref mainMessage);
        return "";
    }
    public static string LoadGame()
    {
        var gameOver = false;
        var saveMenu = CreateSaveMenu();
        var chosenSaveShortCut = saveMenu.Run();

        if (!int.TryParse(chosenSaveShortCut, out var saveNo))
        {
            return chosenSaveShortCut;
        }

        var gameInstance = CreateSavedGameInstance(saveNo);
        var mainMessage = "";
        PlayGameLoop(gameInstance, ref gameOver, ref mainMessage);
        return "";
    }

    private static Menu CreateSaveMenu()
    {
        var saveMenuCreation = new SaveMenuCreation();
        return saveMenuCreation.CreateMenu();
    }

    private static Menu CreateConfigMenu()
    {
        var configMenuCreation = new ConfigurationMenuCreation();
        return configMenuCreation.CreateMenu();
    }

    public static TicTacTwoBrain CreateSavedGameInstance(int saveNo)
    {
        var chosenSave = GameRepository.GetSaveByName(
            GameRepository.GetGameSaveNames()[saveNo]);
        return new TicTacTwoBrain(chosenSave);
    }
    public static TicTacTwoBrain CreateSavedGameInstance(string name)
    {
        var chosenSave = GameRepository.GetSaveByName(name);
        return new TicTacTwoBrain(chosenSave);
    }

    public static TicTacTwoBrain CreateNewGameInstance(int configNo)
    {
        var chosenConfig = ConfigRepository.GetConfigurationByName(
            ConfigRepository.GetConfigurationNames()[configNo]);
        return new TicTacTwoBrain(chosenConfig);
    }

    private static void PlayGameLoop(TicTacTwoBrain gameInstance, ref bool gameOver, ref string mainMessage)
    {
        var message = mainMessage;
        bool defaultExit;
        do
        {
            BoardVisualizer.DrawGameState(gameInstance, message);
            DisplayMenu();
            var chosenMove = MenuVisualizer.InputGetter("Select >");
            ProcessUserChoice(chosenMove, gameInstance, ref gameOver, ref message);
            if (gameOver)
            {
                defaultExit = false;
                continue;
            }

            var winner = GameOverCheck(gameInstance);
            if (winner != EGamePiece.Empty)
            {
                gameOver = true;
            }
            defaultExit = true;
        } while (!gameOver);
        MenuVisualizer.ClearScreen();
        string end;
        if (defaultExit)
        {
            var winner = gameInstance.NextMoveBy == EGamePiece.X ? "O" : "X";
            end = "GAME OVER: " + winner + " wins!";
        }
        else
        {
            end = "ENDING GAME";
        }

        BoardVisualizer.DrawGameState(gameInstance, end);
        MenuVisualizer.WithReadKey("Press any button to continue");

    }

    private static void DisplayMenu()
    {
        MenuVisualizer.WithoutReadKey("1) Make a new move\n" +
                                               "2) Move a piece\n" +
                                               "3) Move the grid\n" +
                                               "S) Save and Exit\n" +
                                               "R) Reset game\n" +
                                               "E) Return to Main Menu");
    }

    private static void ProcessUserChoice(string? chosenMove, TicTacTwoBrain gameInstance, ref bool gameOver,
        ref string message)
    {
        switch (chosenMove)
        {
            case "1":
                if (gameInstance.GetMoveCounts()[gameInstance.NextMoveBy] >= gameInstance.PieceCount)
                {
                    message = "No free pieces left to play";
                }
                else
                {
                    message = "";
                    BoardVisualizer.DrawGameState(gameInstance, message);
                    MakeAMove(gameInstance, ref message);
                }

                break;
            case "2":
                if (!gameInstance.MoveablePieces)
                {
                    message = "Can't move pieces in this game mode";
                }
                else if (gameInstance.GetMoveCounts()[gameInstance.NextMoveBy] == 0)
                {
                    message = "You can't move any pieces, if there aren't any???!!!";
                }
                else
                {
                    message = "";
                    BoardVisualizer.DrawGameState(gameInstance, message);
                    MoveAPiece(gameInstance, ref message);
                }

                break;
            case "3":
                if (gameInstance.GridDimX == gameInstance.BoardDimX && gameInstance.GridDimY == gameInstance.BoardDimY)
                {
                    message = "Where do you plan to move the grid??? Onto your desktop?";
                }
                else if (gameInstance.GetMoveCounts()[gameInstance.NextMoveBy] < gameInstance.MoveAfterNMoves)
                {
                    message = $"Grid can be moved after you have placed {gameInstance.MoveAfterNMoves} pieces";
                }
                else
                {
                    message = "";
                    BoardVisualizer.DrawGameState(gameInstance, message);
                    MoveTheGrid(gameInstance, ref message);
                }

                break;
            default:
            {
                switch (chosenMove?.ToUpper())
                {
                    case "S":
                        var name = MenuVisualizer.InputGetter("Game save name:") ?? gameInstance.GetGameConfigName();
                        GameRepository.SaveGame(gameInstance.GetGameStateJson(), name!);
                        MenuVisualizer.WithReadKey("Saved Successfully");
                        gameOver = true;
                        break;
                    case "E":
                        gameOver = true;
                        break;
                    case "R":
                        gameInstance.ResetGame();
                        break;
                    default:
                        message = "Select either 1, 2, 3, 4, S or E";
                        break;
                }

                break;
            }
        }
    }

    private static void MakeAMove(TicTacTwoBrain gameInstance, ref string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        var input = GetUserInput(gameInstance.NextMoveBy);
        InsertNewPiece(input, gameInstance, out message);
    }

    private static void MoveTheGrid(TicTacTwoBrain gameInstance, ref string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        MenuVisualizer.WithoutReadKey("Select new coordinates for upper left corner of the grid");
        var input = GetUserInput(gameInstance.NextMoveBy);
        if (GameInputValidation.ValidateGridInput(input, gameInstance, out var inputX, out var inputY,
                out var failureMessage))
        {
            gameInstance.MoveTheGrid(inputX, inputY);
            message = "";
        }
        else
        {
            message = failureMessage;
        }
    }

    private static void MoveAPiece(TicTacTwoBrain gameInstance, ref string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        MenuVisualizer.WithoutReadKey("Select coordinates of a piece to move:");
        var inputStart = GetUserInput(gameInstance.NextMoveBy);
        MenuVisualizer.WithoutReadKey("Select new coordinates");
        var inputEnd = GetUserInput(gameInstance.NextMoveBy);
        if (GameInputValidation.ValidateInputExistingPiece(inputStart, inputEnd, gameInstance, out var inputStartX,
                out var inputStartY, out var inputEndX, out var inputEndY, out var failureMessage))
        {
            gameInstance.MoveAPiece(inputStartX, inputStartY, inputEndX, inputEndY);
            message = "";
        }
        else
        {
            message = failureMessage;
        }
    }

    private static string GetUserInput(EGamePiece currentMoveBy)
    {
        return MenuVisualizer.InputGetter($"Player {currentMoveBy} insert coordinates <x,y>:")!;
    }

    private static void InsertNewPiece(string input, TicTacTwoBrain gameInstance, out string message)
    {
        if (GameInputValidation.Validate(input, gameInstance, out var inputX, out var inputY, out var failureMessage))
        {
            gameInstance.GameBoard[inputX][inputY] = EGamePiece.X;
            gameInstance.MakeAMove(inputX, inputY);
            message = "";
        }
        else
        {
            message = failureMessage;
        }
    }

    public static EGamePiece GameOverCheck(TicTacTwoBrain gameBrain)
    {
        var board = gameBrain.GameBoard;
        var winCondition = gameBrain.WinCondition;
        var gridStartX = gameBrain.GridX;
        var gridStartY = gameBrain.GridY;
        var gridEndX = gridStartX + gameBrain.GridDimX;
        var gridEndY = gridStartY + gameBrain.GridDimY;

        // Check horizontal lines
        for (var y = gridStartY; y < gridEndY; y++)
        {
            for (var x = gridStartX; x <= gridEndX - winCondition; x++)
            {
                if (board[x][y] != EGamePiece.Empty && CheckLine(board, x, y, 1, 0, winCondition))
                {
                    return board[x][y];
                }
            }
        }

        // Check vertical lines
        for (var x = gridStartX; x < gridEndX; x++)
        {
            for (var y = gridStartY; y <= gridEndY - winCondition; y++)
            {
                if (board[x][y] != EGamePiece.Empty && CheckLine(board, x, y, 0, 1, winCondition))
                {
                    return board[x][y];
                }
            }
        }

        // Check diagonal lines (top-left to bottom-right)
        for (var x = gridStartX; x <= gridEndX - winCondition; x++)
        {
            for (var y = gridStartY; y <= gridEndY - winCondition; y++)
            {
                if (board[x][y] != EGamePiece.Empty && CheckLine(board, x, y, 1, 1, winCondition))
                {
                    return board[x][y];
                }
            }
        }

        // Check diagonal lines (bottom-left to top-right)
        for (var x = gridStartX; x <= gridEndX - winCondition; x++)
        {
            for (var y = gridStartY + winCondition - 1; y < gridEndY; y++)
            {
                if (board[x][y] != EGamePiece.Empty && CheckLine(board, x, y, 1, -1, winCondition))
                {
                    return board[x][y];
                }
            }
        }

        return EGamePiece.Empty;
    }

    private static bool CheckLine(EGamePiece[][] board, int startX, int startY, int stepX, int stepY, int winCondition)
    {
        var firstPiece = board[startX][startY];
        if (firstPiece == EGamePiece.Empty) return false;

        for (var i = 1; i < winCondition; i++)
        {
            if (board[startX + i * stepX][startY + i * stepY] != firstPiece)
            {
                return false;
            }
        }

        return true;
    }
}