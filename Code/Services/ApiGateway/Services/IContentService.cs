using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Models;

namespace ApiGateway.Services
{
    public interface IContentService
    {
        public Task<IEnumerable<Post>> GetPosts(IEnumerable<Guid> postIds, string token);

        public Task<IEnumerable<Comment>> GetComments(Guid postId, string token);

        public Task<IEnumerable<string>> GetLikedUsers(Guid parentId);

        public Task<List<PostImage>> GetImages(ImagesDTO imagesDTO, string token);
    }
}
