using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapi.Data;
using webapi.Models;

namespace webapi.Views.Pages.Books;

public class CreateModel : PageModel
{
    private readonly ApplicationDBContext _context;
    
    public CreateModel(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> OnPostAsync(string bookTitle, string? bookDescription, string authorName)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(bookTitle))
        {
            ModelState.AddModelError(nameof(bookTitle), "Book Title is required.");
        }
        
        if (string.IsNullOrWhiteSpace(authorName))
        {
            ModelState.AddModelError(nameof(authorName), "Author Name is required.");
        }
        
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        try
        {
            var book = new webapi.Models.Books
            {
                BookTitle = bookTitle.Trim(),
                BookDescription = bookDescription?.Trim(),
                AuthorName = authorName.Trim()
            };
            
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred while creating the book: {ex.Message}");
            return Page();
        }
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
