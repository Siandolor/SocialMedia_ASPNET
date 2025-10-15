namespace SocialMedia.Models
{
    // ==========================================================
    //  CHIRP–PEEP RELATION MODEL
    //  Defines a many-to-many relationship between Chirps and Peeps.
    //
    //  • ChirpId : foreign key referencing a specific Chirp
    //  • Chirp   : navigation property to the Chirp entity
    //  • PeepId  : foreign key referencing a specific Peep
    //  • Peep    : navigation property to the Peep entity
    //
    //  Serves as the link table for connecting user mentions
    //  (Peeps) with individual Chirps in the database schema.
    // ==========================================================
    public class ChirpPeep
    {
        public int ChirpId { get; set; }
        public Chirp Chirp { get; set; } = default!;

        public int PeepId { get; set; }
        public Peep Peep { get; set; } = default!;
    }
}
