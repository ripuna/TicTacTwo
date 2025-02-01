namespace Abstracts;

public abstract class CommonMethods
{
    public static string NotImplementedMethodConsole()
    {
        Console.WriteLine("Menu Item not implemented yet :)");
        return "";
    }

    protected static string NotImplementedMethodReturnable()
    {
        return "Menu Item not implemented yet :)";
    }
}