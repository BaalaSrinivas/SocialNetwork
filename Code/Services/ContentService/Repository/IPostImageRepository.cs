using ContentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Repository
{
    public interface IPostImageRepository
    {
        public Task AddImages(IEnumerable<PostImage> postImages);

        public Task<IEnumerable<PostImage>> GetImages(string userId, int count);
    }
}
