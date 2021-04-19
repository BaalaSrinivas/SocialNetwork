using ContentService.Models;
using ContentService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Controllers
{
    [ApiController]
    [Route("contentapi/v1/[controller]")]
    public class ContentController : ControllerBase
    {
        private IContentRepository _contentRepository;
        public ContentController(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        [HttpGet]
        [Route("Test")]
        public string SampleGet()
        {
            return "hi";
        }

        [HttpPost]
        [Route("AddComment")]
        public async Task<bool> AddComment(Guid postId, string commentText)
        {
            Comment comment = new Comment()
            {
                Id = Guid.NewGuid(),
                CommentText = commentText,
                PostId = postId,
                Timestamp = DateTime.UtcNow,
                UserId = GetUserId()
            };
            return await _contentRepository.AddComment(comment);
        }

        [HttpPost]
        [Route("CreatePost")]
        public async Task<bool> CreatePost(Post post)
        {
            post.Id = Guid.NewGuid();
            post.UserId = GetUserId();
            post.Timestamp = DateTime.UtcNow;

            return await _contentRepository.CreatePost(post);
        }

        [HttpGet]
        [Route("GetComments")]
        public async Task<IEnumerable<Comment>> GetComments(Guid postId)
        {
            return await _contentRepository.GetComments(postId);
        }

        [HttpGet]
        [Route("GetLikedUsers")]
        public async Task<IEnumerable<string>> GetLikedUsers(Guid parentId)
        {
            return await _contentRepository.GetLikedUsers(parentId);
        }

        [HttpPost]
        [Route("GetPosts")]
        public async Task<IEnumerable<Post>> GetPosts(IEnumerable<Guid> postIds)
        {
            return await _contentRepository.GetPosts(postIds);
        }

        [HttpGet]
        [Route("GetUserPosts")]
        public async Task<IEnumerable<Guid>> GetUserPosts(string userId)
        {
            return await _contentRepository.GetUserPosts(userId);
        }

        [HttpPost]
        [Route("Like")]
        public async Task<bool> Like(Guid postId)
        {
            Like like = new Like()
            {
                Id = Guid.NewGuid(),
                UserId = GetUserId(),
                ParentId = postId,
                Timestamp = DateTime.UtcNow
            };
            return await _contentRepository.Like(like);
        }

        [HttpPost]
        [Route("UpdatePost")]
        public async Task<bool> UpdatePostContent(Guid postId, string content)
        {
            return await _contentRepository.UpdatePostContent(postId, content);
        }

        protected string GetUserId()
        {
            return "baala@gmail.com";
            //return this.User.Claims.First(i => i.Type == "MailId").Value;
        }
    }
}
