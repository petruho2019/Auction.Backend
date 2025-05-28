using Microsoft.Extensions.Caching.Distributed;

namespace Auction.Application.Interfaces
{
    public interface ICacheService
    {
        public Task SetAsync<T>(
         string key,
         T value,
         TimeSpan? expiration,
         CancellationToken cancellationToken = default);

        public Task<T?> GetAsync<T>(
            string key,
            CancellationToken cancellationToken = default);

        public Task RemoveAsync(
            string key,
            CancellationToken cancellationToken = default);

        public Task HashSetAsync<T>(
            string key,
            string field,
            T value);

        public Task<T?> HashGetAsync<T>(
            string key,
            string field);

        public Task<Dictionary<string, T>> HashGetAllAsync<T>(
            string key);

        public Task HashRemoveAsync(
            string key,
            string field);

        public Task<bool> HashExistsAsync(string key,
            string field);
    }
}
