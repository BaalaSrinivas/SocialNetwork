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
    public class FriendEntityRepository : IFriendEntityRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public FriendEntityRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public async Task<bool> AddItemAsync(FriendEntity item)
        {
            var sql = "INSERT INTO FriendEntities (Id, FromUser, ToUser, State) VALUES (@id, @fromUser, @toUser, @state)";
            return await _sqlConnection.ExecuteAsync(sql, item, transaction: _dbTransaction) > 0;
        }

        public async Task<IEnumerable<FriendEntity>> GetFriendRequestsAsync(string userId)
        {
            var sql = "SELECT Id, FromUser, ToUser, State FROM FriendEntities WHERE State = 0 AND ToUser = @userId";
            return await _sqlConnection.QueryAsync<FriendEntity>(sql, new { userId = userId }, transaction: _dbTransaction);
        }

        public async Task<IEnumerable<FriendEntity>> GetFriendsAsync(string userId)
        {
            var sql = "SELECT Id, FromUser, ToUser, State FROM FriendEntities WHERE State = 1 AND ToUser = @userId";
            return await _sqlConnection.QueryAsync<FriendEntity>(sql, new { userId = userId }, transaction: _dbTransaction);
        }

        public async Task<bool> RemoveItemAsync(FriendEntity item)
        {
            var sql = "DELETE FROM FriendEntities where FromUser = @fromUser AND ToUser = @toUser";
            return await _sqlConnection.ExecuteAsync(sql, item, transaction: _dbTransaction) > 0;
        }

        public async Task<bool> UpdateFriendRequest(FriendEntity item)
        {
            var sql = "UPDATE FriendEntities SET State = @state WHERE Id = @id AND ToUser = @toUser";
            return await _sqlConnection.ExecuteAsync(sql, item, transaction: _dbTransaction) > 0;
        }
    }
}
