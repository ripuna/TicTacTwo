using DAL;
using GameController;

namespace MenuSystem;

using Abstracts;

public class ConfigurationMenuCreation
{
    public Menu CreateMenu()
    {
        var configRepository = GameController.GameController.GetConfigRepository();
        var configMenuItems = configRepository.GetConfigurationNames()
            .Select((name, index) => new MenuItem
            {
                Title = name,
                Shortcut = (index + 1).ToString(),
                MenuItemAction = (index).ToString
            })
            .ToList();

        if (configMenuItems.Count == 0)
        {
            throw new ApplicationException("Menu items cannot be empty");
        }

        var configMenu = new Menu(
            EMenuLevel.Secondary,
            "TIC-TAC-TWO - Choose Game Config",
            configMenuItems
        );
        return configMenu;
    }
}