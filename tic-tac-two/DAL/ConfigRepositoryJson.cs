using System.Text.Json;
using GameBrain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    public void SaveConfig(GameConfiguration gameConfiguration)
    {
        var configAsJson = JsonSerializer.Serialize(gameConfiguration);
        var fileName = FileHelper.BasePath + gameConfiguration.Name +
                       FileHelper.ConfigExtension;
        File.WriteAllText(fileName, configAsJson);
    }

    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();
        return Directory
            .GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension)
            .Select(fullFileName =>
                Path.GetFileNameWithoutExtension(
                    Path.GetFileNameWithoutExtension(fullFileName))
                )
            .ToList();
    }

    private static void CheckAndCreateInitialConfig()
    {
        if (!System.IO.Directory.Exists(FileHelper.BasePath))
        {
            System.IO.Directory.CreateDirectory(FileHelper.BasePath);
        }
        var data = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension).ToList();
        if (data.Count != 0) return;
        var hardcodedRepo = new ConfigRepositoryHardCoded();
        var optionNames = hardcodedRepo.GetConfigurationNames();
        foreach (var optionName in optionNames)
        {
            var gameOption = hardcodedRepo.GetConfigurationByName(optionName);
            var optionJsonStr = JsonSerializer.Serialize(gameOption);
            System.IO.File.WriteAllText(FileHelper.BasePath + gameOption.Name + FileHelper.ConfigExtension, optionJsonStr);
        }
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        var configJsonStr = File.ReadAllText(FileHelper.BasePath + name + FileHelper.ConfigExtension);
        var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        return config;

    }
}