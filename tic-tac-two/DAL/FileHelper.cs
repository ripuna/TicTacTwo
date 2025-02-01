using System.Text.Json;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public static class FileHelper
{
    public static readonly string BasePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                                             + System.IO.Path.DirectorySeparatorChar + "tic-tac-two" + System.IO.Path.DirectorySeparatorChar;
    public const string ConfigExtension = ".config.json";
    public const string GameExtension = ".game.json";

    public static AppDbContext GetDbContext()
    {
        var connectionString = $"Data Source={FileHelper.BasePath}/game.db";
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;

        var context = new AppDbContext(contextOptions);
        return context;
    }
    public static Config CreateDbConfig(GameConfiguration gameConfiguration)
    {
        return new Config()
        {
            ConfigName = gameConfiguration.Name!,
            BoardSizeWidth = gameConfiguration.BoardSizeWidth,
            BoardSizeHeight = gameConfiguration.BoardSizeHeight,
            GridSizeHeight = gameConfiguration.GridSizeHeight,
            GridSizeWidth = gameConfiguration.GridSizeWidth,
            WinCondition = gameConfiguration.WinCondition,
            MoveablePieces = gameConfiguration.MoveablePieces,
            MoveGridAfterNMoves = gameConfiguration.MoveGridAfterNMoves,
            PieceCount = gameConfiguration.PieceCount
        };
    }

    public static GameConfiguration CreateGameConfiguration(Config config)
    {
        return new GameConfiguration
        {
            Name = config!.ConfigName,
            BoardSizeWidth = config.BoardSizeWidth,
            BoardSizeHeight = config.BoardSizeHeight,
            GridSizeHeight = config.GridSizeHeight,
            GridSizeWidth = config.GridSizeWidth,
            WinCondition = config.WinCondition,
            MoveablePieces = config.MoveablePieces,
            MoveGridAfterNMoves = config.MoveGridAfterNMoves,
            PieceCount = config.PieceCount
        };
    }

    public static Game CreateDbGame(string state, string name)
    {
        var gameState = JsonSerializer.Deserialize<GameState>(state);
        gameState!.Name = name;
        return new Game()
        {
            ConfigId = GetDbContext().Configs.FirstOrDefault(c => c.ConfigName == gameState.GameConfiguration.Name)!.Id,
            GameState = JsonSerializer.Serialize(gameState),
            GameName = name
        };
    }
}