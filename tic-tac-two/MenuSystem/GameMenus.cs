using Abstracts;
using DataSaver;
using GameController;

namespace MenuSystem;

public class GameMenus
{
    public static GameMenus Instance { get; } = new GameMenus();

    private readonly Dictionary<string, Menu> _menus;

    private GameMenus()
    {
        _menus = new Dictionary<string, Menu>
        {
            {
                "newconf", new Menu(
                    EMenuLevel.Secondary,
                    "TIC-TAC-TWO Configuration builder",
                    [
                        new MenuItem
                        {
                            Shortcut = "1",
                            Title = "Add New Configuration",
                            MenuItemAction = ConfigSaver.AddConfig
                        }
                    ])
            },
            {
                "main", new Menu(
                    EMenuLevel.Main,
                    "TIC-TAC-TWO",
                    [
                        new MenuItem
                        {
                            Shortcut = "N",
                            Title = "New Game",
                            MenuItemAction = GameMethods.NewGame
                        },

                        new MenuItem
                        {
                            Shortcut = "L",
                            Title = "Load Game",
                            MenuItemAction =  GameMethods.LoadGame
                        },

                        new MenuItem
                        {
                            Shortcut = "C",
                            Title = "Create new Game Configuration",
                            MenuItemAction = () => _menus!["newconf"].Run()
                        },

                        new MenuItem
                        {
                            Shortcut = "G",
                            Title = "GameRules",
                            MenuItemAction = () =>
                                GameController.GameController.GetMenuVisualizer().ShowRules("https://www.geekyhobbies.com/tic-tac-two-board-game-review/")
                        }
                    ])
            }
        };
    }

    public Menu? this[string key] => _menus.GetValueOrDefault(key);
}