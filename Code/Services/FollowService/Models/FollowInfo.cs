using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Models
{
    public class FollowInfo
    {
        public string UserId { get; set; }
        public int FollowersCount { get; set; }
        public int FriendsCount { get; set; }
    }
}
