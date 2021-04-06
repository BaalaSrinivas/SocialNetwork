using ContentService.Context;
using ContentService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Repository
{
    public class ContentRepository : IContentRepository
    {
        private SqlContext _sqlContext;
        public ContentRepository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> AddComment(Comment comment)
        {
            _sqlContext.Comments.Add(comment);
            return await _sqlContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreatePost(Post post)
        {
            _sqlContext.Posts.Add(post);
            return await _sqlContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Comment>> GetComments(Guid postId)
        {
            return await _sqlContext.Comments.Where(p => p.PostId == postId).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetLikedUsers(Guid parentId)
        {
            return await _sqlContext.Likes.Where(l => l.ParentId == parentId).Select(u => u.UserId).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPosts(IEnumerable<Guid> postIds)
        {
            return await _sqlContext.Posts.Where(p => postIds.Contains(p.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Guid>> GetUserPosts(string userId)
        {
            return await _sqlContext.Posts.Where(p => p.UserId == userId).Select(u=>u.Id).ToListAsync();
        }

        public async Task<bool> Like(Like like)
        {
            Post post = await _sqlContext.Posts.SingleOrDefaultAsync(p => p.Id == like.ParentId);
            post.LikeCount += 1;
            _sqlContext.Posts.Update(post);
            _sqlContext.Likes.Add(like);
            
            return await _sqlContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePostContent(Guid postId, string content)
        {
            Post post = await _sqlContext.Posts.SingleOrDefaultAsync(p => p.Id == postId);
            post.Content = content;
            _sqlContext.Posts.Update(post);
            return await _sqlContext.SaveChangesAsync() > 0;
        }
    }
}
