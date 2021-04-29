using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsfeedService.Repository
{
    public interface ICacheRepository
    {
        public Task<bool> StoreDataAsync(string key, string value);

        public Task<string> RetrieveDataAsync(string key);
    }
}
