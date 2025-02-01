using ConsoleUI;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace GameController;

public static class GameController
{
    private static IGameVisualizer _boardVisualizer = new GameVisualizerForConsole();
    private static IMenuVisualizer _menuVisualizer = new MenuVisualizer();
    private static IGameRepository _gameRepository = new GameRepositoryJson();
    private static IConfigRepository _configRepository = new ConfigRepositoryJson();

    public static IGameVisualizer GetGameVisualizer()
    {
        return _boardVisualizer;
    }

    public static IMenuVisualizer GetMenuVisualizer()
    {
        return _menuVisualizer;
    }

    public static IGameRepository GetGameRepository()
    {
        return _gameRepository;
    }

    public static IConfigRepository GetConfigRepository()
    {
        return _configRepository;
    }
}