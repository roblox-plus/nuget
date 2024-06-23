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
    public string ErrorCode { get; }

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
        : this(ParseErrorCodeResult(httpResponse), httpRequest, httpResponse)
    {
        StatusCode = httpResponse.StatusCode;
    }

    /// <summary>
    /// Makes a copy of <see cref="RobloxApiException"/>.
    /// </summary>
    /// <param name="copyFromExtension">The <see cref="RobloxApiException"/> to copy from.</param>
    public RobloxApiException(RobloxApiException copyFromExtension)
        : this(copyFromExtension.Message, copyFromExtension.InnerException)
    {
        StatusCode = copyFromExtension.StatusCode;
        ErrorCode = copyFromExtension.ErrorCode;
        ErrorMessage = copyFromExtension.ErrorMessage;
    }

    private RobloxApiException(ApiErrorCodeResult errorCodeResult, HttpRequestMessage httpRequest, HttpResponseMessage httpResponse)
        : base(BuildMessage(httpRequest, httpResponse, errorCodeResult))
    {
        ErrorCode = errorCodeResult?.Code;
        ErrorMessage = errorCodeResult?.Message;
    }

    private static ApiErrorCodeResult ParseErrorCodeResult(HttpResponseMessage httpResponse)
    {
        try
        {
            var responseBody = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var errors = JsonConvert.DeserializeObject<ApiErrorResult>(responseBody);

            if (!string.IsNullOrWhiteSpace(errors?.SingularErrorCode) && !string.IsNullOrWhiteSpace(errors?.SingularErrorMessage))
            {
                return new ApiErrorCodeResult
                {
                    Code = errors.SingularErrorCode,
                    Message = errors.SingularErrorMessage
                };
            }

            return errors?.Errors.FirstOrDefault();
        }
        catch
        {
            // Couldn't parse the response body.
            // Oh well.
            return null;
        }
    }

    private static string BuildMessage(HttpRequestMessage httpRequest, HttpResponseMessage httpResponse, ApiErrorCodeResult errorCodeResult)
    {
        var message = $"Request to Roblox failed.\n\tStatus Code: {httpResponse.StatusCode}\n\tUrl: {httpRequest.RequestUri}";
        if (errorCodeResult != null)
        {
            message += $"\n\tError ({errorCodeResult.Code}): {errorCodeResult.Message}";
        }

        return message;
    }
}
