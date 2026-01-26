using System.ComponentModel.DataAnnotations;

namespace webapi.Dtos.Auth;

public class RegisterRequest
{
    [Required]
    public string? Username { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public string? Password { get; set; }
}