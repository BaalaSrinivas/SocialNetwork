using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public DateTime Timestamp { get; set; }
        public bool HasUserLiked { get; set; }
        public string UserName { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
