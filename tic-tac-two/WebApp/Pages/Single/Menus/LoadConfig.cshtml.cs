using System.Collections;
using DAL;
using GameController;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game.Menus;

public class LoadConfig : PageModel
{
    public static IConfigRepository ConfigRepository { get; set; } = GameController.GameController.GetConfigRepository();
    public List<string> ConfigItems { get; set; } = ConfigRepository.GetConfigurationNames();

    public void OnGet()
    {
        
    }
}