using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Models
{
    public class FriendEntity
    {
        public Guid Id { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public State State { get; set; }
    }

    public enum State
    {
        Requested,
        Friends,
        None
    }
}
