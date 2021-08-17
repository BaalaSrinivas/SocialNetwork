using ContentService.Context;
using ContentService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Repository
{
    public class PostRepository : IPostRepository
    {
        private SqlContext _sqlContext;
        public PostRepository(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task CreatePost(Post post)
        {
            await _sqlContext.Posts.AddAsync(post);
        }

        public async Task<List<Post>> GetPosts(IEnumerable<Guid> postIds)
        {
            return await _sqlContext.Posts.Where(p => postIds.Contains(p.Id)).OrderByDescending(p => p.Timestamp).ToListAsync();
        }

        public async Task<IEnumerable<Guid>> GetUserPosts(string userId, int count)
        {
            return await _sqlContext.Posts.Where(p => p.UserId == userId && p.IsSoftDelete == false).OrderByDescending(p=>p.Timestamp).Take(count).Select(u=>u.Id).ToListAsync();
        }

        public async Task<int> AddLikeCount(Guid postId)
        {
            Post post = await _sqlContext.Posts.SingleOrDefaultAsync(p => p.Id == postId);
            if (!post.IsSoftDelete)
            {
                post.LikeCount += 1;
                _sqlContext.Posts.Update(post);
            }
            return post.LikeCount;
        }

        public async Task<int> ReduceLikeCount(Guid postId)
        {
            Post post = await _sqlContext.Posts.SingleOrDefaultAsync(p => p.Id == postId);
            if (!post.IsSoftDelete)
            {
                post.LikeCount -= 1;
                _sqlContext.Posts.Update(post);
            }
            return post.LikeCount;
        }

        public async Task<int> AddCommentCount(Guid postId)
        {
            Post post = await _sqlContext.Posts.SingleOrDefaultAsync(p => p.Id == postId);
            if (!post.IsSoftDelete)
            {
                post.CommentCount += 1;
                _sqlContext.Posts.Update(post);
            }
            return post.CommentCount;
        }

        public async Task UpdatePostContent(Guid postId, string content)
        {
            Post post = await _sqlContext.Posts.SingleOrDefaultAsync(p => p.Id == postId);
            if (!post.IsSoftDelete)
            {
                post.Content = content;
                _sqlContext.Posts.Update(post);
            }
        }

        public async Task<Post> GetPost(Guid postId)
        {
            return await _sqlContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        }

        public async Task SoftDeletePost(Guid postId, string userId)
        {
            Post post = await _sqlContext.Posts.SingleOrDefaultAsync(p => p.Id == postId);
            //Verify if user is authorized to delete
            if (post.UserId == userId)
            {
                post.IsSoftDelete = true;
                _sqlContext.Posts.Update(post);
            }
        }
    }
}
