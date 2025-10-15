namespace SocialMedia.ViewModels
{
    // ==========================================================
    //  LOGIN VIEW MODEL
    //  Represents the data structure for user authentication
    //  via the login form in the MVC application.
    //
    //  • UserNameOrEmail : username or email used for login
    //  • Password        : user’s password (hidden input field)
    //  • RememberMe      : indicates whether to persist login session
    //
    //  Used by the AccountController to validate and process
    //  user credentials against the ASP.NET Identity framework.
    // ==========================================================
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username or Email")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } = false;
    }
}
