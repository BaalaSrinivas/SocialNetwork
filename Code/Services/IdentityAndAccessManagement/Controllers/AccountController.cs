using IdentityAndAccessManagement.Models;
using IdentityAndAccessManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
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

        [HttpGet]
        [Route("login")]
        public async Task<string> Login([FromQuery] string returnUrl)
        {
            await _identityService.SignOut();
            return "User is not authenticated";
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
        public async Task<IActionResult> LogOut([FromQuery]string logoutId)
        {
            await _identityService.SignOut();
            
            //TODO:Hardcoding for now, Has to be fixed
            return Redirect("http://localhost:4200/signoutredirect");
        }
    }
}
