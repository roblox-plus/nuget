using System;

namespace Roblox.Api;

/// <summary>
/// An exception thrown when the application is not properly configured.
/// </summary>
public class ConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new <see cref="ConfigurationException"/>.
    /// </summary>
    /// <param name="message">The <see cref="Exception.Message"/>.</param>
    public ConfigurationException(string message)
        : base(message)
    {
    }
}
