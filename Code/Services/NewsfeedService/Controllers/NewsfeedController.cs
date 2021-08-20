using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsfeedService.Models;
using NewsfeedService.Repository;
using NewsfeedService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsfeedService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("newsfeedapi/v1/[controller]")]
    public class NewsfeedController : ControllerBase
    {
        IContentService _contentService;
        IFollowService _followService;
        public NewsfeedController(IContentService contentService, IFollowService followService)
        {
            _contentService = contentService;
            _followService = followService;
        }

        [HttpGet]
        [Route("GetNewsfeed")]
        public async Task<IEnumerable<string>> GetNewsfeed(string userId)
        {
            var data = "";// await _cacheRepository.RetrieveDataAsync(userId);
            if(data == "")
            {
                data = GenerateNewsfeedInternal(userId).Result;
            }
            return data.Split('|');
        }

        [HttpGet]
        [Route("GenerateNewsfeed")]
        public async Task<bool> GenerateNewsfeed(string userId)
        {
            string data = GenerateNewsfeedInternal(userId).Result;

            return true;
        }

        private async Task<string> GenerateNewsfeedInternal(string userId)
        {
            var token = HttpContext.Request.Headers["Authorization"][0];

            IEnumerable<string> following = await _followService.GetUserFollowingAsync(userId, token);

            List<Guid> posts = new List<Guid>();

            foreach (string user in following)
            {
                UserPostDTO userPostDTO = new UserPostDTO() { UserId = user, Count = 10 };
                posts.AddRange(await _contentService.GetUsersPosts(userPostDTO, token));
            }           

            return posts.Aggregate<Guid, string>("", (a, b) => { return a.ToString() + "|" + b.ToString(); }).Substring(1);
        }
    }
}
