using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Repository;

namespace UserManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("userapi/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private ISMUserRepository _repository;
        private IConfiguration _configuration;

        public UserController(ISMUserRepository userRepository, IConfiguration configuration)
        {
            _repository = userRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("search")]
        public IEnumerable<SMUser> SearchUsers(string key)
        {
            return _repository.SearchUsers(key);
        }

        [HttpGet]
        public SMUser GetUser(string userId)
        {
            if(string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            return _repository.GetUser(userId);
        }

        [HttpPost]
        [Route("getusers")]
        public IEnumerable<SMUser> GetUsers(IEnumerable<string> userId)
        {
            return _repository.GetUsers(userId);
        }

        [HttpPost]
        public async Task<SMUser> Create([FromForm] SMUser user, IFormFile profileImage)
        {
            user.Name = GetUserName();
            user.Timestamp = DateTime.UtcNow;
            user.MailId = GetUserId();
            string imageName = $"{Guid.NewGuid()}{Path.GetExtension(profileImage.FileName)}";
            user.ProfileImageUrl = Path.Combine(_configuration.GetValue<string>("ProfileStoragePath"), imageName);

            using (Stream fileStream = new FileStream($@"C:\Work\Learning\Projects\Images\{imageName}", FileMode.Create))
            {
                await profileImage.CopyToAsync(fileStream);
            }

            try
            {
                if (_repository.GetUser(user.MailId) == null)
                {
                    _repository.CreateUser(user);
                    _repository.Save();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return _repository.GetUser(user.MailId);
        }

        [HttpPut]
        public bool Update(SMUser user)
        {
            bool result = true;
            user.MailId = GetUserId();
            try
            {
                _repository.UpdateUser(user);
                _repository.Save();
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        [HttpDelete]
        public bool Delete(SMUser user)
        {
            bool result = true;
            user.MailId = GetUserId();
            try
            {
                _repository.DeleteUser(user);
                _repository.Save();
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        protected string GetUserId()
        {
            return this.User.Claims.First(i => i.Type.Contains("mail")).Value;
        }

        protected string GetUserName()
        {
            return this.User.Claims.First(i => i.Type == "name").Value;
        }
    }
}
