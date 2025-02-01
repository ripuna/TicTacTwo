using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game.Menus;

public class Index : PageModel
{

    public string Selection { get; set; } = default!;
    
    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        Selection = HttpContext.Request.Form["selection"]!;

        return Selection switch
        {
            "new" => RedirectToPage("/Game/Menus/LoadConfig"),
            "load" => RedirectToPage("/Game/Menus/LoadGame"),
            "options" => RedirectToPage("/Game/Menus/Options"),
            "rules" => RedirectToPage("/Game/Rules"),
            _ => RedirectToPage("/Game/Menus/Index")
        };
    }
}