using Abstracts;
using ConsoleUI;
using GameController;

namespace MenuSystem;

public class Menu
{
    private static IMenuVisualizer _visualizer = GameController.GameController.GetMenuVisualizer();
    private string MenuHeader { get; set; }
    private const string MenuDivider = "====================";
    private List<MenuItem> MenuItems { get; set; }

    private readonly MenuItem _menuItemExit = new MenuItem()
    {
        Shortcut = "E",
        Title = "Exit",
    };

    private readonly MenuItem _menuItemReturn = new MenuItem()
    {
        Shortcut = "R",
        Title = "Return",
    };

    private readonly MenuItem _menuItemReturnMain = new MenuItem()
    {
        Shortcut = "M",
        Title = "Return to MainMenu",
    };

    private EMenuLevel MenuLevel { get; set; }

    private int _menuMadness;

    public Menu(EMenuLevel menuLevel, string menuHeader, List<MenuItem> menuItems)
    {
        if (string.IsNullOrWhiteSpace(menuHeader))
        {
            throw new ApplicationException("Menu header cannot be empty");
        }

        MenuHeader = menuHeader;
        if (menuItems == null || menuItems.Count == 0)
        {
            throw new ApplicationException("Menu items cannot be empty");
        }

        MenuItems = menuItems;
        MenuLevel = menuLevel;

        if (MenuLevel != EMenuLevel.Main)
        {
            MenuItems.Add(_menuItemReturn);
        }

        if (MenuLevel == EMenuLevel.Deep)
        {
            MenuItems.Add(_menuItemReturnMain);
        }

        MenuItems.Add(_menuItemExit);

        ValidateMenuItems(); // Validate menu items for shortcut conflict
    }

    private void ValidateMenuItems()
    {
        var shortcuts = new HashSet<string>();
        foreach (var item in MenuItems.Where(item => !shortcuts.Add(item.Shortcut.ToUpper())))
        {
            throw new ApplicationException(
                $"Every MenuItem inside Menu must be different. Found multiple Shortcuts: {item.Shortcut}");
        }
    }

    public string Run()
    {
        _menuMadness = 0; // Reset the counter
        do
        {
            var menuItem = DisplayMenuGetUserChoice();
            var menuReturnValue = "";
            if (menuItem.MenuItemAction != null)
            {
                menuReturnValue = menuItem.MenuItemAction();
            }

            if (menuItem.Shortcut == _menuItemReturn.Shortcut)
            {
                return menuItem.Shortcut;
            }

            if (menuItem.Shortcut == _menuItemExit.Shortcut || menuReturnValue == _menuItemExit.Shortcut)
            {
                return "exit";
            }

            if ((menuItem.Shortcut == _menuItemReturnMain.Shortcut ||
                 menuReturnValue == _menuItemReturnMain.Shortcut) && MenuLevel != EMenuLevel.Main)
            {
                return menuItem.Shortcut;
            }

            if (!string.IsNullOrWhiteSpace(menuReturnValue))
            {
                return menuReturnValue;
            }
        } while (true);
    }

    private MenuItem DisplayMenuGetUserChoice()
    {
        do
        {
            DisplayMenu();
            var userInput = _visualizer.InputGetter("");

            string message;
            if (string.IsNullOrWhiteSpace(userInput))
            {
                message = _menuMadness switch
                {
                    <= 3 => "You actually have to choose something.",
                    <= 10 => "Just press a button on your keyboard :/",
                    _ => "YOU ARE STUPID!"
                };
            }
            else
            {
                userInput = userInput.ToUpper();
                foreach (var menuItem in MenuItems.Where(menuItem => menuItem.Shortcut.ToUpper() == userInput))
                {
                    return menuItem;
                }

                message = _menuMadness switch
                {
                    <= 3 => "Please choose an existing option :/",
                    <= 10 => "HEY! CHOOSE AN EXISTING OPTION >:I",
                    _ => "YOU ARE STUPID!"
                };
            }

            _visualizer.WithReadKey(message);

            _menuMadness++;
        } while (true);
    }

    private void DisplayMenu()
    {
        var abstractMenuItems = MenuItems.Cast<AbstractMenuItem>().ToList();
        _visualizer.DisplayMenu(MenuHeader, MenuDivider, abstractMenuItems);
    }
}