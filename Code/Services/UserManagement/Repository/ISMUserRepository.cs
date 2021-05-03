using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Repository
{
    public interface ISMUserRepository
    {
        SMUser GetUser(string mailId);

        void CreateUser(SMUser user);

        void UpdateUser(SMUser user);

        void DeleteUser(SMUser user);

        void Save();

        void ChangeImage(string mailId, Guid imageId);
    }
}
