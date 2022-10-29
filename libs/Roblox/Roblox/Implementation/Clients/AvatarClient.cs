using System;
using System.Collections.Immutable;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Roblox.Api;
using Roblox.Avatar;

namespace Roblox.Groups;

/// <inheritdoc cref="IAvatarClient"/>
public class AvatarClient : IAvatarClient
{
    private readonly HttpClient _HttpClient;
    private AvatarRules _AvatarRules;

    /// <summary>
    /// Initializes a new <seealso cref="AvatarClient"/>.
    /// </summary>
    /// <param name="httpClient">The <seealso cref="HttpClient"/> to use to make the requests.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="httpClient"/>
    /// </exception>
    public AvatarClient(HttpClient httpClient)
    {
        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <inheritdoc cref="IAvatarClient.GetAvatarRulesAsync"/>
    public async Task<AvatarRules> GetAvatarRulesAsync(CancellationToken cancellationToken)
    {
        if (_AvatarRules != default)
        {
            return _AvatarRules;
        }

        var avatarRules = _AvatarRules = await _HttpClient.SendApiRequestAsync<AvatarRules>(HttpMethod.Get, RobloxDomain.AvatarApi, $"v1/avatar-rules", ImmutableDictionary<string, string>.Empty, cancellationToken);
        return avatarRules;
    }

    /// <inheritdoc cref="IAvatarClient.GetUserAvatarAsync"/>
    public Task<Avatar.Avatar> GetUserAvatarAsync(long userId, CancellationToken cancellationToken)
    {
        return _HttpClient.SendApiRequestAsync<Avatar.Avatar>(HttpMethod.Get, RobloxDomain.AvatarApi, $"v1/users/{userId}/avatar", ImmutableDictionary<string, string>.Empty, cancellationToken);
    }
}
