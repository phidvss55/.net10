using Microsoft.AspNetCore.Identity;

namespace webapi.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; }
    public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
}