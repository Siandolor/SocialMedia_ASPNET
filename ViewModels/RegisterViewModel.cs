namespace SocialMedia.ViewModels
{
    // ==========================================================
    //  REGISTER VIEW MODEL
    //  Defines the data structure used during user registration.
    //
    //  • Email           : user’s email address (must be valid)
    //  • UserName        : chosen username (3–16 alphanumeric characters)
    //  • Password        : user’s chosen password
    //  • ConfirmPassword : confirmation field for password validation
    //  • Description     : optional short bio or user description
    //
    //  Used by the AccountController to register new users via
    //  ASP.NET Identity. All fields are validated before submission.
    // ==========================================================
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(16, ErrorMessage = "The username must not exceed 16 characters.")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "The username may contain only letters and digits.")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "The description must not exceed 300 characters.")]
        [Display(Name = "Short Description")]
        public string? Description { get; set; }
    }
}
