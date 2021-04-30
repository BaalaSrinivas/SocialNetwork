using FollowService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public interface IFollowEntityRepository: IGenericRepository<FollowEntity>
    {
        public Task<IEnumerable<string>> GetFollowers(string userId);
    }
}
