using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public interface IUnitofWork
    {
        public IFollowEntityRepository FollowEntityRepository { get; }
        public IFriendEntityRepository FriendEntityRepository { get; }
        public IFollowMetaDataRepository FollowMetaDataRepository { get; }

        void Commit();
    }
}
