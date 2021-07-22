using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class PostImage
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public Guid PostId { get; set; }

        public string ImageUrl { get; set; }
    }
}
