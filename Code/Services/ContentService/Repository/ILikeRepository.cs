using ContentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Repository
{
    public interface ILikeRepository
    {
        public Task AddLike(Like like);

        public Task RemoveLike(string userId, Guid parentId);

        public Task<IEnumerable<string>> GetLikedUsers(Guid parentId);

        public Task<bool> HasUserLiked(string userId, Guid parentId);
    }
}
