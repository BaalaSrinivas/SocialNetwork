using MessageBus.MessageBusCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Events.EventModel;
using UserManagement.Models;
using UserManagement.Repository;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("userapi/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private ISMUserRepository _repository;
        private IConfiguration _configuration;
        private IBlobService _blobService;
        IQueue<UserAddedEventModel> _userAddedQueue;

        public UserController(ISMUserRepository userRepository, 
            IConfiguration configuration, 
            IBlobService blobService,
            IQueue<UserAddedEventModel> userAddedQueue)
        {
            _repository = userRepository;
            _configuration = configuration;
            _blobService = blobService;
            _userAddedQueue = userAddedQueue;
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
            var token = HttpContext.Request.Headers["Authorization"][0];

            user.ProfileImageUrl = await _blobService.UploadImage(profileImage, token);
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

            //Trigger Message to Event Queue to notify other services
            UserAddedEventModel userAddedEventModel = new UserAddedEventModel() { UserId = user.MailId, MessageText = "New user added" };
            _userAddedQueue.Publish(userAddedEventModel);

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
