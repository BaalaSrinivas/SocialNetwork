using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsfeedService.Repository
{
    public class RedisCacheRepository : ICacheRepository
    {
        IDatabase _database;
        public RedisCacheRepository(ConnectionMultiplexer cache)
        {
            _database = cache.GetDatabase();
        }
        public async Task<string> RetrieveDataAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task<bool> StoreDataAsync(string key, string value)
        {
            return await _database.StringSetAsync(key, value);
        }
    }
}
