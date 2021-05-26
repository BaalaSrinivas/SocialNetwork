using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Models
{
    public class FollowEntity
    {
        public Guid Id { get; set; }
        public string Follower { get; set; }
        public string Following { get; set; }
    }
}
