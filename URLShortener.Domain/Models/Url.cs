namespace URLShortener.Domain.Models;

/// <summary>
/// Represents a Url object
/// </summary>
public record Url
{
    /// <summary>
    /// Unique identifier for the Url.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The original long Url.
    /// </summary>
    public string OriginalUrl { get; init; } = null!;

    /// <summary>
    /// The shortened Url.
    /// </summary>
    public string ShortUrl { get; init; } = null!;

    /// <summary>
    /// The date and time when the Url was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }
}