namespace BadmintonSystem.Application.Abstractions;

public interface IRedisService
{
    Task SetAsync<T>(string key, T value, TimeSpan? expiry);

    Task<string> GetAsync(string key);

    Task<List<string>> GetBeforeAsync(string pattern);

    Task DeleteByKeyAsync(string key);

    Task DeletesAsync(string pattern);
}
