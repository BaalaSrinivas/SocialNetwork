using IdentityAndAccessManagement.Models;
using IdentityAndAccessManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private IIdentityService _identityService;
        private IConfiguration _configuration;

        public AccountController(IIdentityService identityService, IConfiguration configuration)
        {
            _identityService = identityService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IdentityResult> Register(RegisterModel registerModel)
        {
            SocialUser socialUser = new SocialUser() {
                Name = registerModel.Name,
                Email = registerModel.MailId,
                UserName = registerModel.MailId
            };
            return await _identityService.Register(socialUser, registerModel.Password);
        }

        [HttpPost]
        [Route("login")]
        public async Task<bool> Login(LoginModel socialUser)
        {
            var userSignIn = await _identityService.SignIn(socialUser);
            return userSignIn.Succeeded;
        }

        [HttpGet]
        [Route("logout")]
        public async void LogOut()
        {
            await _identityService.SignOut();
        }
    }
}
