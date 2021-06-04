using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class FriendEntityDTO
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }    

        public string UserName { get; set; }

        public string ProfileImageUrl { get; set; }

        public string UserHeadline { get; set; }
    }
}
