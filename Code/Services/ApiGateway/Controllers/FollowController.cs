using ApiGateway.Models;
using ApiGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/agg/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly ILogger<FollowController> _logger;

        private readonly IFollowService _followService;

        private readonly IUserService _userService;

        public FollowController(ILogger<FollowController> logger, IFollowService followService, IUserService userService)
        {
            _logger = logger;
            _followService = followService;
            _userService = userService;
        }

        [HttpGet]
        [Route("GetFriendRequests")]
        public async Task<IEnumerable<FriendEntityDTO>> GetFriendRequests()
        {
            List<FriendEntityDTO> friendRequestDTOEntities = new List<FriendEntityDTO>();

            var token = HttpContext.Request.Headers["Authorization"][0];

            IEnumerable<FriendEntity> friendRequestEntities = await _followService.GetFriendRequests(token);

            IEnumerable<string> userIds = friendRequestEntities.Select(p => p.UserId).Distinct();

            IEnumerable<SMUser> users = await _userService.GetUsers(userIds, token);

            foreach(FriendEntity friendRequestEntity in friendRequestEntities)
            {
                SMUser user = users.First(s => s.MailId == friendRequestEntity.UserId);
                friendRequestDTOEntities.Add(new FriendEntityDTO()
                {
                    Id = friendRequestEntity.Id,
                    UserId = user.MailId,
                    UserName = user.Name,
                    ProfileImageUrl = user.ProfileImageUrl,
                    UserHeadline = user.Headline
                });
            }

            return friendRequestDTOEntities;
        }

        [HttpGet]
        [Route("GetFriends")]
        public async Task<IEnumerable<FriendEntityDTO>> GetFriends()
        {
            List<FriendEntityDTO> friendDTOEntities = new List<FriendEntityDTO>();

            var token = HttpContext.Request.Headers["Authorization"][0];

            IEnumerable<FriendEntity> friendEntities = await _followService.GetFriends(token);

            IEnumerable<string> userIds = friendEntities.Select(p => p.UserId).Distinct();

            IEnumerable<SMUser> users = await _userService.GetUsers(userIds, token);

            foreach (FriendEntity friendEntity in friendEntities)
            {
                SMUser user = users.First(s => s.MailId == friendEntity.UserId);
                friendDTOEntities.Add(new FriendEntityDTO()
                {
                    Id = friendEntity.Id,
                    UserId = user.MailId,
                    UserName = user.Name,
                    ProfileImageUrl = user.ProfileImageUrl,
                    UserHeadline = user.Headline
                });
            }

            return friendDTOEntities;
        }
    }
}
