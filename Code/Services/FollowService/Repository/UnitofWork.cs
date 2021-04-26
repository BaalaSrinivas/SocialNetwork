using FollowService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public class UnitofWork : IUnitofWork
    {
        public IFollowEntityRepository FollowEntityRepository { get; }
        public IFriendEntityRepository FriendEntityRepository { get; }
        public IFollowMetaDataRepository FollowMetaDataRepository { get; }

        public UnitofWork(IFollowEntityRepository followEntityRepository, IFriendEntityRepository friendEntityRepository, IFollowMetaDataRepository followInfoRepository)
        {
            FollowEntityRepository = followEntityRepository;
            FriendEntityRepository = friendEntityRepository;
            FollowMetaDataRepository = followInfoRepository;
        }
        public void Commit()
        {
            throw new NotImplementedException();
        }
    }
}
