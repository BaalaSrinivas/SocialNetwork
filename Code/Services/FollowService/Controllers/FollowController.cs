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
        public async Task<bool> FollowUser(FollowEntity follow)
        {
            string userId = GetUserId();

            if(userId == follow.Following)
            {
                return false;
            }

            FollowEntity followEntity = new FollowEntity() { Id = Guid.NewGuid(), Following = follow.Following, Follower = userId };
            Task<bool> addItemResult = _unitofWork.FollowEntityRepository.AddItemAsync(followEntity);
            Task<bool> addCountResult = _unitofWork.FollowMetaDataRepository.AddFollowerCount(userId);

            bool[] resultArr = await Task.WhenAll(addItemResult, addCountResult);
            bool result  = resultArr.Count(s => s == false) == 0;

            if (!result)
            {
                _unitofWork.Rollback();
            }
            else
            {
                _unitofWork.Commit();
            }

            return result;
        }

        [HttpPost]
        [Route("UnFollowUser")]
        public async Task<bool> UnFollowUser(FollowEntity followEntity)
        {
            followEntity.Follower = GetUserId();

            Task<bool> removeItemResult = _unitofWork.FollowEntityRepository.RemoveItemAsync(followEntity);
            Task<bool> reduceCountResult = _unitofWork.FollowMetaDataRepository.ReduceFollowerCount(followEntity.Follower);

            bool[] resultArr = await Task.WhenAll(removeItemResult, reduceCountResult);
            bool result = resultArr.Count(s => s == false) == 0;

            if (!result)
            {
                _unitofWork.Rollback();
            }
            else
            {
                _unitofWork.Commit();
            }

            return result;
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
                _logger.LogInformation($"Malicious: Friend request sent from id {GetUserId()} to {toUserId}");
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
        public async Task<bool> Unfriend(FriendEntityDTO friendEntityDTO)
        {
            if (friendEntityDTO.UserId == GetUserId())
            {
                _logger.LogInformation($"Malicious: Unfriend request sent from id {GetUserId()} to {friendEntityDTO.UserId}");
                return false;
            }

            FriendEntity friendEntity = new FriendEntity() { Id = friendEntityDTO.Id, FromUser = GetUserId(), ToUser = friendEntityDTO.UserId };

            List<Task<bool>> results = new List<Task<bool>>();

            results.Add(_unitofWork.FollowMetaDataRepository.ReduceFollowerCount(friendEntity.FromUser));
            results.Add(_unitofWork.FollowMetaDataRepository.ReduceFriendsCount(friendEntity.FromUser));

            results.Add(_unitofWork.FollowMetaDataRepository.ReduceFollowerCount(friendEntity.ToUser));
            results.Add(_unitofWork.FollowMetaDataRepository.ReduceFriendsCount(friendEntity.ToUser));

            results.Add(_unitofWork.FriendEntityRepository.RemoveItemAsync(friendEntity));

            FollowEntity followEntitySource = new FollowEntity() { Following = friendEntity.ToUser, Follower = friendEntity.FromUser };
            FollowEntity followEntityTarget = new FollowEntity() { Following = friendEntity.FromUser, Follower = friendEntity.ToUser };
            results.Add(_unitofWork.FollowEntityRepository.RemoveItemAsync(followEntitySource));
            results.Add(_unitofWork.FollowEntityRepository.RemoveItemAsync(followEntityTarget));

            bool[] resultArr = await Task.WhenAll(results);
            bool result = resultArr.Count(s => s == false) == 0;

            if (!result)
            {
                _unitofWork.Rollback();
            }
            else
            {
                _unitofWork.Commit();
            }

            return result;
        }

        [HttpPost]
        [Route("AcceptFriendRequest")]
        public async Task<bool> AcceptFriendRequest(FriendEntityDTO friendEntityDTO)
        {
            bool result = false;

            if (friendEntityDTO.UserId == GetUserId())
            {
                _logger.LogInformation($"Malicious: Accept Friend request sent from id {GetUserId()} to {friendEntityDTO.UserId}");
                return result;
            }

            FriendEntity friendEntity = new FriendEntity() { Id = friendEntityDTO.Id, FromUser = friendEntityDTO.UserId, ToUser = GetUserId() };
            friendEntity.State = State.Friends;

            List<Task<bool>> results = new List<Task<bool>>();         

            results.Add(_unitofWork.FriendEntityRepository.UpdateFriendRequest(friendEntity));

            results.Add(_unitofWork.FollowMetaDataRepository.AddFollowerCount(friendEntity.FromUser));
            results.Add(_unitofWork.FollowMetaDataRepository.AddFriendsCount(friendEntity.FromUser));

            results.Add(_unitofWork.FollowMetaDataRepository.AddFollowerCount(friendEntity.ToUser));
            results.Add(_unitofWork.FollowMetaDataRepository.AddFriendsCount(friendEntity.ToUser));

            FollowEntity followEntitySource = new FollowEntity() { Id = Guid.NewGuid(), Following = friendEntity.ToUser, Follower = friendEntity.FromUser };
            FollowEntity followEntityTarget = new FollowEntity() { Id = Guid.NewGuid(), Following = friendEntity.FromUser, Follower = friendEntity.ToUser };
            results.Add(_unitofWork.FollowEntityRepository.AddItemAsync(followEntitySource));
            results.Add(_unitofWork.FollowEntityRepository.AddItemAsync(followEntityTarget));

            bool[] resultArr = await Task.WhenAll(results);
            result = resultArr.Count(s => s == false) == 0;

            if (!result)
            {
                _unitofWork.Rollback();
            }
            else
            {
                _unitofWork.Commit();
            }

            return result;
        }

        [HttpPost]
        [Route("deleteFriendRequest")]
        public async Task<bool> DeleteFriendRequest(FriendEntityDTO friendEntityDTO)
        {
            bool result = false;
            FriendEntity friendEntity = new FriendEntity() { Id = friendEntityDTO.Id, FromUser = GetUserId(), ToUser = friendEntityDTO.UserId };

            if (friendEntity.ToUser == GetUserId())
            {
                _logger.LogInformation($"Malicious: Delete Friend request sent from id {GetUserId()} to {friendEntity.ToUser}");
                return result;
            }

            result = await _unitofWork.FriendEntityRepository.RemoveItemAsync(friendEntity);
            _unitofWork.Commit();

            return result;

        }

        [HttpGet]
        [Route("GetFriendRequests")]
        public async Task<IEnumerable<FriendEntityDTO>> GetFriendRequests()
        {
            List<FriendEntityDTO> result = new List<FriendEntityDTO>();
            IEnumerable<FriendEntity> friendRequestEntities = await _unitofWork.FriendEntityRepository.GetFriendRequestsAsync(GetUserId());

            foreach(FriendEntity friendRequestEntity in friendRequestEntities)
            {
                result.Add(new FriendEntityDTO()
                {
                    Id = friendRequestEntity.Id,
                    UserId = friendRequestEntity.FromUser == GetUserId() ? friendRequestEntity.ToUser : friendRequestEntity.FromUser
                });
            }

            return result;
        }

        [HttpGet]
        [Route("GetFriends")]
        public async Task<IEnumerable<FriendEntityDTO>> GetFriends()
        {
            List<FriendEntityDTO> result = new List<FriendEntityDTO>();
            IEnumerable<FriendEntity> friendEntities = await _unitofWork.FriendEntityRepository.GetFriendsAsync(GetUserId());

            foreach (FriendEntity friendEntity in friendEntities)
            {
                result.Add(new FriendEntityDTO()
                {
                    Id = friendEntity.Id,
                    UserId = friendEntity.FromUser == GetUserId() ? friendEntity.ToUser : friendEntity.FromUser
                });
            }

            return result;
        }

        #endregion

        #region MetaData

        [HttpGet]
        [Route("GetUserFollowInfo")]
        public async Task<FollowMetaData> GetUserFollowInfo()
        {
            return await _unitofWork.FollowMetaDataRepository.GetFollowMetaData(GetUserId());
        }

        [HttpGet]
        [Route("friendfollowInfo")]
        public FriendFollowEntity GetFriendFollowInfo(string userId)
        {
            FriendFollowEntity friendFollowEntity = new FriendFollowEntity();
            friendFollowEntity.IsFriend = _unitofWork.FriendEntityRepository.GetFriendsAsync(GetUserId()).Result.
                Count(f => (f.FromUser == userId && f.ToUser == GetUserId()) || (f.FromUser == GetUserId() && f.ToUser == userId)) > 0;
            friendFollowEntity.IsFollowing = _unitofWork.FollowEntityRepository.GetFollowing(GetUserId()).Result.Count(f => f == userId) > 0;

            return friendFollowEntity;
        }
        #endregion


        protected string GetUserId()
        {
            return this.User.Claims.First(i => i.Type.Contains("mail")).Value;
        }
    }
}
