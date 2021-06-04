using ApiGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Services
{
    public interface IFollowService
    {
        public Task<IEnumerable<FriendEntity>> GetFriendRequests(string token);

        public Task<IEnumerable<FriendEntity>> GetFriends(string token);
    }
}
