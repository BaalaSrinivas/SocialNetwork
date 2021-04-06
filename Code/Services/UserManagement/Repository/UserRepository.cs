using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Context;
using UserManagement.Models;

namespace UserManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private UserContext _context;
        public UserRepository(UserContext userContext)
        {
            _context = userContext;
        }
        public void CreateUser(User user)
        {
            _context.Users.Add(user);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            //TODO: Async request to archive all the contents of the user
        }

        public User GetUser(string mailId)
        {
            return _context.Users.Where(u => u.MailId == mailId).FirstOrDefault();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
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
