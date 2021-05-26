using Dapper;
using FollowService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public class FollowMetaDataRepository : IFollowMetaDataRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public FollowMetaDataRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public async Task<bool> AddItemAsync(FollowMetaData item)
        {
            var sql = "INSERT INTO FollowMetaData([UserId], [FollowersCount], [FriendsCount]) VALUES (@userId, @followersCount, @friendsCount)";
            return await _sqlConnection.ExecuteAsync(sql, item, transaction: _dbTransaction) > 0;
        }

        public async Task<FollowMetaData> GetFollowMetaData(string userId)
        {
            var sql = "SELECT UserId, FollowersCount, FriendsCount FROM FollowMetaData WHERE UserId = @userId";
            return await _sqlConnection.QueryFirstOrDefaultAsync<FollowMetaData>(sql, param: new { userId = userId }, transaction: _dbTransaction);
        }

        public async Task<bool> RemoveItemAsync(FollowMetaData item)
        {
            var sql = "DELETE FROM FollowMetaData where UserId = @userId";
            return await _sqlConnection.ExecuteAsync(sql, item, transaction: _dbTransaction) > 0;
        }

        public async Task<bool> AddFollowerCount(string userId)
        {
            var sql = "UPDATE FollowMetaData SET FollowersCount = FollowersCount + 1 where UserId = @userId";
            return await _sqlConnection.ExecuteAsync(sql, param: new { userId = userId }, transaction: _dbTransaction) > 0;
        }

        public async Task<bool> ReduceFollowerCount(string userId)
        {
            var sql = "UPDATE FollowMetaData SET FollowersCount = FollowersCount - 1 where UserId = @userId";
            return await _sqlConnection.ExecuteAsync(sql, param: new { userId = userId }, transaction: _dbTransaction) > 0;
        }

        public async Task<bool> AddFriendsCount(string userId)
        {
            var sql = "UPDATE FollowMetaData SET FriendsCount = FriendsCount + 1 where UserId = @userId";
            return await _sqlConnection.ExecuteAsync(sql, param: new { userId = userId }, transaction: _dbTransaction) > 0;
        }

        public async Task<bool> ReduceFriendsCount(string userId)
        {
            var sql = "UPDATE FollowMetaData SET FriendsCount = FriendsCount - 1 where UserId = @userId";
            return await _sqlConnection.ExecuteAsync(sql, param: new { userId = userId }, transaction: _dbTransaction) > 0;
        }
    }
}
