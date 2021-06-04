using FollowService.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public class UnitofWork : IUnitofWork
    {
        public IFollowEntityRepository FollowEntityRepository { get; }
        public IFriendEntityRepository FriendEntityRepository { get; }
        public IFollowMetaDataRepository FollowMetaDataRepository { get; }

        private IDbTransaction _dbTransaction;

        public UnitofWork(IDbTransaction dbTransaction, IFollowEntityRepository followEntityRepository, IFriendEntityRepository friendEntityRepository, IFollowMetaDataRepository followInfoRepository)
        {
            _dbTransaction = dbTransaction;
            FollowEntityRepository = followEntityRepository;
            FriendEntityRepository = friendEntityRepository;
            FollowMetaDataRepository = followInfoRepository;
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch(Exception ex)
            {
                _dbTransaction.Rollback();
            }
        }

        public void Rollback()
        {
            _dbTransaction.Rollback();
        }
    }
}
