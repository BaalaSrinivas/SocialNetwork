using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsfeedService.Models
{
    public class Userfeed
    {
        public string UserId { get; set; }

        public DateTime GeneratedTime { get; set; }

        public IEnumerable<Guid> postIds { get; set; }
    }
}
