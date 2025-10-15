namespace SocialMedia.Models
{
    // ==========================================================
    //  APPLICATION USER MODEL
    //  Extends the default ASP.NET IdentityUser class to include
    //  additional domain-specific fields for the SocialMedia app.
    //
    //  • Description : optional profile text (max. 300 chars)
    //  • CreatedAt   : UTC timestamp of account creation
    //  • Chirps      : collection of user-created posts
    //  • Likes       : collection of user reactions (likes)
    //
    //  Acts as the central user entity in the application’s
    //  authentication and relational data model.
    // ==========================================================
    public class ApplicationUser : IdentityUser
    {
        [StringLength(300)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Chirp> Chirps { get; set; } = new List<Chirp>();

        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
