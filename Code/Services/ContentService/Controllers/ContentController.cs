using ContentService.Context;
using ContentService.Events.EventModel;
using ContentService.Models;
using ContentService.Repository;
using ContentService.Services;
using MessageBus.MessageBusCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private IPostImageRepository _postImageRepository;
        private SqlContext _sqlContext;
        private ILogger<ContentController> _logger;
        private IQueue<NewContentEventModel> _contentQueue;
        private IQueue<UserLikedEventModel> _userLikedQueue;
        private IQueue<UserCommentedEventModel> _userCommentedQueue;
        private IBlobService _blobService;

        public ContentController(IPostRepository postRepository,
            ICommentRepository commentRepository,
            ILikeRepository likeRepository,
            IPostImageRepository postImageRepository,
            SqlContext sqlContext,
            IQueue<NewContentEventModel> contentQueue,
            IQueue<UserLikedEventModel> userLikedQueue,
            IQueue<UserCommentedEventModel> userCommentedQueue,
            ILogger<ContentController> logger,
            IBlobService blobService)
        {
            _contentQueue = contentQueue;
            _userLikedQueue = userLikedQueue;
            _userCommentedQueue = userCommentedQueue;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _sqlContext = sqlContext;
            _likeRepository = likeRepository;
            _logger = logger;
            _blobService = blobService;
            _postImageRepository = postImageRepository;
        }

        [HttpGet]
        [Route("Test")]
        public string SampleGet()
        {
            _contentQueue.Publish(new NewContentEventModel() { MessageText = $"Get called at {DateTime.Now}", PostId = Guid.NewGuid() });
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

            Post post = await _postRepository.GetPost(comment.PostId);

            UserCommentedEventModel userCommentedEventModel = new UserCommentedEventModel()
            {
                CommentedBy = comment.UserId,
                CommentText = comment.CommentText,
                OwnerUserId = post.UserId,
                MessageText = "<UserName> has commented on your post"
            };

            _userCommentedQueue.Publish(userCommentedEventModel);

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
            var token = HttpContext.Request.Headers["Authorization"][0];

            Regex imageDataRegex = new Regex(@"(?<=<img src="")(.*?)(?="">)");
            MatchCollection matchCollection = imageDataRegex.Matches(post.Content);

            List<PostImage> imageList = new List<PostImage>();

            foreach (Match match in matchCollection)
            {
                string[] image = match.Value.Split(',');
                string type = image[0].Split(';')[0].Replace("data:image/", string.Empty);
                string imageString = image[1];
                byte[] imageBytes = Convert.FromBase64String(imageString);
                string path = await _blobService.UploadImage(imageBytes, type ,token);
                post.Content = post.Content.Replace(match.Value, path);

                PostImage postImage = new PostImage() { Id = Guid.NewGuid(), PostId = post.Id, UserId = post.UserId, ImageUrl = path };
                imageList.Add(postImage);
            }

            await _postImageRepository.AddImages(imageList);
            await _postRepository.CreatePost(post);
            _contentQueue.Publish(new NewContentEventModel() { MessageText = $"New post by {post.UserId}", PostUserId = post.UserId, PostId = post.Id });
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
        public async Task<IEnumerable<Guid>> GetUserPosts(UserPostDTO userPostDTO)
        {
            return await _postRepository.GetUserPosts(userPostDTO.UserId, userPostDTO.Count);
        }

        [HttpPost]
        [Route("GetUserImages")]
        public async Task<IEnumerable<PostImage>> GetImages(ImagesDTO imagesDTO)
        {
            return await _postImageRepository.GetImages(imagesDTO.UserId, imagesDTO.Count);
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

            Post post = await _postRepository.GetPost(postId);

            UserLikedEventModel userLikedEventModel = new UserLikedEventModel()
            {
                LikedBy = userId,
                OwnerUserId = post.UserId,
                MessageText = "<UserName> has liked your post"
            };

            _userLikedQueue.Publish(userLikedEventModel);

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

        [HttpPost]
        [Route("DeletePost")]
        public async Task<bool> DeletePost(Post post)
        {
            await _postRepository.SoftDeletePost(post.Id, GetUserId());
            return await _sqlContext.SaveChangesAsync() > 0;
        }

        protected string GetUserId()
        {
            return this.User.Claims.First(i => i.Type.Contains("mail")).Value;
        }
    }
}
