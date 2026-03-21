using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapi.Data;
using webapi.Models;

namespace webapi.Views.Pages;

public class GamePageModel : PageModel
{
    private readonly ApplicationDBContext _context;
    public IList<Game> Games { get; set; } = default!;
    
    public GamePageModel(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public void OnGetAsync()
    {
        Games = _context.Games.ToList();
    }
    
    public async Task<IActionResult> OnPostAsync(string name, Genre genre, DateOnly releaseDate)
    {
        var game = new Game
        {
            Name = name,
            Genre = genre,
            ReleaseDate = releaseDate
        };
        
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        
        return RedirectToPage();
    }
}