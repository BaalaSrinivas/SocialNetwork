using ContentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Repository
{
    public interface IContentRepository
    {
        public Task<bool> CreatePost(Post post);

        public Task<bool> UpdatePostContent(Guid postId, string content);

        public Task<bool> Like(Like like);

        public Task<bool> AddComment(Comment comment);

        public Task<IEnumerable<Guid>> GetUserPosts(string userId, int count);

        public Task<IEnumerable<Post>> GetPosts(IEnumerable<Guid> postIds);

        public Task<IEnumerable<Comment>> GetComments(Guid postId);

        public Task<IEnumerable<string>> GetLikedUsers(Guid parentId);
    }
}
