using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data;

public class ApplicationDBContext: DbContext
{
    public ApplicationDBContext(DbContextOptions dbContext) : base(dbContext)
    {
        
    }      
    
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
}