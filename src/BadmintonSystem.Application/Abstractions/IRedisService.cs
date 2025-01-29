namespace BadmintonSystem.Application.Abstractions;

public interface IRedisService
{
    Task SetAsync<T>(string key, T value);

    Task<string> GetAsync(string key);

    Task DeleteByKeyAsync(string key);

    Task DeletesAsync(string pattern);
}
