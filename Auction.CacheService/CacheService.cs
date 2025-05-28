using Auction.Application.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Data;

namespace Auction.CacheService
{
    public class CacheService(IConnectionMultiplexer multiplexer) : ICacheService
    {
        private readonly IConnectionMultiplexer multiplexer = multiplexer;
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var db = multiplexer.GetDatabase();
            var json = await db.StringGetAsync(key);
            return json.HasValue ? JsonConvert.DeserializeObject<T>(json) : default;
        }

        public async Task<bool> HashExistsAsync(string key, string field)
        {
            var db = multiplexer.GetDatabase();
            return await db.HashExistsAsync(key, field); ;
        }

        public async Task<Dictionary<string, T>> HashGetAllAsync<T>(string key)
        {
            var db = multiplexer.GetDatabase();
            var entries = await db.HashGetAllAsync(key);
            return entries.ToDictionary(
                key => key.Name.ToString(),
                value => JsonConvert.DeserializeObject<T>(value.Value));
        }

        public async Task<T?> HashGetAsync<T>(string key, string field)
        {
            var db = multiplexer.GetDatabase();
            var value = await db.HashGetAsync(key, field);
            return value.HasValue ? JsonConvert.DeserializeObject<T>(value) : default;
        }

        public async Task HashRemoveAsync(string key, string field)
        {
            var db = multiplexer.GetDatabase();
            await db.HashDeleteAsync(key, field);
        }

        public async Task HashSetAsync<T>(string key, string field, T value)
        {
            var db = multiplexer.GetDatabase();
            await db.HashSetAsync(key, field, JsonConvert.SerializeObject(value));
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            var db = multiplexer.GetDatabase();
            await db.KeyDeleteAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration, CancellationToken cancellationToken = default)
        {
            var db = multiplexer.GetDatabase();
            await db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiration);
        }
    }
}
