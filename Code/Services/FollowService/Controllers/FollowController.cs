using FollowService.Models;
using FollowService.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("followapi/v1/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly ILogger<FollowController> _logger;

        private IUnitofWork _unitofWork;

        public FollowController(ILogger<FollowController> logger, IUnitofWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        #region Follow

        [HttpGet]
        [Route("GetFollowers")]
        public async Task<IEnumerable<string>> GetFollowers()
        {
            return await _unitofWork.FollowEntityRepository.GetFollowers(GetUserId());
        }

        [HttpGet]
        [Route("GetFollowing")]
        public async Task<IEnumerable<string>> GetFollowing()
        {
            return await _unitofWork.FollowEntityRepository.GetFollowing(GetUserId());
        }

        [HttpPost]
        [Route("FollowUser")]
        public async Task<bool> FollowUser([FromBody] string followingUserId)
        {
            string userId = GetUserId();

            if(userId == followingUserId)
            {
                return false;
            }

            FollowEntity followEntity = new FollowEntity() { Id = Guid.NewGuid(), Following = followingUserId, Follower = userId };
            Task<bool> addItemResult = _unitofWork.FollowEntityRepository.AddItemAsync(followEntity);
            Task<bool> addCountResult = _unitofWork.FollowMetaDataRepository.AddFollowerCount(userId);

            bool[] result = await Task.WhenAll(addItemResult, addCountResult);
            _unitofWork.Commit();

            return result.Count(s => s == false) == 0;
        }

        [HttpPost]
        [Route("UnFollowUser")]
        public async Task<bool> UnFollowUser(FollowEntity followEntity)
        {
            followEntity.Follower = GetUserId();

            Task<bool> removeItemResult = _unitofWork.FollowEntityRepository.RemoveItemAsync(followEntity);
            Task<bool> reduceCountResult = _unitofWork.FollowMetaDataRepository.ReduceFollowerCount(followEntity.Follower);

            bool[] result = await Task.WhenAll(removeItemResult, reduceCountResult);
            _unitofWork.Commit();

            return result.Count(s => s == false) == 0;
        }

        #endregion

        #region Friend

        [HttpPost]
        [Route("SendFriendRequest")]
        public async Task<bool> SendFriendRequest([FromBody] string toUserId)
        {
            bool result = false;

            if(toUserId == GetUserId())
            {
                return result;
            }

            FriendEntity friendEntity = new FriendEntity()
            {
                Id = Guid.NewGuid(),
                State = State.Requested,
                ToUser = toUserId,
                FromUser = GetUserId()
            };
            result = await _unitofWork.FriendEntityRepository.AddItemAsync(friendEntity);
            _unitofWork.Commit();

            return result;
        }

        [HttpPost]
        [Route("Unfriend")]
        public async Task<bool> Unfriend(FriendEntity friendEntity)
        {
            friendEntity.FromUser = GetUserId();
            Task<bool> reduceFollowerCountResult = _unitofWork.FollowMetaDataRepository.ReduceFollowerCount(friendEntity.FromUser);
            Task<bool> reduceFriendCountResult = _unitofWork.FollowMetaDataRepository.ReduceFriendsCount(friendEntity.FromUser);
            Task<bool> unFriendResult = _unitofWork.FriendEntityRepository.RemoveItemAsync(friendEntity);

            FollowEntity followEntity = new FollowEntity() { Following = friendEntity.ToUser, Follower = friendEntity.FromUser };
            Task<bool> removeFollowResult = _unitofWork.FollowEntityRepository.RemoveItemAsync(followEntity);

            bool[] result = await Task.WhenAll(unFriendResult, removeFollowResult, reduceFollowerCountResult, reduceFriendCountResult);
            _unitofWork.Commit();

            return result.Count(s => s == false) == 0;
        }

        [HttpPost]
        [Route("UpdateFriendRequest")]
        public async Task<bool> UpdateFriendRequest(FriendEntity friendEntity)
        {
            bool result = false;

            friendEntity.FromUser = GetUserId();

            Task<bool> updateResult = _unitofWork.FriendEntityRepository.UpdateFriendRequest(friendEntity);
            if (friendEntity.State != State.Friends)
            {
                result = await updateResult;
                _unitofWork.Commit();
                return result;
            }

            Task<bool> addFollowerCountResult = _unitofWork.FollowMetaDataRepository.AddFollowerCount(friendEntity.FromUser);
            Task<bool> addFriendCountResult = _unitofWork.FollowMetaDataRepository.AddFriendsCount(friendEntity.FromUser);
            FollowEntity followEntity = new FollowEntity() { Id = Guid.NewGuid(), Following = friendEntity.ToUser, Follower = friendEntity.FromUser };
            Task<bool> addFollowResult = _unitofWork.FollowEntityRepository.AddItemAsync(followEntity);

            bool[] resultArr = await Task.WhenAll(updateResult, addFollowerCountResult, addFriendCountResult, addFollowResult);
            _unitofWork.Commit();

            return resultArr.Count(s => s == false) == 0;
        }

        [HttpGet]
        [Route("GetFriendRequests")]
        public async Task<IEnumerable<FriendEntity>> GetFriendRequests()
        {
            return await _unitofWork.FriendEntityRepository.GetFriendRequestsAsync(GetUserId());
        }

        [HttpGet]
        [Route("GetFriends")]
        public async Task<IEnumerable<FriendEntity>> GetFriends()
        {
            return await _unitofWork.FriendEntityRepository.GetFriendsAsync(GetUserId());
        }

        #endregion

        #region MetaData

        [HttpGet]
        [Route("GetUserFollowInfo")]
        public async Task<FollowMetaData> GetUserFollowInfo()
        {
            return await _unitofWork.FollowMetaDataRepository.GetFollowMetaData(GetUserId());
        }

        #endregion
        
        /// <summary>
        /// Will be called when a new user is added to the application
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddNewUser")]
        public async Task<bool> AddNewUser(string userId)
        {
            bool result = false;
            //TODO: To be called on listening to new user added event
            FollowMetaData followInfo = new FollowMetaData(userId);
            result = await _unitofWork.FollowMetaDataRepository.AddItemAsync(followInfo);

            _unitofWork.Commit();

            return result;
        }

        protected string GetUserId()
        {
            return this.User.Claims.First(i => i.Type.Contains("mail")).Value;
        }
    }
}
