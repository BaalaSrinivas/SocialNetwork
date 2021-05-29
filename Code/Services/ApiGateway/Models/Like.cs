using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
