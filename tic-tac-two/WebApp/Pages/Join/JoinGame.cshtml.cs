using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Join;

public class JoinGame : PageModel
{
    public string Password { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Message { get; set; } = default!;
    public static IGameRepository GameRepository { get; set; } = GameController.GameController.GetGameRepository();
    public List<string> GameItems { get; set; } = GameRepository.GetGameSaveNames();


    public void OnGet()
    {
        Message = "Enter the game name and correct piece password";
    }

    public IActionResult OnPost()
    {
        Password = HttpContext.Request.Form["Password"]!;
        Name = HttpContext.Request.Form["Name"]!;
        for (var i = 0; i < GameItems.Count; i++)
        {
            var game = GameRepository.GetSaveByName(GameItems[i]);
            if ((game.OPass == Password || game.XPass == Password) && Name == game.Name)
            {
                return RedirectToPage("../TicTacTwo/TicTacTwo", new { pass = Password, index = game.Name });
            }
            else
            {
                Message = "Wrong inputs!";
            }
        }
        return Page();
    }
}