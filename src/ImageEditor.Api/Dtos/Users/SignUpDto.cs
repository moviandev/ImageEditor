using System.ComponentModel.DataAnnotations;

namespace ImageEditor.Api.Dtos.Users;
public class SignUpDto
{
    [Required(ErrorMessage = "The field {0} is required")]
    [EmailAddress(ErrorMessage = "The field {0} is invalid email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The field {0} is required")]
    [StringLength(100, ErrorMessage = "The field {0} should have at least {2} and a maximum {1} characters", MinimumLength = 6)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The passwords don't match")]
    public string ConfirmPassword { get; set; }
}
