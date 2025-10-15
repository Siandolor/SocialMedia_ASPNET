namespace SocialMedia.Controllers
{
    // ==========================================================
    //  PEEPS CONTROLLER
    //  Displays chirps associated with a specific Peep (tag or mention).
    //
    //  Route pattern:  /Peep/{name}
    //
    //  Responsibilities:
    //  • Retrieve and display all chirps that include a given Peep name
    //  • Limit results to the 20 most recent entries
    //  • Provide context for the active Peep tag to the view
    //
    //  Depends on:
    //  • ApplicationDbContext  → database access for Chirps and Peeps
    // ==========================================================
    [Route("Peep/{name}")]
    public class PeepsController : Controller
    {
        private readonly ApplicationDbContext _db;

        // ----------------------------------------------------------
        //  CONSTRUCTOR
        //  Injects the EF Core database context used to query
        //  chirps and related Peep data.
        // ----------------------------------------------------------
        public PeepsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ==========================================================
        //  INDEX (GET)
        //  Displays up to 20 of the most recent chirps that contain
        //  a given Peep (mention/tag) by name.
        //
        //  Behavior:
        //  • Returns 404 (NotFound) if no name is provided
        //  • Performs a case-insensitive search for matching Peeps
        //  • Populates ViewBag.PeepName for use in the view
        // ==========================================================
        [HttpGet]
        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return NotFound();

            string search = name.ToLowerInvariant();

            var chirpEntities = await _db.Chirps
                .Include(c => c.User)
                .Include(c => c.Likes)
                .Include(c => c.ChirpPeeps)
                    .ThenInclude(cp => cp.Peep)
                .Where(c => c.ChirpPeeps.Any(cp => cp.Peep.Name.ToLower() == search))
                .OrderByDescending(c => c.CreatedAt)
                .Take(20)
                .ToListAsync();

            var chirps = chirpEntities.Select(c => new ChirpViewModel
            {
                Id = c.Id,
                UserName = c.User.UserName,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                LikeCount = c.Likes.Count,
                LikedByCurrentUser = false,
                Peeps = c.ChirpPeeps.Select(cp => cp.Peep.Name).ToList()
            }).ToList();

            ViewBag.PeepName = name;

            return View(chirps);
        }
    }
}
