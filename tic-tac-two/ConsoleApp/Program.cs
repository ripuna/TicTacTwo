using MenuSystem;


while (true)
{
    var result = GameMenus.Instance["main"]?.Run();
    if (result == "exit")
    {
        break;
    }
}
