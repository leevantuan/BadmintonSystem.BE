namespace BadmintonSystem.Application.Abstractions;

public interface IRedisService
{
    Task SetAsync<T>(string key, T value, TimeSpan expiration);

    Task<string> GetAsync(string key);

    Task DeleteAsync(string pattern);
}
