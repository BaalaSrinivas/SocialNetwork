using Microsoft.AspNetCore.Mvc;
using NewsfeedService.Repository;
using NewsfeedService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsfeedService.Controllers
{
    [ApiController]
    [Route("newsfeed/api/v1/[controller]")]
    public class NewsfeedController : ControllerBase
    {
        ICacheRepository _cacheRepository;
        IContentService _contentService;
        IFollowService _followService;
        public NewsfeedController(ICacheRepository cacheRepository, IContentService contentService, IFollowService followService)
        {
            _cacheRepository = cacheRepository;
            _contentService = contentService;
            _followService = followService;
        }

        [HttpPost]
        [Route("GetNewsfeed")]
        public async Task<IEnumerable<Guid>> GetNewsfeed(string userId, int index, int count)
        {
            var data = await _cacheRepository.RetrieveDataAsync(userId);
            if(data == null)
            {
                data = GenerateNewsfeedInternal(userId).Result;
            }
            return data.Split('|').Select(s=>Guid.Parse(s));
        }

        [HttpPost]
        [Route("GenerateNewsfeed")]
        public async Task<bool> GenerateNewsfeed(string userId)
        {
            string data = GenerateNewsfeedInternal(userId).Result;

            return await _cacheRepository.StoreDataAsync(userId, data);
        }

        private async Task<string> GenerateNewsfeedInternal(string userId)
        {
            IEnumerable<string> followers = await _followService.GetUserFollowersAsync(userId);
            IEnumerable<Guid> posts = await _contentService.GetUsersPosts(followers, 200);

            return posts.Aggregate<Guid, string>("", (a, b) => { return a.ToString() + "|" + b.ToString(); });
        }
    }
}
