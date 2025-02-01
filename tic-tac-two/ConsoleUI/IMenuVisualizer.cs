using Abstracts;

namespace ConsoleUI;

public interface IMenuVisualizer
{
    void DisplayMenu(string menuHeader, string menuDivider, List<AbstractMenuItem> menuItems);
    string ShowRules(string gameRulesLink);
    void WithReadKey(string message);
    void WithoutReadKey(string message);
    string? InputGetter(string message);
    void ClearScreen();
}