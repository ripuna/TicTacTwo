using Abstracts;
using Exception = System.Exception;

namespace GameBrain;

public abstract class GameInputValidation
{
    public static bool Validate(string input, TicTacTwoBrain gameInstance, out int inputX, out int inputY,
        out string failureMessage)
    {
        inputX = 0;
        inputY = 0;

        if (string.IsNullOrWhiteSpace(input))
        {
            failureMessage = "Input cannot be empty. Please provide coordinates in the format <x,y>.";
            return false;
        }

        var inputSplit = input.Split(",");
        if (inputSplit.Length != 2)
        {
            failureMessage = "Invalid format. Please provide coordinates in the format <x,y>.";
            return false;
        }

        if (!int.TryParse(inputSplit[0], out inputX) || !int.TryParse(inputSplit[1], out inputY))
        {
            failureMessage = "Both coordinates must be valid integers.";
            return false;
        }

        // Adjust for zero-based indexing
        inputX -= 1;
        inputY -= 1;

        var xMax = gameInstance.BoardDimX;
        var yMax = gameInstance.BoardDimY;

        if ((!(0 <= inputX && inputX < xMax)) && (!(0 <= inputY && inputY < yMax)))
        {
            failureMessage = $"Both Coordinates out of range!\n" +
                             $"Coordinate X must stay in range (1 -> {xMax})\n" +
                             $"Coordinate Y must stay in range (1 -> {yMax})";
            return false;
        }

        if (!(0 <= inputX && inputX < xMax))
        {
            failureMessage = $"X coordinate must stay in range (1 -> {xMax})";
            return false;
        }

        if (!(0 <= inputY && inputY < yMax))
        {
            failureMessage = $"Y coordinate must stay in range (1 -> {yMax})";
            return false;
        }

        if (gameInstance.GameBoard[inputX][inputY] != EGamePiece.Empty)
        {
            failureMessage = "Selected coordinate contains game piece!";
            return false;
        }

        failureMessage = "no failure";
        return true;
    }

    public static bool ValidateGridInput(string input, TicTacTwoBrain gameInstance, out int inputX, out int inputY,
        out string failureMessage)
    {
        inputX = 0;
        inputY = 0;
        if (string.IsNullOrWhiteSpace(input))
        {
            failureMessage = "Input cannot be empty. Please provide coordinates in the format <x,y>.";
            return false;
        }

        var inputSplit = input.Split(",");
        if (inputSplit.Length != 2)
        {
            failureMessage = "Invalid format. Please provide coordinates in the format <x,y>.";
            return false;
        }

        if (!int.TryParse(inputSplit[0], out inputX) || !int.TryParse(inputSplit[1], out inputY))
        {
            failureMessage = "Both coordinates must be valid integers.";
            return false;
        }

        var boardWidth = gameInstance.BoardDimX;
        var boardLength = gameInstance.BoardDimY;
        var gridWidth = gameInstance.GridDimX;
        var gridLength = gameInstance.GridDimY;
        var gridStartX = gameInstance.GridX;
        var gridStartY = gameInstance.GridY;

        inputX -= 1;
        inputY -= 1;
        if ((!(0 <= inputX && inputX < boardWidth - gridWidth + 1)) &&
            (!(0 <= inputY && inputY < boardLength - gridLength + 1)))
        {
            failureMessage = $"Both Coordinates out of range!\n" +
                             $"Coordinate X must stay in range (1 -> {boardWidth - gridWidth + 1})\n" +
                             $"Coordinate Y must stay in range (1 -> {boardLength - gridLength + 1})";
            return false;
        }

        if (!(0 <= inputX && inputX < boardWidth - gridWidth + 1))
        {
            failureMessage = $"X coordinate must stay in range (1 -> {boardWidth - gridWidth + 1})";
            return false;
        }

        if (!(0 <= inputY && inputY < boardLength - gridLength + 1))
        {
            failureMessage = $"Y coordinate must stay in range (1 -> {boardLength - gridLength + 1})";
            return false;
        }

        if (inputX == gridStartX && inputY == gridStartY)
        {
            failureMessage = $"Grid corner already on selected coordinates <{gridStartX},{gridStartY}>";
        }

        failureMessage = "no failure";
        return true;
    }

    public static bool ValidateInputExistingPiece(string inputStart, string inputEnd, TicTacTwoBrain gameInstance,
        out int inputStartX,
        out int inputStartY, out int inputEndX, out int inputEndY, out string failureMessage)
    {
        inputStartX = 0;
        inputStartY = 0;
        inputEndX = 0;
        inputEndY = 0;

        if (string.IsNullOrWhiteSpace(inputStart))
        {
            failureMessage = "Input cannot be empty. Please provide coordinates in the format <x,y>.";
            return false;
        }

        var inputSplit = inputStart.Split(",");
        if (inputSplit.Length != 2)
        {
            failureMessage = "Invalid format. Please provide coordinates in the format <x,y>.";
            return false;
        }

        if (!int.TryParse(inputSplit[0], out inputStartX) || !int.TryParse(inputSplit[1], out inputStartY))
        {
            failureMessage = "Both coordinates must be valid integers.";
            return false;
        }

        inputStartX -= 1;
        inputStartY -= 1;
        var pieceOnCoordinates = gameInstance.GameBoard[inputStartX][inputStartY];
        if (pieceOnCoordinates == EGamePiece.Empty)
        {
            failureMessage = $"No piece on <{inputStartX + 1},{inputStartY + 1}>";
            return false;
        }

        if (pieceOnCoordinates != gameInstance.NextMoveBy)
        {
            failureMessage = $"Not your piece on <{inputStartX + 1},{inputStartY + 1}>";
            return false;
        }

        if (!Validate(inputEnd, gameInstance, out inputEndX, out inputEndY, out var failedMessage))
        {
            failureMessage = failedMessage;
            return false;
        }

        failureMessage = "no failure";
        return true;
    }
}