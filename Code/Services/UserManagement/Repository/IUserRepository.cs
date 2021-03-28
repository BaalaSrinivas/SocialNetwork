using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Model;

namespace UserManagement.Repository
{
    public interface IUserRepository
    {
        User GetUser(string mailId);

        void CreateUser(User user);

        void UpdateUser(User user);

        void DeleteUser(User user);

        void Save();
    }
}
