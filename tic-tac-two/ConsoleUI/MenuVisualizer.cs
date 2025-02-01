using Abstracts;

namespace ConsoleUI;

public class MenuVisualizer : IMenuVisualizer
{
    public void DisplayMenu(string menuHeader, string menuDivider, List<AbstractMenuItem> menuItems)
    {
        Console.Clear();
        Console.WriteLine(menuHeader);
        Console.WriteLine(menuDivider);
        foreach (var menuItem in menuItems)
        {
            Console.WriteLine(menuItem);
        }

        Console.WriteLine();
        Console.Write("Select item >");
    }

    public string ShowRules(string gameRulesLink)
    {
        ClearScreen();
        WithoutReadKey($"Game is played in the console,\nYou can choose what to do by inserting the corresponding Selection.\n\n\n" +
                       $"Example:\n" +
                       $"1) Selection A\n" +
                       $"2) Selection B\n\n" +
                       $"Select:> (You can either insert 1 or 2 as the selection shows)\n\n");
        WithReadKey($"Tic-Tac-Two rules can be found here:\n{gameRulesLink}\n\nPress any key to return.");
        return "";
    }

    public void WithReadKey(string message)
    {
        Console.WriteLine(message);
        Console.ReadKey();
    }

    public void WithoutReadKey(string message)
    {
        Console.WriteLine(message);
    }

    public string? InputGetter(string message)
    {
        WithoutReadKey(message);
        var input = Console.ReadLine();
        return input;
    }

    public void ClearScreen()
    {
        Console.Clear();
    }
}