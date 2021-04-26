using Dapper;
using FollowService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public class FriendEntityRepository : IFriendEntityRepository
    {
        private SqlConnection _sqlConnection;
        public FriendEntityRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<bool> AddItemAsync(FriendEntity item)
        {
            var sql = "INSERT INTO FriendEntities (Id, UserId, TargetUserId, FriendRequestState) VALUES (@id, @userId, @targetUserId, @friendRequestState)";
            return await _sqlConnection.ExecuteAsync(sql, item) > 0;
        }

        public async Task<IEnumerable<FriendEntity>> GetFriendRequestsAsync(string userId)
        {
            var sql = "SELECT Id, UserId, TargetUserId, FriendRequestState FROM FriendEntities WHERE FriendRequestState = 0 AND UserId = @userId";
            return await _sqlConnection.QueryAsync<FriendEntity>(sql, new { userId = userId });
        }

        public async Task<IEnumerable<FriendEntity>> GetFriendsAsync(string userId)
        {
            var sql = "SELECT Id, UserId, TargetUserId, FriendRequestState FROM FriendEntities WHERE FriendRequestState = 1 AND UserId = @userId";
            return await _sqlConnection.QueryAsync<FriendEntity>(sql, new { userId = userId });
        }

        public async Task<bool> RemoveItemAsync(FriendEntity item)
        {
            var sql = "DELETE FROM FriendEntities where Id = @id AND UserId = @userId";
            return await _sqlConnection.ExecuteAsync(sql, item) > 0;
        }

        public async Task<bool> UpdateFriendRequest(string requestId, FriendRequestState friendRequestState)
        {
            var sql = "UPDATE FriendEntities SET FriendRequestState = @friendRequestState WHERE Id = @requestId";
            return await _sqlConnection.ExecuteAsync(sql, new { friendRequestState = friendRequestState, requestId = requestId }) > 0;
        }
    }
}
