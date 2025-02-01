using DAL;
using GameController;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game.Menus;

public class LoadGame : PageModel
{
    public static IGameRepository GameRepository { get; set; } = GameController.GameController.GetGameRepository();
    public List<string> GameItems { get; set; } = GameRepository.GetGameSaveNames();

    public void OnGet()
    {

    }
}