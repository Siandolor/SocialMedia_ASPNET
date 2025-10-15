namespace SocialMedia.Models
{
    // ==========================================================
    //  ERROR VIEW MODEL
    //  Provides diagnostic information for error handling and
    //  user-facing error pages in the MVC application.
    //
    //  • RequestId     : unique identifier assigned to each HTTP request
    //  • ShowRequestId : indicates whether the RequestId should be shown
    //
    //  Used by the shared error view to display contextual
    //  request information for debugging or support purposes.
    // ==========================================================
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
