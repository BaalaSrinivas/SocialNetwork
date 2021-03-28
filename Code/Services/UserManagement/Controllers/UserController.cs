using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Model;
using UserManagement.Repository;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserRepository _repository;
        public UserController(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        [HttpGet("{mailId}")]
        public User GetUser(string mailId)
        {
            return _repository.GetUser(mailId);
        }

        [HttpPost]
        public bool Create(User user)
        {
            bool result = true;

            try
            {
                _repository.CreateUser(user);
                _repository.Save();
            }
            catch(Exception)
            {
                result = false;
            }
            return result;
        }

        [HttpPut]
        public bool Update(User user)
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
        public bool Delete(User user)
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
