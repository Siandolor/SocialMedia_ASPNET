namespace SocialMedia.ViewModels
{
    // ==========================================================
    //  CHIRP VIEW MODEL
    //  Defines the data structure passed from controller to view
    //  when displaying a chirp with user context and engagement info.
    //
    //  • Id                : unique identifier of the chirp
    //  • UserName          : display name of the chirp’s author
    //  • Content           : message content of the chirp
    //  • CreatedAt         : timestamp when the chirp was created
    //  • LikeCount         : total number of likes received
    //  • LikedByCurrentUser: indicates whether the current user liked it
    //  • Peeps             : list of tagged peep names within the chirp
    //
    //  Acts as a flattened representation of multiple related models
    //  for efficient rendering in MVC views.
    // ==========================================================
    public class ChirpViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int LikeCount { get; set; }

        public bool LikedByCurrentUser { get; set; }

        public List<string> Peeps { get; set; } = new List<string>();
    }
}
