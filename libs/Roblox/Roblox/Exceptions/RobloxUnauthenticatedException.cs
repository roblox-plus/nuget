using System.Net.Http;

namespace Roblox.Api;

/// <summary>
/// An exception thrown when you are unauthenticated against Roblox.
/// </summary>
public class RobloxUnauthenticatedException : RobloxApiException
{
    /// <summary>
    /// Initializes a new <see cref="RobloxUnauthenticatedException"/>.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequestMessage"/> that was sent.</param>
    /// <param name="httpResponse">The <see cref="HttpResponseMessage"/> that generated this exception.</param>
    public RobloxUnauthenticatedException(HttpRequestMessage httpRequest, HttpResponseMessage httpResponse)
        : base(httpRequest, httpResponse)
    {
    }
}
