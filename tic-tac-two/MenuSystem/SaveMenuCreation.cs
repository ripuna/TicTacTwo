using GameController;
using Abstracts;
using DAL;
namespace MenuSystem;
public class SaveMenuCreation
{
    public Menu CreateMenu()
    {
        var gameRepository = GameController.GameController.GetGameRepository();
        var configMenuItems = gameRepository.GetGameSaveNames()
            .Select((name, index) => new MenuItem
            {
                Title = name,
                Shortcut = (index + 1).ToString(),
                MenuItemAction = (name != "No Saves! Return to menu, to create a new game") ? (index).ToString: null!
            })
            .ToList();

        if (configMenuItems.Count == 0)
        {
            throw new ApplicationException("Menu items cannot be empty");
        }

        var configMenu = new Menu(
            EMenuLevel.Secondary,
            "TIC-TAC-TWO - Choose Game Save",
            configMenuItems
        );
        return configMenu;
    }
}