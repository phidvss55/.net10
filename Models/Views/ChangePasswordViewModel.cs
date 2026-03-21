using System.ComponentModel.DataAnnotations;

namespace webapi.Models.Views;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage =  "Email is required")]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(40, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 40 characters")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [Compare("ConfirmNewPassword", ErrorMessage = "Passwords must match")]
    public string NewPassword { get; set; }
    
    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string ConfirmNewPassword { get; set; }
}