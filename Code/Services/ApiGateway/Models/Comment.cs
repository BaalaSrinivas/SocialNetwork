using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string CommentText { get; set; }
        public Guid PostId { get; set; }
        public int LikesCount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
