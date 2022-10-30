using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Roblox.Api;
using Roblox.Avatar;
using Roblox.Thumbnails;

namespace Roblox.Catalog;

/// <inheritdoc cref="IThumbnailsClient"/>
public class ThumbnailsClient : IThumbnailsClient
{
    private readonly IAvatarClient _AvatarClient;
    private readonly HttpClient _HttpClient;
    private readonly ThumbnailClientConfiguration _Settings;
    private readonly IBatchClient<string, ThumbnailResult> _ThumbnailsClient;

    /// <summary>
    /// Initializes a new <seealso cref="CatalogClient"/>.
    /// </summary>
    /// <param name="avatarClient">An <see cref="IAvatarClient"/>.</param>
    /// <param name="httpClient">An <see cref="HttpClient"/>.</param>
    /// <param name="configuration">An <see cref="IConfiguration"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="avatarClient"/>
    /// - <paramref name="httpClient"/>
    /// - <paramref name="configuration"/>
    /// </exception>
    public ThumbnailsClient(IAvatarClient avatarClient, HttpClient httpClient, IConfiguration configuration)
    {
        _AvatarClient = avatarClient ?? throw new ArgumentNullException(nameof(avatarClient));
        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        var settings = _Settings = new ThumbnailClientConfiguration();
        var clientName = configuration.BindClientConfiguration(settings);

        _ThumbnailsClient = new BatchingClient<string, ThumbnailResult>(MultiGetThumbnailsAsync, clientName, batchSize: settings.BatchSize, throttle: settings.Throttle, sendInterval: settings.MaxWaitTime);
    }

    /// <inheritdoc cref="IThumbnailsClient.GetAssetThumbnailAsync"/>
    public Task<ThumbnailResult> GetAssetThumbnailAsync(long assetId, CancellationToken cancellationToken)
    {
        return _ThumbnailsClient.GetAsync($"Asset:{assetId}", cancellationToken);
    }

    /// <inheritdoc cref="IThumbnailsClient.GetBundleThumbnailAsync"/>
    public Task<ThumbnailResult> GetBundleThumbnailAsync(long bundleId, CancellationToken cancellationToken)
    {
        return _ThumbnailsClient.GetAsync($"BundleThumbnail:{bundleId}", cancellationToken);
    }

    /// <inheritdoc cref="IThumbnailsClient.RenderAvatarAsync"/>
    public async Task<ThumbnailResult> RenderAvatarAsync(long userId, long[] assetIds, CancellationToken cancellationToken)
    {
        var avatarRules = await _AvatarClient.GetAvatarRulesAsync(cancellationToken);
        var avatar = await _AvatarClient.GetUserAvatarAsync(userId, cancellationToken);
        var renderRequest = new AvatarRenderRequest
        {
            AvatarConfiguration = new AvatarRenderAvatarConfiguration
            {
                Assets = avatar.Assets.Where(a => a.Type != AssetType.Emote).Concat(assetIds.Select(assetId => new AvatarAsset
                {
                    Id = assetId
                })).ToArray(),
                BodyColors = new AvatarRenderBodyColors(avatarRules, avatar.BodyColors),
                Scales = avatar.Scales,
                Type = new AvatarRenderAvatarType
                {
                    Type = avatar.Type
                }
            },
            ThumbnailConfiguration = new AvatarRenderThumbnailConfiguration
            {
                Size = _Settings.ThumbnailSize
            }
        };

        return await _HttpClient.SendApiRequestAsync<AvatarRenderRequest, ThumbnailResult>(HttpMethod.Post, RobloxDomain.ThumbnailsApi, $"v1/avatar/render", ImmutableDictionary<string, string>.Empty, renderRequest, cancellationToken);
    }

    private async Task<IReadOnlyDictionary<string, ThumbnailResult>> MultiGetThumbnailsAsync(IReadOnlyCollection<string> requests, CancellationToken cancellationToken)
    {
        var requestBody = requests.Select(requestId => new ThumbnailRequest
        {
            RequestId = requestId,
            Size = _Settings.ThumbnailSize
        }).ToArray();

        var pagedResult = await _HttpClient.SendApiRequestAsync<ThumbnailRequest[], PagedResult<ThumbnailResult>>(HttpMethod.Post, RobloxDomain.ThumbnailsApi, $"v1/batch", queryParameters: null, requestBody, cancellationToken);
        return pagedResult.Data.ToDictionary(d => d.RequestId, d => d);
    }
}
