namespace GameBrain;

public class TicTacTwoBrain
{
    private readonly GameState _gameState;

    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        var gameBoard = new EGamePiece[gameConfiguration.BoardSizeWidth][];
        for (var x = 0; x < gameBoard.Length; x++)
        {
            gameBoard[x] = new EGamePiece[gameConfiguration.BoardSizeHeight];
        }
        _gameState = new GameState(
            gameBoard,
            gameConfiguration,
            new GameGrid()
            {
                GridWidth = gameConfiguration.GridSizeWidth,
                GridLength = gameConfiguration.GridSizeHeight,
                GridStartX = (gameConfiguration.BoardSizeWidth - gameConfiguration.GridSizeWidth) / 2,
                GridStartY = (gameConfiguration.BoardSizeHeight - gameConfiguration.GridSizeHeight) / 2
            });
    }

    public TicTacTwoBrain(GameState gameState)
    {
        _gameState = gameState;
    }

    public void SetPasswords(string x, string o)
    {
        _gameState.OPass = o;
        _gameState.XPass = x;
    }

    public void SetName(string name)
    {
        _gameState.Name = name;
    }

    public void SetAi(bool b)
    {
        _gameState.Ai = b;
    }


    public Dictionary<EGamePiece, int> GetMoveCounts()
    {
        return new Dictionary<EGamePiece, int>(_gameState.MoveCounts);
    }

    public string GetGameStateJson()
    {
        return _gameState.ToString();
    }

    public string? GetGameConfigName()
    {
        return _gameState.GameConfiguration.Name;
    }

    public EGamePiece[][] GameBoard => GetBoard();

    public int BoardDimX => _gameState.GameBoard.Length;
    public int BoardDimY => _gameState.GameBoard[0].Length;
    public int GridDimX => _gameState.GameGrid.GridWidth;
    public int GridDimY => _gameState.GameGrid.GridLength;
    public int GridX => _gameState.GameGrid.GridStartX;
    public int GridY => _gameState.GameGrid.GridStartY;
    public EGamePiece NextMoveBy => _gameState.NextMoveMadeBy;
    public int PieceCount => _gameState.GameConfiguration.PieceCount;
    public int MoveAfterNMoves => _gameState.GameConfiguration.MoveGridAfterNMoves;
    public int WinCondition => _gameState.GameConfiguration.WinCondition;
    public string OPass => _gameState.OPass;
    public string XPass => _gameState.XPass;
    public string GameName => _gameState.Name;

    public bool Ai => _gameState.Ai;


    public bool MoveablePieces => _gameState.GameConfiguration.MoveablePieces;


    private EGamePiece[][] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameState.GameBoard.Length][];
        for (var x = 0; x < _gameState.GameBoard.Length; x++)
        {
            copyOfBoard[x] = new EGamePiece[_gameState.GameBoard[x].Length];
            for (var y = 0; y < _gameState.GameBoard[x].Length; y++)
            {

                copyOfBoard[x][y] = _gameState.GameBoard[x][y];
            }
        }

        return copyOfBoard;
    }

    private void SwitchNextPlayer()
    {
        _gameState.NextMoveMadeBy = _gameState.NextMoveMadeBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
    }

    public void EmptyPlace(int x, int y)
    {
        var removedPiece = _gameState.GameBoard[x][y];
        _gameState.MoveCounts[removedPiece]--;
        _gameState.GameBoard[x][y] = EGamePiece.Empty;
    }

    public void MakeAMove(int x, int y)
    {
        if (_gameState.GameBoard[x][y] != EGamePiece.Empty)
        {
            return;
        }

        _gameState.GameBoard[x][y] = NextMoveBy;
        _gameState.MoveCounts[NextMoveBy]++;
        SwitchNextPlayer();
    }

    public void MoveTheGrid(int x, int y)
    {
        if (GameInputValidation.ValidateGridInput($"{x+1},{y+1}", this, out var inputX, out var inputY,
                out var failureMessage))
        {
            var gameGrid = _gameState.GameGrid;
            gameGrid.GridStartX = x;
            gameGrid.GridStartY = y;
            _gameState.GameGrid = gameGrid;
            SwitchNextPlayer();
        }
        else
        {
            throw new Exception(failureMessage);
        }

    }

    public void MoveAPiece(int xStart, int yStart, int xEnd, int yEnd)
    {
        var pieceToMove = _gameState.GameBoard[xStart][yStart];
        _gameState.GameBoard[xStart][yStart] = EGamePiece.Empty;
        _gameState.GameBoard[xEnd][yEnd] = pieceToMove;
        SwitchNextPlayer();
    }

    public void ResetGame()
    {
        var gameBoard = new EGamePiece[_gameState.GameConfiguration.BoardSizeWidth][];
        for (var x = 0; x < gameBoard.Length; x++)
        {
            gameBoard[x] = new EGamePiece[_gameState.GameConfiguration.BoardSizeHeight];
        }

        _gameState.GameBoard = gameBoard;
        _gameState.NextMoveMadeBy = EGamePiece.X;
        var gameGrid = new GameGrid()
        {
            GridWidth = _gameState.GameConfiguration.GridSizeWidth,
            GridLength = _gameState.GameConfiguration.GridSizeHeight,
            GridStartX = (_gameState.GameConfiguration.BoardSizeWidth - _gameState.GameConfiguration.GridSizeWidth) / 2,
            GridStartY = (_gameState.GameConfiguration.BoardSizeHeight - _gameState.GameConfiguration.GridSizeHeight) / 2
        };
        _gameState.GameGrid = gameGrid; // Assign the new GameGrid to the property
        _gameState.MoveCounts = new Dictionary<EGamePiece, int>
        {
            { EGamePiece.O, 0 },
            { EGamePiece.X, 0 }
        };
    }
}