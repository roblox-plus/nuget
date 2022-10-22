using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Roblox.Catalog;
using Roblox.Economy;
using Roblox.Inventory;
using Roblox.Thumbnails;
using Roblox.Users;

namespace Roblox.Api;

/// <summary>
/// Extension methods to use to initialize this assembly.
/// </summary>
public static class StartupExtensions
{
    /// <summary>
    /// Adds Roblox clients to an <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <seealso cref="IServiceCollection"/>.</param>
    /// <param name="configureHttpClient">An optional action to configure the <see cref="HttpClient"/>.</param>
    /// <param name="httpMessageHandlerFactory">An optional function to provide the primary <see cref="HttpMessageHandler"/> for the <see cref="HttpClient"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="services"/>
    /// </exception>
    public static void AddRobloxClients(
        this IServiceCollection services,
        Action<IServiceProvider, HttpClient> configureHttpClient = null,
        Func<IServiceProvider, HttpMessageHandler> httpMessageHandlerFactory = null)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddRobloxHttpClient<ICatalogClient, CatalogClient>(configureHttpClient, httpMessageHandlerFactory);
        services.AddRobloxHttpClient<IEconomyTransactionsClient, EconomyTransactionsClient>(configureHttpClient, httpMessageHandlerFactory);
        services.AddRobloxHttpClient<IInventoryClient, InventoryClient>(configureHttpClient, httpMessageHandlerFactory);
        services.AddRobloxHttpClient<IThumbnailsClient, ThumbnailsClient>(configureHttpClient, httpMessageHandlerFactory);
        services.AddRobloxHttpClient<IUsersClient, UsersClient>(configureHttpClient, httpMessageHandlerFactory);
    }

    private static void AddRobloxHttpClient<TClientInterface, TClient>(
        this IServiceCollection services,
        Action<IServiceProvider, HttpClient> configureHttpClient = null,
        Func<IServiceProvider, HttpMessageHandler> httpMessageHandlerFactory = null)
        where TClientInterface : class
        where TClient : class, TClientInterface
    {
        services.AddHttpClient<TClientInterface, TClient>((serviceProvider, httpClient) =>
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Roblox NuGet (https://www.nuget.org/packages/Roblox)");
                configureHttpClient?.Invoke(serviceProvider, httpClient);
            })
            .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
            {
                var sendRequestHandler = httpMessageHandlerFactory != null ? httpMessageHandlerFactory(serviceProvider) : new HttpClientHandler();

                var xsrfTokenHandler = new XsrfTokenHandler();
                xsrfTokenHandler.InnerHandler = sendRequestHandler;

                return xsrfTokenHandler;
            });
    }
}
