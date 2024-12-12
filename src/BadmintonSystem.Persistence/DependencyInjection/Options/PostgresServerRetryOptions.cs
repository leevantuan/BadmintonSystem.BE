using System.ComponentModel.DataAnnotations;

namespace BadmintonSystem.Persistence.DependencyInjection.Options;
public record PostgresServerRetryOptions
{
    [Required, Range(5, 10)]
    public int MaxRetryCount { get; set; }

    [Required, Timestamp]
    public TimeSpan MaxRetryDelay { get; set; }
    public List<string>? ErrorNumbersToAdd { get; set; }
}
