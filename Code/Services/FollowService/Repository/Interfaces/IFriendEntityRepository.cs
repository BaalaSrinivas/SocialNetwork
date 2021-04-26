using FollowService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public interface IFriendEntityRepository: IGenericRepository<FriendEntity>
    {
        Task<IEnumerable<FriendEntity>> GetFriendsAsync(string userId);

        Task<IEnumerable<FriendEntity>> GetFriendRequestsAsync(string userId);

        Task<bool> UpdateFriendRequest(string requestId, FriendRequestState friendRequestState);
    }
}
