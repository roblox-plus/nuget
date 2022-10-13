using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Roblox.Economy;

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

        services.AddRobloxHttpClient<IEconomyTransactionsClient, EconomyTransactionsClient>(configureHttpClient, httpMessageHandlerFactory);
    }

    private static IHttpClientBuilder AddRobloxHttpClient<TClientInterface, TClient>(
        this IServiceCollection services,
        Action<IServiceProvider, HttpClient> configureHttpClient = null,
        Func<IServiceProvider, HttpMessageHandler> httpMessageHandlerFactory = null)
        where TClientInterface : class
        where TClient : class, TClientInterface
    {
        var httpClientBuilder = services.AddHttpClient<TClientInterface, TClient>((serviceProvider, httpClient) =>
            {
                configureHttpClient?.Invoke(serviceProvider, httpClient);
            });

        if (httpMessageHandlerFactory != null)
        {
            httpClientBuilder = httpClientBuilder.ConfigurePrimaryHttpMessageHandler(httpMessageHandlerFactory);
        }

        return httpClientBuilder;
    }
}