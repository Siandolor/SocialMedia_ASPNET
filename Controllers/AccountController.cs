namespace SocialMedia.Controllers
{
    // ==========================================================
    //  ACCOUNT CONTROLLER
    //  Handles all authentication and registration logic for
    //  the SocialMedia application using ASP.NET Identity.
    //
    //  Provides endpoints for:
    //  • Register : create a new user account
    //  • Login    : authenticate an existing user
    //  • Logout   : terminate the active session
    //  • AccessDenied : handle unauthorized access attempts
    //
    //  The controller uses dependency injection to access
    //  UserManager and SignInManager for identity operations.
    // ==========================================================
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // ----------------------------------------------------------
        //  CONSTRUCTOR
        //  Injects ASP.NET Identity services for user management
        //  and sign-in handling.
        // ----------------------------------------------------------
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ==========================================================
        //  REGISTER (GET)
        //  Displays the user registration form.
        // ==========================================================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ==========================================================
        //  REGISTER (POST)
        //  Handles new user registration. Validates the input model,
        //  creates a new ApplicationUser, and logs the user in
        //  automatically upon success.
        // ==========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        // ==========================================================
        //  LOGIN (GET)
        //  Displays the login form and passes along a return URL
        //  (used for redirecting after successful authentication).
        // ==========================================================
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // ==========================================================
        //  LOGIN (POST)
        //  Processes user authentication requests. Accepts either
        //  username or email for login and validates the password
        //  using ASP.NET Identity’s SignInManager.
        //
        //  Redirects to the specified returnUrl if valid, otherwise
        //  returns to the home page.
        // ==========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.UserNameOrEmail)
                       ?? await _userManager.FindByNameAsync(model.UserNameOrEmail);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        // ==========================================================
        //  LOGOUT
        //  Signs the current user out and redirects to the home page.
        // ==========================================================
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ==========================================================
        //  ACCESS DENIED
        //  Displays an error page for unauthorized access attempts.
        // ==========================================================
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
