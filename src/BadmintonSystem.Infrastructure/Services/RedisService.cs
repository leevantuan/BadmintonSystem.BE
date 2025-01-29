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
    public async Task SetAsync<T>(string cacheKey, T response, TimeSpan timeOut)
    {
        // Convert sang dạng string
        string serializerResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        // Lưu trữ dữ liệu vào Redis cache
        await distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeOut
        });
    }

    public async Task<string> GetAsync(string cacheKey)
    {
        // Lấy dữ liệu từ Redis cache
        string? cacheResponse = await distributedCache.GetStringAsync(cacheKey);

        return string.IsNullOrEmpty(cacheResponse) ? string.Empty : cacheResponse;
    }

    public async Task DeleteAsync(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
        {
            throw new ArgumentException("Value cannot be null or white space");
        }

        // Tìm kiếm tất cả các key có first: pattern
        await foreach (string key in GetKeyAsync(pattern + "*"))
        {
            // Xoá các key tìm được
            await distributedCache.RemoveAsync(key);
        }
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
