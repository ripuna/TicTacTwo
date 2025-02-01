using GameBrain;

namespace DAL;

public class ConfigRepositoryHardCoded : IConfigRepository
{
    private readonly List<GameConfiguration> _gameConfigurations =
    [
        new GameConfiguration()
        {
            Name = "Classical 5x5",
        },

        new GameConfiguration()
        {
            Name = "Tic-Tac-Toe",
            BoardSizeWidth = 3,
            BoardSizeHeight = 3,
            MoveGridAfterNMoves = 0,
            MoveablePieces = false,
            PieceCount = 5
        }
    ];

    public void SaveConfig(GameConfiguration gameConfiguration)
    {
        return; //Hard-Coded -> new Configs can't be added
    }

    public List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .Select(config => config.Name)
            .ToList()!;
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        return _gameConfigurations.Single(c => c.Name == name);
    }

    public void AddConfiguration(GameConfiguration newConfig)
    {
        _gameConfigurations.Add(newConfig);
    }

}