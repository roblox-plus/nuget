using System.Runtime.Serialization;

namespace Roblox.Authentication;

/// <summary>
/// App authentication information.
/// </summary>
/// <remarks>
/// https://devforum.roblox.com/t/oauth-20-api-reference/1941034
/// </remarks>
[DataContract(Name = "Authentication")]
public class AuthenticationConfiguration
{
    /// <summary>
    /// The client ID of the registered Roblox app.
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// The client secret, pairing with the <see cref="ClientId"/>.
    /// </summary>
    public string ClientSecret { get; set; }
}
