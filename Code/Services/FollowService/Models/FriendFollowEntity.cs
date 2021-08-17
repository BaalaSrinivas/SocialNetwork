using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Models
{
    public class FriendFollowEntity
    {
        public State FriendState { get; set; } = State.None;

        public bool IsFollowing { get; set; }
    }
}
