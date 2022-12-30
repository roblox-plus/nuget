using System.Runtime.Serialization;

namespace Roblox.Authentication;

[DataContract]
internal class OAuthTokenResult
{
    [DataMember(Name = "access_token")]
    public string AccessToken { get; set; }

    [DataMember(Name = "refresh_token")]
    public string RefreshToken { get; set; }
}
