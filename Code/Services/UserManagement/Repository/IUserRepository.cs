using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Repository
{
    public interface IUserRepository
    {
        User GetUser(string mailId);

        void CreateUser(User user);

        void UpdateUser(User user);

        void DeleteUser(User user);

        void Save();

        void ChangeImage(string mailId, Guid imageId);
    }
}
