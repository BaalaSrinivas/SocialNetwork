using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Context;
using UserManagement.Models;

namespace UserManagement.Repository
{
    public class SMUserRepository : ISMUserRepository
    {
        private UserContext _context;
        public SMUserRepository(UserContext userContext)
        {
            _context = userContext;
        }
        public void CreateUser(SMUser user)
        {
            _context.SMUsers.Add(user);
        }

        public void DeleteUser(SMUser user)
        {
            _context.SMUsers.Remove(user);
            //TODO: Async request to archive all the contents of the user
        }

        public SMUser GetUser(string mailId)
        {
            return _context.SMUsers.Where(u => u.MailId == mailId).FirstOrDefault();
        }

        public void UpdateUser(SMUser user)
        {
            _context.SMUsers.Update(user);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void ChangeImage(string mailId, Guid imageId)
        {
            throw new NotImplementedException();
        }
    }
}
