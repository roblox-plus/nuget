using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
    public async Task<Avatar.Avatar> GetUserAvatarAsync(long userId, CancellationToken cancellationToken)
    {
        var avatarResult = await _HttpClient.SendApiRequestAsync<AvatarResult>(HttpMethod.Get, RobloxDomain.AvatarApi, $"v1/users/{userId}/avatar", ImmutableDictionary<string, string>.Empty, cancellationToken);
        var assets = new List<AvatarAsset>();

        assets.AddRange(avatarResult.Emotes.Select(emote => new AvatarAsset
        {
            Id = emote.AssetId,
            Name = emote.AssetName,
            Type = AssetType.Emote,
            EmotePosition = emote.Position
        }));

        assets.AddRange(avatarResult.Assets.Select(asset => new AvatarAsset
        {
            Id = asset.Id,
            Name = asset.Name,
            Type = asset.AssetType.Value
        }));

        return new Avatar.Avatar
        {
            Type = avatarResult.Type,
            BodyColors = avatarResult.BodyColors,
            Scales = avatarResult.Scales,
            Assets = assets
        };
    }
}
