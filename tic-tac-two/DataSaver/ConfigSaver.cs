using ConsoleUI;
using GameBrain;
using DAL;
using GameController;

namespace DataSaver;

public static class ConfigSaver
{
    public static string AddConfig()
    {
        var getInput = GameController.GameController.GetMenuVisualizer();
        bool okay;
        var message = "Insert Config name:";
        string? configName;
        do
        {
            configName = getInput.InputGetter(message);
            if (string.IsNullOrEmpty(configName))
            {
                message = "Name can't be empty! Try Again:";
                okay = false;
            }
            else
            {
                okay = true;
            }
        } while (!okay);

        message = "Insert Board Width:";
        var boardWidth = 0;
        do
        {
            var boardWidthString = getInput.InputGetter(message);
            if (int.TryParse(boardWidthString, out var result) && result >= 3)
            {
                okay = true;
                boardWidth = result;
            }
            else
            {
                okay = false;
                message = "Board Width minimal size is 3. Try again:";
            }
        } while (!okay);

        message = "Insert Board Height:";
        var boardHeight = 0;
        do
        {
            var boardHeightString = getInput.InputGetter(message);
            if (int.TryParse(boardHeightString, out var result) && result >= 3)
            {
                okay = true;
                boardHeight = result;
            }
            else
            {
                okay = false;
                message = "Board Height minimal size is 3. Try again:";
            }
        } while (!okay);

        message = "Insert Grid Width:";
        var gridWidth = 0;
        do
        {
            var gridWidthString = getInput.InputGetter(message);
            if (int.TryParse(gridWidthString, out var result) && result >= 3 && result <= boardWidth)
            {
                okay = true;
                gridWidth = result;
            }
            else
            {
                okay = false;
                message = $"Grid Width minimal size is 3 and maximal is {boardWidth}. Try again:";
            }
        } while (!okay);

        message = "Insert Grid Height:";
        var gridHeight = 0;
        do
        {
            var gridHeightString = getInput.InputGetter(message);
            if (int.TryParse(gridHeightString, out var result) && result >= 3 && result <= boardHeight)
            {
                okay = true;
                gridHeight = result;
            }
            else
            {
                okay = false;
                message = $"Grid Height minimal size is 3 and maximal is {boardHeight}. Try again:";
            }
        } while (!okay);

        message = "How many pieces must be in a straight line in grid to win the game? :";
        var winCondition = 0;
        do
        {
            var winConditionString = getInput.InputGetter(message);
            if (int.TryParse(winConditionString, out var result) && result >= 3)
            {
                okay = true;
                winCondition = result;
            }
            else
            {
                okay = false;
                message = "Minimal game winning line size is 3. Try Again:";
            }
        } while (!okay);

        message = "How many moves must be made before Grid can be moved? :";
        var gridMove = 0;
        do
        {
            var gridMoveString = getInput.InputGetter(message);
            if (int.TryParse(gridMoveString, out var result) && result >= 0)
            {
                okay = true;
                gridMove = result;
            }
            else
            {
                okay = false;
                message = "Value must be a positive number or 0. Try Again:";
            }
        } while (!okay);

        message = "How many pieces does each player have? :";
        var pieces = 0;
        do
        {
            var piecesString = getInput.InputGetter(message);
            if (int.TryParse(piecesString, out var result) && result >= winCondition)
            {
                okay = true;
                pieces = result;
            }
            else
            {
                okay = false;
                message = $"Minimal piece count is {winCondition}. Try again:";
            }
        } while (!okay);

        message = "Can the pieces be moved after they are placed? Insert yes or no:";
        var moveable = false;
        do
        {
            var moveableString = getInput.InputGetter(message);
            if (moveableString!.Trim().Equals("yes", StringComparison.CurrentCultureIgnoreCase))
            {
                okay = true;
                moveable = true;
            }
            else if (moveableString.Trim().Equals("no", StringComparison.CurrentCultureIgnoreCase))
            {
                okay = true;
                moveable = false;
            }
            else
            {
                okay = false;
                message = "Insert yes or no. Nothing else counts. Try again:";
            }
        } while (!okay);

        var configuration = new GameConfiguration
        {
            Name = configName,
            BoardSizeWidth = boardWidth,
            BoardSizeHeight = boardHeight,
            GridSizeWidth = gridWidth,
            GridSizeHeight = gridHeight,
            WinCondition = winCondition,
            MoveGridAfterNMoves = gridMove,
            PieceCount = pieces,
            MoveablePieces = moveable
        };

        SaveConfig(configuration);

        return "Configuration added successfully!";
    }


    private static void SaveConfig(GameConfiguration configuration)
    {
        var configRepository = GameController.GameController.GetConfigRepository();
        configRepository.SaveConfig(configuration);
    }
}