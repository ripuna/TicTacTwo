using Domain;
using GameBrain;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    private readonly AppDbContext _context = FileHelper.GetDbContext();
    public void SaveConfig(GameConfiguration gameConfiguration)
    {
        var conf = FileHelper.CreateDbConfig(gameConfiguration);
        _context.Configs.Add(conf);
        _context.SaveChanges();
    }

    public List<string> GetConfigurationNames()
    {
        return _context.Configs.Select(c => c.ConfigName).ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        var config = _context.Configs.FirstOrDefault(c => c.ConfigName == name);

        return FileHelper.CreateGameConfiguration(config!);
    }
}