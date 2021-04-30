using Dapper;
using FollowService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public class FollowEntityRepository : IFollowEntityRepository
    {
        private SqlConnection _sqlConnection;
        public FollowEntityRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<bool> AddItemAsync(FollowEntity item)
        {
            var sql = "INSERT INTO FollowEntities (Id, UserId, TargetUserId) VALUES (@id, @userId, @targetUserId)";
            return await _sqlConnection.ExecuteAsync(sql, item) > 0;
        }

        public async Task<IEnumerable<string>> GetFollowers(string userId)
        {
            var sql = "SELECT TargetUserId FROM FollowEntities WHERE UserId = @userId";
            return await _sqlConnection.QueryAsync<string>(sql, param: new { userId = userId });
        }

        public async Task<bool> RemoveItemAsync(FollowEntity item)
        {
            var sql = "DELETE FROM FollowEntities where Id = @id AND UserId = @userId";
            return await _sqlConnection.ExecuteAsync(sql, item) > 0;
        }
    }
}
