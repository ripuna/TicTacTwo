using System.Text;
using DAL;
using GameBrain;
using MenuSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.New;

public class pvp : PageModel
{
    private static readonly Random Random = new Random();
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public IGameRepository GameRepository { get; set; } = GameController.GameController.GetGameRepository();
    public static IConfigRepository ConfigRepository { get; set; } = GameController.GameController.GetConfigRepository();
    public List<string> ConfigItems { get; set; } = ConfigRepository.GetConfigurationNames();

    [BindProperty] public string GameName { get; set; } = default!;
    [BindProperty] public bool Ai { get; set; } = default!;
    [BindProperty]
    public string Config { get; set; } = default!;
    [BindProperty]
    public string XPass { get; set; } = default!;
    [BindProperty]
    public string OPass { get; set; } = default!;
    [BindProperty]
    public string Message { get; set; } = default!;

    public TicTacTwoBrain GameBrain { get; set; } = default!;

    public void OnGet()
    {
        var message1 = HttpContext.Request.Query["message"]!;
        if (message1 == "empty")
        {
            Message = "Game Name can't be empty!";
        }
    }


    public IActionResult OnPost()
    {
        XPass = HttpContext.Request.Form["xPass"]!;
        OPass = HttpContext.Request.Form["oPass"]!;
        GameName = HttpContext.Request.Form["gameName"]!;
        if (string.IsNullOrEmpty(GameName)) return RedirectToPage("../New/pvp", new { message = "empty" });
        Config = HttpContext.Request.Form["config"]!;
        GameBrain = GameMethods.CreateNewGameInstance(int.Parse(Config));
        GameBrain.SetPasswords(XPass, OPass);
        GameBrain.SetName(GameName);
        GameBrain.SetAi(Ai);
        var gameJson = GameBrain.GetGameStateJson();
        GameRepository.SaveGame(gameJson, GameName);
        return RedirectToPage("../Join/JoinGame");
    }

    public string CreatePassword()
    {
        var stringBuilder = new StringBuilder(10);
        for (var i = 0; i < 10; i++)
        {
            stringBuilder.Append(Chars[Random.Next(Chars.Length)]);
        }
        return stringBuilder.ToString();
    }
}