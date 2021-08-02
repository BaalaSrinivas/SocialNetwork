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
        private IEmailService _emailService;

        public AccountController(IIdentityService identityService, IEmailService emailService)
        {
            _identityService = identityService;
            _emailService = emailService;
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
            IdentityResult result = await _identityService.Register(socialUser, registerModel.Password);

           /* if(result.Succeeded)
            {
                string token = await _identityService.GenerateEmailConfirmationTokenAsync(socialUser);
                _emailService.SendEmail(socialUser.Email, "Token", token);
            }*/

            return result;
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

        [HttpGet]
        [Route("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            SocialUser user = await _identityService.FindByUserId(userId);
            await _identityService.ConfirmEmail(user, token);

            return Redirect("http://localhost:4200/login");
        }
    }
}
