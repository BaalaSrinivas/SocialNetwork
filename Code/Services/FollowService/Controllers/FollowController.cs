using FollowService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Controllers
{
    [ApiController]
    [Route("followservice/api/v1/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly ILogger<FollowController> _logger;

        public FollowController(ILogger<FollowController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetUserFollowInfo")]
        public async Task<FollowInfo> GetUserFollowInfo()
        {

        }

        [HttpPost]
        [Route("FollowUser")]
        public async Task<bool> FollowUser(string targetUserId)
        {

        }

        [HttpPost]
        [Route("UnFollowUser")]
        public async Task<bool> UnFollowUser(string targetUserId)
        {

        }

        [HttpPost]
        [Route("SendFriendRequest")]
        public async Task<bool> SendFriendRequest(string targetUserId)
        {

        }
        
        [HttpPost]
        [Route("UpdateFriendRequest")]
        public async Task<bool> UpdateFriendRequest(string requestId, FriendRequestState friendRequestState)
        {

        }

        [HttpGet]
        [Route("GetFriendRequests")]
        public async Task<IEnumerable<FriendEntity>> GetFriendRequests()
        {
        }

        [HttpPost]
        [Route("GetFriends")]
        public async Task<IEnumerable<string>> GetFriends()
        {

        }
    }
}
