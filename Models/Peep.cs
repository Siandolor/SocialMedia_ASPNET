namespace SocialMedia.Models
{
    // ==========================================================
    //  PEEP MODEL
    //  Represents a user mention, tag, or lightweight entity
    //  associated with one or more chirps.
    //
    //  • Id         : unique identifier of the peep
    //  • Name       : alphanumeric identifier (3–16 characters)
    //                 validated via length and regex attributes
    //  • ChirpPeeps : navigation property linking Peeps to Chirps
    //
    //  Serves as the secondary entity in the many-to-many
    //  Chirp–Peep relationship, enabling tagging and mention logic.
    // ==========================================================
    public class Peep
    {
        public int Id { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "A Peep must be between 3 and 16 characters long.")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "Peeps may only contain alphanumeric characters.")]
        public string Name { get; set; } = string.Empty;

        public ICollection<ChirpPeep> ChirpPeeps { get; set; } = new List<ChirpPeep>();
    }
}
