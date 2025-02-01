using System.Text.Json;
using Domain;
using GameBrain;

namespace DAL;

public class GameRepositoryDb : IGameRepository
{
    private readonly AppDbContext _context = FileHelper.GetDbContext();
    public void SaveGame(string stateAsJson, string saveName)
    {

        if (_context.Games.Any(g => g.GameName == saveName))
        {
            var game = _context.Games.FirstOrDefault(c => c.GameName == saveName);
            game!.GameState = stateAsJson;
        }
        else
        {
            _context.Games.Add(FileHelper.CreateDbGame(stateAsJson, saveName));
        }
        _context.SaveChanges();
    }

    public List<string> GetGameSaveNames()
    {
        var returnable = _context.Games.Select(c => c.GameName).ToList();
        return returnable.Count != 0 ? returnable : ["No Saves! Return to menu, to create a new game"];
    }

    public GameState GetSaveByName(string name)
    {
        var game = _context.Games.FirstOrDefault(c => c.GameName == name)!.GameState;

        return JsonSerializer.Deserialize<GameState>(game)!;
    }
}