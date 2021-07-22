using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Context;
using UserManagement.Models;

namespace UserManagement.Repository
{
    public class SMUserRepository : ISMUserRepository
    {
        private UserContext _context;

        private IConfiguration _configuration;

        public SMUserRepository(UserContext userContext, IConfiguration configuration)
        {
            _context = userContext;
            _configuration = configuration;
        }

        public IEnumerable<SMUser> SearchUsers(string key)
        {
            List<SMUser> users = _context.SMUsers.Where(s => s.Name.Contains(key)).Take(15).ToList();

            users.ForEach(s =>
            {
                s.ProfileImageUrl = GetUpdatedProfileUrl(s.ProfileImageUrl);
            });

            return users;
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
            SMUser user = _context.SMUsers.Where(u => u.MailId == mailId).FirstOrDefault();
            user.ProfileImageUrl = GetUpdatedProfileUrl(user.ProfileImageUrl);
            return user;
        }

        public IEnumerable<SMUser> GetUsers(IEnumerable<string> mailId)
        {
            List<SMUser> users = _context.SMUsers.Where(u => mailId.Contains(u.MailId)).ToList();

            users.ForEach(s =>
            {
                s.ProfileImageUrl = GetUpdatedProfileUrl(s.ProfileImageUrl);
            });

            return users;
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

        //TODO: Find better alternative 
        private string GetUpdatedProfileUrl(string profileUrl)
        {
            return profileUrl.Replace("BlobUrlBSK", _configuration.GetValue<string>("ApiGateWayUrl"));
        }
    }
}
