using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Repository
{
    public interface ISMUserRepository
    {
        public IEnumerable<SMUser> SearchUsers(string key);

        SMUser GetUser(string mailId);

        IEnumerable<SMUser> GetUsers(IEnumerable<string> mailId);

        void CreateUser(SMUser user);

        void UpdateUser(SMUser user);

        void DeleteUser(SMUser user);

        void Save();

        void ChangeImage(string mailId, Guid imageId);
    }
}
