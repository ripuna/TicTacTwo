using GameBrain;

namespace DAL;

public interface IGameRepository
{
    public void SaveGame(string stateAsJson, string saveName);
    public List<string> GetGameSaveNames();
    public GameState GetSaveByName(string name);
}