using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace Roblox.Api;

/// <summary>
/// An exception thrown when a Roblox API request fails.
/// </summary>
public class RobloxApiException : Exception
{
    /// <summary>
    /// The <see cref="HttpStatusCode"/> from the response.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// The error code from the response body.
    /// </summary>
    public int? ErrorCode { get; }

    /// <summary>
    /// The error message from the response body.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Initializes a new <see cref="RobloxApiException"/>.
    /// </summary>
    /// <param name="message">The <see cref="Exception.Message"/>.</param>
    /// <param name="innerException">The <see cref="Exception.InnerException"/>.</param>
    public RobloxApiException(string message, Exception innerException)
        : base(message, innerException)
    {
        if (innerException is HttpRequestException httpRequestException)
        {
            StatusCode = httpRequestException.StatusCode;
        }
    }

    /// <summary>
    /// Initializes a new <see cref="RobloxApiException"/>.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequestMessage"/> that was sent.</param>
    /// <param name="httpResponse">The <see cref="HttpResponseMessage"/> that generated this exception.</param>
    public RobloxApiException(HttpRequestMessage httpRequest, HttpResponseMessage httpResponse)
        : base($"Request to Roblox failed.\n\tStatus Code: {httpResponse.StatusCode}\n\tUrl: {httpRequest.RequestUri}")
    {
        StatusCode = httpResponse.StatusCode;

        try
        {
            var responseBody = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var errors = JsonConvert.DeserializeObject<ApiErrorResult>(responseBody);
            var error = errors?.Errors.FirstOrDefault();
            ErrorCode = error?.Code;
            ErrorMessage = error?.Message;
        }
        catch
        {
            // Couldn't parse the response body.
            // Oh well.
        }
    }
}
