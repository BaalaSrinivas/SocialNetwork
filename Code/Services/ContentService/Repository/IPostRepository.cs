using ContentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Repository
{
    public interface IPostRepository
    {
        public Task CreatePost(Post post);

        public Task SoftDeletePost(Guid postId, string userId);

        public Task UpdatePostContent(Guid postId, string content);

        public Task<IEnumerable<Guid>> GetUserPosts(string userId, int count);

        public Task<List<Post>> GetPosts(IEnumerable<Guid> postIds);

        public Task<Post> GetPost(Guid postId);

        public Task<int> AddLikeCount(Guid postId);

        public Task<int> ReduceLikeCount(Guid postId);

        public Task<int> AddCommentCount(Guid postId);
    }
}
