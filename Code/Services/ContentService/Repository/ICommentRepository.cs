using ContentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Repository
{
    public interface ICommentRepository
    {
        public Task AddComment(Comment comment);

        public Task<IEnumerable<Comment>> GetComments(Guid postId);

        public Task<int> AddLikeCount(Guid commentId);
    }
}
