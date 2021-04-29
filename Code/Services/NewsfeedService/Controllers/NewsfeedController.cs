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
        public IEnumerable<Guid> GetNewsfeed(int index, int count)
        {
            return _cacheRepository.RetrieveDataAsync("User Id").Result.Split('|').Select(s=>Guid.Parse(s));
        }

        public async Task<bool> GenerateNewsFeed(string userId)
        {
            IEnumerable<string> followers = await _followService.GetUserFollowersAsync(userId);
            IEnumerable<Guid> posts = await _contentService.GetUsersPosts(followers, 200);

            return await _cacheRepository.StoreDataAsync(userId, posts.Aggregate<Guid,string>("",(a,b)=> { return a.ToString() + "|" + b.ToString(); }));
        }
    }
}
