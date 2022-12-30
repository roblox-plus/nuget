using System;
using System.Runtime.Serialization;
using System.Text;

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

    /// <summary>
    /// The value to use for the outbound authorization header.
    /// </summary>
    public string Authorization => string.IsNullOrWhiteSpace(ClientId) || string.IsNullOrWhiteSpace(ClientSecret) ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));
}
