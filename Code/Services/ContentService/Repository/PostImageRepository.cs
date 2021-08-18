using ContentService.Context;
using ContentService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Repository
{
    public class PostImageRepository : IPostImageRepository
    {
        private SqlContext _sqlContext;

        public PostImageRepository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task AddImages(IEnumerable<PostImage> postImages)
        {
            await _sqlContext.PostImages.AddRangeAsync(postImages);
        }

        public async Task<IEnumerable<PostImage>> GetImages(string userId)
        {
            return await _sqlContext.PostImages.Where(pi => pi.UserId == userId && pi.IsSoftDelete == false).ToListAsync();
        }

        public void SoftDeleteImages(Guid postId)
        {
            List<PostImage> postImages = _sqlContext.PostImages.Where(p => p.PostId == postId).ToList();
            postImages.ForEach((i) =>
            {
                i.IsSoftDelete = true;
            });
            _sqlContext.UpdateRange(postImages);
        }
    }
}
