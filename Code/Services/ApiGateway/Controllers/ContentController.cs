﻿using ApiGateway.Models;
using ApiGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/agg/[controller]")]
    public class ContentController : ControllerBase
    {
        private IContentService _contentService;

        private IUserService _userService;

        public ContentController(IContentService contentService, IUserService userService)
        {
            _contentService = contentService;
            _userService = userService;
        }

        [HttpPost]
        [Route("GetPosts")]
        public async Task<IEnumerable<PostDTO>> GetPosts(IEnumerable<Guid> postIds)
        {
            var token = HttpContext.Request.Headers["Authorization"][0];

            List<PostDTO> result = new List<PostDTO>();

            IEnumerable<Post> posts = await _contentService.GetPosts(postIds, token);

            IEnumerable<string> userIds = posts.Select(p => p.UserId).Distinct();

            IEnumerable<SMUser> userDetails = await _userService.GetUsers(userIds, token);

            foreach (Post post in posts)
            {
                SMUser targetUser = userDetails.FirstOrDefault(s => s.MailId == post.UserId);

                result.Add(new PostDTO()
                {
                    CommentCount = post.CommentCount,
                    Content = post.Content,
                    HasUserLiked = post.HasUserLiked,
                    Id = post.Id,
                    LikeCount = post.LikeCount,
                    ProfileImageUrl = targetUser.ProfileImageUrl,
                    UserId = post.UserId,
                    Timestamp = post.Timestamp,
                    UserName = targetUser.Name
                });
            }

            return result;
        }

        [HttpGet]
        [Route("GetComments")]
        public async Task<IEnumerable<CommentDTO>> GetComments(Guid postId)
        {
            var token = HttpContext.Request.Headers["Authorization"][0];

            List<CommentDTO> result = new List<CommentDTO>();

            IEnumerable<Comment> comments = await _contentService.GetComments(postId, token);

            IEnumerable<string> userIds = comments.Select(c => c.UserId).Distinct();

            IEnumerable<SMUser> userDetails = await _userService.GetUsers(userIds, token);

            foreach (Comment comment in comments)
            {
                SMUser targetUser = userDetails.FirstOrDefault(s => s.MailId == comment.UserId);

                result.Add(new CommentDTO()
                {
                    CommentText = comment.CommentText,
                    Id = comment.Id,
                    LikesCount = comment.LikesCount,
                    PostId = comment.PostId,
                    Timestamp = comment.Timestamp,
                    UserId = comment.UserId,
                    ProfileImageUrl = targetUser.ProfileImageUrl,
                    UserName = targetUser.Name
                });
            }

            return result;
        }
    }
}
