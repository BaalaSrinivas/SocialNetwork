using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Models
{
    public class FollowMetaData
    {
        public FollowMetaData(string userId)
        {
            UserId = userId;    
        }
        public string UserId { get; set; }
        public int FollowersCount { get; set; }
        public int FriendsCount { get; set; }
    }
}
