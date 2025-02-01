using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class MenuController : PageModel
{
    public string Cmd { get; set; } = default!;

    public IActionResult OnGet()
    {
        return RedirectToPage("./Index");
    }

    public IActionResult OnPost()
    {
        Cmd = HttpContext.Request.Form["Cmd"]!;
        if (Cmd == "new")
        {
            return RedirectToPage("./New/pvp");
        }
        else if (Cmd == "load")
        {
            return RedirectToPage("");
        }
        else if (Cmd == "join")
        {
            return RedirectToPage("./Join/JoinGame");
        }
        else if (Cmd == "rules")
        {
            return RedirectToPage("./Rules");
        }
        else if (Cmd == "config")
        {
            return RedirectToPage("./AddConfig");
        }

        return RedirectToPage("./Index");
    }
}