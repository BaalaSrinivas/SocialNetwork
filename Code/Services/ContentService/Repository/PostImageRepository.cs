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

        public async Task<IEnumerable<PostImage>> GetImages(string userId, int count)
        {
            return await _sqlContext.PostImages.Where(pi => pi.UserId == userId).Take(count).ToListAsync();
        }
    }
}
