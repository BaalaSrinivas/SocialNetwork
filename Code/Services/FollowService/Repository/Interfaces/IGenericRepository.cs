using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Repository
{
    public interface IGenericRepository<T>
    {
        public Task<bool> AddItemAsync(T item);
        public Task<bool> RemoveItemAsync(T item);
        
    }
}
