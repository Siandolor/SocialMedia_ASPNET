namespace SocialMedia.Controllers
{
    // ==========================================================
    //  PROFILE CONTROLLER
    //  Displays a user’s public profile, including recent chirps,
    //  likes statistics, and overall activity summary.
    //
    //  Route pattern:  /Profile/{username}
    //
    //  Responsibilities:
    //  • Retrieve user details and associated chirps
    //  • Display basic engagement stats (likes given/received)
    //  • Render the user’s latest activity as ChirpViewModels
    //
    //  Depends on:
    //  • ApplicationDbContext  → database access for users, chirps, and likes
    // ==========================================================
    [Route("Profile/{username}")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _db;

        // ----------------------------------------------------------
        //  CONSTRUCTOR
        //  Injects the EF Core database context used to query
        //  user, chirp, and like data.
        // ----------------------------------------------------------
        public ProfileController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ==========================================================
        //  INDEX (GET)
        //  Displays the user’s public profile page.
        //
        //  Behavior:
        //  • Returns 404 if the username parameter is missing or invalid
        //  • Loads user with related chirps and likes
        //  • Calculates total chirps, likes received, and likes given
        //  • Selects and maps up to five recent chirps for display
        //
        //  Populates ViewBag with:
        //  • ProfileUser    → the ApplicationUser entity
        //  • ChirpCount     → number of chirps created by user
        //  • LikesReceived  → total likes received across all chirps
        //  • LikesGiven     → total likes the user has given to others
        // ==========================================================
        [HttpGet]
        public async Task<IActionResult> Index(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return NotFound();

            var user = await _db.Users
                .Include(u => u.Chirps)
                    .ThenInclude(c => c.Likes)
                .Include(u => u.Likes)
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());

            if (user == null)
                return NotFound();

            var recentChirps = user.Chirps
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .Select(c => new ChirpViewModel
                {
                    Id = c.Id,
                    UserName = user.UserName,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    LikeCount = c.Likes.Count,
                    LikedByCurrentUser = false,
                    Peeps = c.ChirpPeeps.Select(cp => cp.Peep.Name).ToList()
                })
                .ToList();

            int chirpCount = user.Chirps.Count;
            int likesReceived = user.Chirps.Sum(c => c.Likes.Count);
            int likesGiven = user.Likes.Count;

            ViewBag.ProfileUser = user;
            ViewBag.ChirpCount = chirpCount;
            ViewBag.LikesReceived = likesReceived;
            ViewBag.LikesGiven = likesGiven;

            return View(recentChirps);
        }
    }
}
