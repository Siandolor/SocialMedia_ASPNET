namespace SocialMedia.Models
{
    // ==========================================================
    //  LIKE MODEL
    //  Represents a single "like" interaction between a user and a chirp.
    //
    //  • UserId    : foreign key referencing the liking user
    //  • User      : navigation property to the ApplicationUser entity
    //  • ChirpId   : foreign key referencing the liked chirp
    //  • Chirp     : navigation property to the Chirp entity
    //  • CreatedAt : UTC timestamp when the like was created
    //
    //  This model acts as a join table for user–chirp interactions,
    //  supporting analytics and engagement tracking features.
    // ==========================================================
    public class Like
    {
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        public int ChirpId { get; set; }
        public Chirp Chirp { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
