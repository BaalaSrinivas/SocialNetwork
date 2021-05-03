using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Repository;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("userapi/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private ISMUserRepository _repository;
        public UserController(ISMUserRepository userRepository)
        {
            _repository = userRepository;
        }

        [HttpGet("{mailId}")]
        public SMUser GetUser(string mailId)
        {
            return _repository.GetUser(mailId);
        }

        [HttpPost]
        public JsonResult Create(SMUser user)
        {
            string result = "Success";
            user.Timestamp = DateTime.UtcNow;
            try
            {
                if (_repository.GetUser(user.MailId) == null)
                {
                    _repository.CreateUser(user);
                    _repository.Save();
                }
                else
                {
                    result = $"User already exist. Try to log in with {user.MailId}";
                }
            }
            catch(Exception ex)
            {
                result = "Failed";
            }

            return new JsonResult(result);
        }

        [HttpPut]
        public bool Update(SMUser user)
        {
            bool result = true;

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
    }
}
