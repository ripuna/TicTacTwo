using System.Text.Json;
using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    public void SaveGame(string stateAsJson, string saveName)
    {
        var fileName = FileHelper.BasePath + saveName +
                       FileHelper.GameExtension;
        File.WriteAllText(fileName, stateAsJson);
    }

    public List<string> GetGameSaveNames()
    {
        CheckSaves(out var error);
        if (!error)
        {
            return Directory
                .GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension)
                .Select(fullFileName =>
                    Path.GetFileNameWithoutExtension(
                        Path.GetFileNameWithoutExtension(fullFileName))
                )
                .ToList();
        }
        else
        {
            return ["No Saves! Return to menu, to create a new game"];
        }
    }


    private static void CheckSaves(out bool error)
    {
        if (!Directory.Exists(FileHelper.BasePath))
        {
            Directory.CreateDirectory(FileHelper.BasePath);
        }
        var data = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension).ToList();
        if (data.Count != 0)
        {
            error = false;
            return;
        }
        error = true;
    }

    public GameState GetSaveByName(string name)
    {
        var gameJsonStr = File.ReadAllText(FileHelper.BasePath + name + FileHelper.GameExtension);
        var gameSave = JsonSerializer.Deserialize<GameState>(gameJsonStr);
        return gameSave!;
    }
}