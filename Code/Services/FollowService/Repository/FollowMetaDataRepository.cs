using Dapper;
using FollowService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public class FollowMetaDataRepository : IFollowMetaDataRepository
    {
        private SqlConnection _sqlConnection;
        public FollowMetaDataRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<bool> AddItemAsync(FollowMetaData item)
        {
            var sql = "INSERT INTO FollowMetaData([UserId], [FollowersCount], [FriendsCount]) VALUES (@userId, @followersCount, @friendsCount)";
            return await _sqlConnection.ExecuteAsync(sql, item) > 0;
        }

        public async Task<FollowMetaData> GetFollowMetaData(string userId)
        {
            var sql = "SELECT UserId, FollowersCount, FriendsCount FROM FollowMetaData WHERE UserId = @userId";
            return await _sqlConnection.QueryFirstOrDefaultAsync<FollowMetaData>(sql, param: new { userId = userId });
        }

        public async Task<bool> RemoveItemAsync(FollowMetaData item)
        {
            var sql = "DELETE FROM FollowInfos where UserId = @userId";
            return await _sqlConnection.ExecuteAsync(sql, item) > 0;
        }
    }
}
