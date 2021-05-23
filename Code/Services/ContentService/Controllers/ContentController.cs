using ContentService.Context;
using ContentService.Models;
using ContentService.Repository;
using MessageBus.MessageBusCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("contentapi/v1/[controller]")]
    public class ContentController : ControllerBase
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;
        private ILikeRepository _likeRepository;
        private SqlContext _sqlContext;

        IMessageBus _messageBus;

        public ContentController(IPostRepository postRepository,
            ICommentRepository commentRepository,
            ILikeRepository likeRepository,
            SqlContext sqlContext,
            IMessageBus messageBus)
        {
            _messageBus = messageBus;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _sqlContext = sqlContext;
            _likeRepository = likeRepository;
        }

        [HttpGet]
        [Route("Test")]
        public string SampleGet()
        {
            _messageBus.Publish(new Message() { MessageText = $"Get called at {DateTime.Now}" });
            return "hi";
        }

        [HttpPost]
        [Route("AddComment")]
        public async Task<int> AddComment(Comment comment)
        {
            comment.Id = Guid.NewGuid();
            comment.Timestamp = DateTime.UtcNow;
            comment.UserId = GetUserId();

            await _commentRepository.AddComment(comment);
            int result = await _postRepository.AddCommentCount(comment.PostId);
            await _sqlContext.SaveChangesAsync();

            return result;
        }

        [HttpPost]
        [Route("CreatePost")]
        public async Task<bool> CreatePost(Post post)
        {
            post.Id = Guid.NewGuid();
            post.UserId = GetUserId();
            post.Timestamp = DateTime.UtcNow;
            post.LikeCount = post.CommentCount = 0;

            await _postRepository.CreatePost(post);

            return await _sqlContext.SaveChangesAsync() > 0;
        }

        [HttpGet]
        [Route("GetComments")]
        public async Task<IEnumerable<Comment>> GetComments(Guid postId)
        {
            return await _commentRepository.GetComments(postId);
        }

        [HttpGet]
        [Route("GetLikedUsers")]
        public async Task<IEnumerable<string>> GetLikedUsers(Guid parentId)
        {
            return await _likeRepository.GetLikedUsers(parentId);
        }

        [HttpPost]
        [Route("GetPosts")]
        public async Task<IEnumerable<Post>> GetPosts(IEnumerable<Guid> postIds)
        {
            List<Post> posts = await _postRepository.GetPosts(postIds);
            string userId = GetUserId();
            foreach (Post post in posts)
            {
                post.HasUserLiked = await _likeRepository.HasUserLiked(userId, post.Id);
            }
            return posts;
        }

        [HttpPost]
        [Route("GetUserPosts")]
        public async Task<IEnumerable<Guid>> GetUserPosts(int count)
        {
            return await _postRepository.GetUserPosts(GetUserId(), count);
        }

        [HttpGet]
        [Route("LikePost")]
        public async Task<int> LikePost(Guid postId)
        {
            int result = 0;
            string userId = GetUserId();
            bool hasUserLiked = await _likeRepository.HasUserLiked(userId, postId);

            if (hasUserLiked)
            {
                await _likeRepository.RemoveLike(userId, postId);
                result = await _postRepository.ReduceLikeCount(postId);
                await _sqlContext.SaveChangesAsync();
                return result;
            }

            Like like = new Like()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ParentId = postId,
                Timestamp = DateTime.UtcNow
            };

            await _likeRepository.AddLike(like);
            result = await _postRepository.AddLikeCount(postId);
            await _sqlContext.SaveChangesAsync();
            return result;
        }

        [HttpGet]
        [Route("LikeComment")]
        public async Task<int> LikeComment(Guid commentId)
        {
            Like like = new Like()
            {
                Id = Guid.NewGuid(),
                UserId = GetUserId(),
                ParentId = commentId,
                Timestamp = DateTime.UtcNow
            };
            await _likeRepository.AddLike(like);
            int result = await _commentRepository.AddLikeCount(commentId);
            await _sqlContext.SaveChangesAsync();
            return result;
        }

        [HttpPost]
        [Route("UpdatePost")]
        public async Task<bool> UpdatePostContent(Guid postId, string content)
        {
            await _postRepository.UpdatePostContent(postId, content);
            return await _sqlContext.SaveChangesAsync() > 0;
        }

        protected string GetUserId()
        {
            return this.User.Claims.First(i => i.Type.Contains("mail")).Value;
        }
    }
}
