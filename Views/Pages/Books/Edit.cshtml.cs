using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using webapi.Data;

namespace webapi.Views.Pages.Books;

public class EditModel : PageModel
{
    private readonly ApplicationDBContext _context;
    
    public EditModel(ApplicationDBContext context)
    {
        _context = context;
    }

    [BindProperty]
    public webapi.Models.Books Books { get; set; } = default;
    
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return RedirectToPage("/Index");
        }
        
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book == null)
        {
            return RedirectToPage("/Index");
        }
        
        Books = book;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Console.WriteLine("Attempting to update book with ID: " + Books.Id);
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Books).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.WriteLine("Concurrency error occurred while updating the book." + $"Book ID: {Books.Id}");
        }
        return RedirectToPage("./Index");
    }
}