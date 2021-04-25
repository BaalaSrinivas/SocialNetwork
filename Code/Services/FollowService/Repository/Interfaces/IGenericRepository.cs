using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public interface IGenericRepository<T>
    {
        public bool AddItem(T item);
        public bool RemoveItem(T item);
    }
}
