using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class AddConfig : PageModel
{
    [BindProperty]
    public string Name { get; set; } = default!;
    [BindProperty]
    public int BWidth { get; set; } = 5;
    [BindProperty]
    public int BHeight { get; set; } = 5;
    [BindProperty]
    public int GWidth { get; set; } = 3;
    [BindProperty]
    public int GHeight { get; set; } = 3;
    [BindProperty]
    public int WinCond { get; set; } = 3;
    [BindProperty]
    public int MovesBeforeGrid { get; set; } = 2;
    [BindProperty]
    public int PieceCount { get; set; } = 4;
    [BindProperty]
    public bool MovablePieces { get; set; } = true;
    public List<string> Messages { get; set; } = new List<string>();

    public void OnGet()
    {
    }

    public void OnPost()
    {
        if (string.IsNullOrEmpty(Name))
        {
            Messages.Add("Name can't be empty!");
        }

        if (BWidth < 3 || BHeight < 3)
        {
            Messages.Add("Minimal board size is 3 x 3");
        }

        if (GWidth < 3 || GHeight < 3 || GWidth > BWidth || GHeight > BHeight)
        {
            Messages.Add($"Minimal grid size is 3 x 3 and maximal size is {BWidth} x {BHeight}");
        }

        if (WinCond < 3)
        {
            Messages.Add("Minimal game winning line size is 3.");
        }

        if (MovesBeforeGrid < 0)
        {
            Messages.Add("Moves before grid movement can't be negative");
        }

        if (PieceCount < WinCond)
        {
            Messages.Add($"Minimal piece count is {WinCond}");
        }

        if (Messages.Count != 0) return;
        Messages.Add("Configuration Added");
        var configuration = new GameConfiguration
        {
            Name = Name.Trim(),
            BoardSizeWidth = BWidth,
            BoardSizeHeight = BHeight,
            GridSizeWidth = GWidth,
            GridSizeHeight = GHeight,
            WinCondition = WinCond,
            MoveGridAfterNMoves = MovesBeforeGrid,
            PieceCount = PieceCount,
            MoveablePieces = MovablePieces
        };
        var configRepository = GameController.GameController.GetConfigRepository();
        configRepository.SaveConfig(configuration);

    }
}
