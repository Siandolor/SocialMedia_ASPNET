namespace SocialMedia.Controllers
{
    // ==========================================================
    //  HOME CONTROLLER
    //  Handles the main feed, chirp creation, and like toggling
    //  for the SocialMedia application.
    //
    //  Responsibilities:
    //  • Displaying the main timeline with recent chirps
    //  • Handling creation of new chirps (with peep mentions)
    //  • Managing user likes and unlikes
    //  • Displaying trending Peeps based on recent activity
    //
    //  Depends on:
    //  • ApplicationDbContext  → database access
    //  • UserManager<ApplicationUser> → user management
    // ==========================================================
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        // ----------------------------------------------------------
        //  CONSTRUCTOR
        //  Injects the EF Core database context and ASP.NET Identity
        //  user manager used throughout the controller.
        // ----------------------------------------------------------
        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ==========================================================
        //  INDEX (GET)
        //  Displays the main feed with the most recent chirps.
        //
        //  • Authenticated users see 10 chirps
        //  • Anonymous users see 5 chirps
        //
        //  Also queries trending Peeps from the last 24 hours and
        //  injects them into the ViewBag for sidebar display.
        // ==========================================================
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            bool isAuthenticated = User.Identity != null && User.Identity.IsAuthenticated;
            int count = isAuthenticated ? 10 : 5;

            var chirpEntities = await _db.Chirps
                .OrderByDescending(c => c.CreatedAt)
                .Include(c => c.User)
                .Include(c => c.ChirpPeeps)
                    .ThenInclude(cp => cp.Peep)
                .Include(c => c.Likes)
                .Take(count)
                .ToListAsync();

            var chirps = chirpEntities.Select(c => new ChirpViewModel
            {
                Id = c.Id,
                UserName = c.User.UserName,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                LikeCount = c.Likes.Count,
                LikedByCurrentUser = currentUserId != null && c.Likes.Any(l => l.UserId == currentUserId),
                Peeps = c.ChirpPeeps.Select(cp => cp.Peep.Name).ToList()
            }).ToList();

            DateTime since = DateTime.UtcNow.AddHours(-24);
            var trending = await _db.ChirpPeeps
                .Where(cp => cp.Chirp.CreatedAt >= since)
                .GroupBy(cp => cp.Peep.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            ViewBag.TrendingPeeps = trending;
            ViewBag.IsAuthenticated = isAuthenticated;

            return View(chirps);
        }

        // ==========================================================
        //  CREATE CHIRP (POST)
        //  Handles the creation of a new chirp by the authenticated
        //  user. Parses content for Peep mentions (“<Name>”) and
        //  links them to the new Chirp entity.
        //
        //  Validation:
        //  • Rejects empty or overly long content (>123 characters)
        // ==========================================================
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChirp(string content)
        {
            if (string.IsNullOrWhiteSpace(content) || content.Length > 123)
            {
                TempData["Error"] = "Chirps must not be empty and may contain a maximum of 123 characters.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var chirp = new Chirp
            {
                UserId = user.Id,
                Content = content.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            // ------------------------------------------------------
            //  PARSE PEEPS FROM CONTENT
            //  Detects mention patterns like "<Name>" and extracts
            //  valid alphanumeric names (3–16 chars) as Peeps.
            // ------------------------------------------------------
            var peepNames = new List<string>();
            int i = 0;

            while (i < content.Length)
            {
                if (content[i] == '<')
                {
                    int start = i + 1;
                    int j = start;

                    while (j < content.Length && char.IsLetterOrDigit(content[j]))
                    {
                        j++;
                    }

                    int length = j - start;
                    if (length >= 3 && length <= 16)
                    {
                        string peepName = content.Substring(start, length);
                        peepNames.Add(peepName);
                    }
                    i = j;
                }
                else
                {
                    i++;
                }
            }

            // ------------------------------------------------------
            //  LINK OR CREATE PEEPS IN DATABASE
            //  Adds references to existing Peeps or creates new ones
            //  if they do not yet exist.
            // ------------------------------------------------------
            foreach (var peepName in peepNames)
            {
                var peep = await _db.Peeps.FirstOrDefaultAsync(p => p.Name == peepName);
                if (peep == null)
                {
                    peep = new Peep { Name = peepName };
                    _db.Peeps.Add(peep);
                }

                chirp.ChirpPeeps.Add(new ChirpPeep { Chirp = chirp, Peep = peep });
            }

            _db.Chirps.Add(chirp);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // ==========================================================
        //  TOGGLE LIKE (POST)
        //  Adds or removes a like from the specified chirp,
        //  depending on whether the user has already liked it.
        //
        //  Ensures user authentication and updates the database
        //  in a single transaction.
        // ==========================================================
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
                return Challenge();

            var existing = await _db.Likes.FindAsync(userId, id);

            if (existing != null)
            {
                _db.Likes.Remove(existing);
            }
            else
            {
                var like = new Like { UserId = userId, ChirpId = id, CreatedAt = DateTime.UtcNow };
                _db.Likes.Add(like);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
