using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Model;

namespace UserManagement.Repository
{
    public interface IUserRepository
    {
        User GetUser(Guid id);

        bool CreateUser(User user);

        bool UpdateUser(User user);

        bool DeleteUser(Guid id);
    }
}
