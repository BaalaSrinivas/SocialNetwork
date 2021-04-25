using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Models
{
    public class FriendEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string TargetUserId { get; set; }
        public FriendRequestState FriendRequestState { get; set; }
    }

    public enum FriendRequestState
    {
        Requested,
        Accepted,
        Rejected
    }
}
