using FollowService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public interface IFollowMetaDataRepository: IGenericRepository<FollowMetaData>
    {
        Task<FollowMetaData> GetFollowMetaData(string userId);

        Task<bool> AddFollowerCount(string userId);

        Task<bool> ReduceFollowerCount(string userId);

        Task<bool> AddFriendsCount(string userId);

        Task<bool> ReduceFriendsCount(string userId);
    }
}
