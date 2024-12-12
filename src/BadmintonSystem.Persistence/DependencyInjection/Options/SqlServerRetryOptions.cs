using System.ComponentModel.DataAnnotations;

namespace BadmintonSystem.Persistence.DependencyInjection.Options;
public record SqlServerRetryOptions
{
    [Required, Range(5, 10)]
    public int MaxRetryCount { get; set; }

    [Required, Timestamp]
    public TimeSpan MaxRetryDelay { get; set; }
    public int[]? ErrorNumbersToAdd { get; set; }
}
