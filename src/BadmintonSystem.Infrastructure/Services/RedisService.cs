using System.Net;
using BadmintonSystem.Application.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace BadmintonSystem.Infrastructure.Services;

public class RedisService(
    IDistributedCache distributedCache,
    IConnectionMultiplexer connectionMultiplexer)
    : IRedisService
{
    public async Task<string> GetAsync(string key)
    {
        // Lấy dữ liệu từ Redis cache
        string? cacheResponse = await distributedCache.GetStringAsync(key);

        return string.IsNullOrEmpty(cacheResponse) ? string.Empty : cacheResponse;
    }

    public async Task DeleteByKeyAsync(string key)
    {
        string? cacheResponse = await distributedCache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(cacheResponse))
        {
            await distributedCache.RemoveAsync(key);
        }
    }

    public async Task DeletesAsync(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
        {
            throw new ArgumentException("Value cannot be null or white space");
        }

        // Tìm kiếm tất cả các key có first: pattern
        await foreach (string key in GetKeyAsync(pattern + "*"))
        {
            string newKey = key.Replace("BMTSYS_", "");

            await distributedCache.RemoveAsync(newKey);
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry)
    {
        // Convert sang dạng string
        string serializerResponse = JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        // Lưu trữ dữ liệu vào Redis cache
        await distributedCache.SetStringAsync(key, serializerResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(30)
        });
    }

    private async IAsyncEnumerable<string> GetKeyAsync(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
        {
            throw new ArgumentException("Value cannot be null or white space");
        }

        foreach (EndPoint endPoint in connectionMultiplexer.GetEndPoints())
        {
            IServer server = connectionMultiplexer.GetServer(endPoint);

            foreach (RedisKey key in server.Keys(pattern: pattern))
            {
                yield return key.ToString();
            }
        }
    }
}
