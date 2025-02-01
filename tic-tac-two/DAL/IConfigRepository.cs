using GameBrain;

namespace DAL;

public interface IConfigRepository
{
    void SaveConfig(GameConfiguration gameConfiguration);
    List<string> GetConfigurationNames();
    GameConfiguration GetConfigurationByName(string name);
}