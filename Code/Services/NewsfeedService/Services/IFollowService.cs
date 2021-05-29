using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsfeedService.Services
{
    public interface IFollowService
    {
        public Task<IEnumerable<string>> GetUserFollowingAsync(string userId, string token);
    }
}
