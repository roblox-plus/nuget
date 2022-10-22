namespace Roblox.Thumbnails;

/// <summary>
/// Thumbnail states.
/// </summary>
/// <seealso cref="ThumbnailResult.State"/>
public static class ThumbnailState
{
    /// <summary>
    /// The thumbnail is ready.
    /// </summary>
    public const string Completed = "Completed";

    /// <summary>
    /// The thumbnail is pending.
    /// </summary>
    /// <remarks>
    /// This state should be retried, the thumbnail is not ready yet.
    /// </remarks>
    public const string Pending = "Pending";

    /// <summary>
    /// The thumbnail is in a bad state.
    /// </summary>
    /// <remarks>
    /// Something went horribly wrong with this thumbnail, retrying probably won't fix it.
    /// </remarks>
    public const string Error = "Error";

    /// <summary>
    /// The thumbnail is for an item currently under moderation review.
    /// </summary>
    /// <remarks>
    /// This state should be retried, but not immediately.
    /// The item must still be reviewed by moderation.
    /// </remarks>
    public const string InReview = "InReview";

    /// <summary>
    /// The thumbnail is pending.
    /// </summary>
    /// <remarks>
    /// This state should be retried, the thumbnail is not ready yet.
    /// </remarks>
    public const string TemporarilyUnavailable = "TemporarilyUnavailable";

    /// <summary>
    /// The thumbnail has been rejected.
    /// </summary>
    /// <remarks>
    /// This thumbnail has been declined by moderation, and should not be retried.
    /// </remarks>
    public const string Blocked = "Blocked";
}
