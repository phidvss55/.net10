using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data;

public class ApplicationDBContext: IdentityDbContext<AppUser>
{
    public ApplicationDBContext(DbContextOptions dbContext) : base(dbContext)
    {
        
    }      
    
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    // public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                Name = "Admin",
                NormalizedName =  "ADMIN",
                ConcurrencyStamp = "1"
            },
            new IdentityRole
            {
                Id = "b2c3d4e5-f6a7-8901-bcde-f12345678901",
                Name = "User",
                NormalizedName =  "USER",
                ConcurrencyStamp = "2"
            }
        };
         
        builder.Entity<IdentityRole>().HasData(roles);
    }
}