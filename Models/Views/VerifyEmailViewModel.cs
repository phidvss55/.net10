using System.ComponentModel.DataAnnotations;

namespace webapi.Models.Views;

public class VerifyEmailViewModel
{
    [Required(ErrorMessage =  "Email is required")]
    [EmailAddress]
    public string Email { get; set; }
}