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
    public class FollowEntityRepository : IFollowEntityRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public FollowEntityRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public async Task<bool> AddItemAsync(FollowEntity item)
        {
            var sql = "INSERT INTO FollowEntities (Id, Follower, Following) VALUES (@id, @follower, @following)";
            return await _sqlConnection.ExecuteAsync(sql, item, transaction: _dbTransaction) > 0;
        }

        public async Task<IEnumerable<string>> GetFollowers(string userId)
        {
            var sql = "SELECT Follower FROM FollowEntities WHERE Following = @following";
            return await _sqlConnection.QueryAsync<string>(sql, param: new { following = userId }, transaction: _dbTransaction);
        }

        public async Task<IEnumerable<string>> GetFollowing(string userId)
        {
            var sql = "SELECT Following FROM FollowEntities WHERE Follower = @follower";
            return await _sqlConnection.QueryAsync<string>(sql, param: new { follower = userId }, transaction: _dbTransaction);
        }

        public async Task<bool> RemoveItemAsync(FollowEntity item)
        {
            var sql = "DELETE FROM FollowEntities WHERE Follower = @follower AND Following = @following";
            return await _sqlConnection.ExecuteAsync(sql, item, transaction: _dbTransaction) > 0;
        }
    }
}
