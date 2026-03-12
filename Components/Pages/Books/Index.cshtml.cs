using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Components.Pages.Books;

public class IndexModel : PageModel
{
    private readonly ApplicationDBContext _context;
    
    public IndexModel(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public IList<webapi.Models.Books> Books { get; set; } = default!;
    
    public async Task OnGetAsync()
    {
        Books = await _context.Books.ToListAsync();
    }
}
