using FollowService.Models;
using FollowService.Repository;
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

        private IUnitofWork _unitofWork;

        public FollowController(ILogger<FollowController> logger, IUnitofWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        [HttpGet]
        [Route("GetUserFollowInfo")]
        public async Task<FollowMetaData> GetUserFollowInfo()
        {
            return await _unitofWork.FollowMetaDataRepository.GetFollowMetaData("User Id");
        }

        [HttpPost]
        [Route("FollowUser")]
        public async Task<bool> FollowUser(string targetUserId)
        {
            FollowEntity followEntity = new FollowEntity() { Id = Guid.NewGuid(), TargetUserId = targetUserId, UserId = "User Id" };
            return await _unitofWork.FollowEntityRepository.AddItemAsync(followEntity);
        }

        [HttpPost]
        [Route("UnFollowUser")]
        public async Task<bool> UnFollowUser(FollowEntity followEntity)
        {
            followEntity.UserId = "User Id";
            return await _unitofWork.FollowEntityRepository.RemoveItemAsync(followEntity);
        }

        [HttpPost]
        [Route("SendFriendRequest")]
        public async Task<bool> SendFriendRequest(string targetUserId)
        {
            FriendEntity friendEntity = new FriendEntity()
            {
                Id = Guid.NewGuid(),
                FriendRequestState = FriendRequestState.Requested,
                TargetUserId = targetUserId,
                UserId = "User Id"
            };
            return await _unitofWork.FriendEntityRepository.AddItemAsync(friendEntity);
        }

        [HttpPost]
        [Route("Unfriend")]
        public async Task<bool> Unfriend(FriendEntity friendEntity)
        {
            friendEntity.UserId = "User Id";
            return await _unitofWork.FriendEntityRepository.RemoveItemAsync(friendEntity);
        }
        
        [HttpPost]
        [Route("UpdateFriendRequest")]
        public async Task<bool> UpdateFriendRequest(string requestId, FriendRequestState friendRequestState)
        {
            return await _unitofWork.FriendEntityRepository.UpdateFriendRequest(requestId, friendRequestState);
        }

        [HttpGet]
        [Route("GetFriendRequests")]
        public async Task<IEnumerable<FriendEntity>> GetFriendRequests()
        {
            return await _unitofWork.FriendEntityRepository.GetFriendRequestsAsync("User Id");
        }

        [HttpPost]
        [Route("GetFriends")]
        public async Task<IEnumerable<string>> GetFriends()
        {
            return _unitofWork.FriendEntityRepository.GetFriendsAsync("User Id").Result.Select(s=>s.TargetUserId);
        }

        [HttpPost]
        [Route("AddNewUser")]
        public async Task<bool> AddNewUser(string userId)
        {
            FollowMetaData followInfo = new FollowMetaData(userId);
            return await _unitofWork.FollowMetaDataRepository.AddItemAsync(followInfo);
        }
    }
}
