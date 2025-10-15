namespace SocialMedia.Models
{
    // ==========================================================
    //  CHIRP MODEL
    //  Represents a single post (similar to a tweet) created by a user.
    //
    //  • Id          : unique identifier of the chirp
    //  • UserId      : foreign key referencing the author (ApplicationUser)
    //  • User        : navigation property to the author entity
    //  • Content     : short text message (max. 123 characters)
    //  • CreatedAt   : UTC timestamp when the chirp was created
    //  • ChirpPeeps  : collection of user mentions or relationships
    //  • Likes       : collection of user likes associated with the chirp
    //
    //  This entity forms the core of the SocialMedia domain model,
    //  linking users, interactions, and engagement data.
    // ==========================================================
    public class Chirp
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = default!;

        public ApplicationUser User { get; set; } = default!;

        [Required]
        [StringLength(123, ErrorMessage = "Chirps must not exceed 123 characters.")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ChirpPeep> ChirpPeeps { get; set; } = new List<ChirpPeep>();

        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
