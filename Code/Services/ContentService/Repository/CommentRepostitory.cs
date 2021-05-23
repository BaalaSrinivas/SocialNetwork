using ContentService.Context;
using ContentService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Repository
{
    public class CommentRepostitory : ICommentRepository
    {
        private SqlContext _sqlContext;
        public CommentRepostitory(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task AddComment(Comment comment)
        {
            await _sqlContext.Comments.AddAsync(comment);
        }

        public async Task<int> AddLikeCount(Guid commentId)
        {
            Comment comment = await _sqlContext.Comments.FirstOrDefaultAsync(s => s.Id == commentId);
            comment.LikesCount += 1;
            _sqlContext.Comments.Update(comment);
            return comment.LikesCount;
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid postId)
        {
            return await _sqlContext.Comments.Where(p => p.PostId == postId).OrderBy(p => p.Timestamp).ToListAsync();
        }
    }
}
