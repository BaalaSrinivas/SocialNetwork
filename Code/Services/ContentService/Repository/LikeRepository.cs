using ContentService.Context;
using ContentService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private SqlContext _sqlContext;

        public LikeRepository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task AddLike(Like like)
        {
            await _sqlContext.Likes.AddAsync(like);
        }

        public async Task RemoveLike(string userId, Guid parentId)
        {
            Like targetLike = await _sqlContext.Likes.FirstOrDefaultAsync(l => l.ParentId == parentId && l.UserId == userId);
            _sqlContext.Remove(targetLike);
        }

        public async Task<IEnumerable<string>> GetLikedUsers(Guid parentId)
        {
            return await _sqlContext.Likes.Where(l => l.ParentId == parentId).Select(u => u.UserId).ToListAsync();
        }

        public async Task<bool> HasUserLiked(string userId, Guid parentId)
        {
            Like like = await _sqlContext.Likes.FirstOrDefaultAsync(l => l.ParentId == parentId && l.UserId == userId);
            return like != null;
        }
    }
}
