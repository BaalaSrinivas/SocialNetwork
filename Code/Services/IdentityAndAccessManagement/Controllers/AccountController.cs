using IdentityAndAccessManagement.Data;
using IdentityAndAccessManagement.Models;
using IdentityAndAccessManagement.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        [Route("/register")]
        public async Task<IdentityResult> Register(SocialUser socialUser)
        {
            IdentityUser user = new IdentityUser() { UserName = socialUser.Name, Email = socialUser.MailId };
            return await _identityService.Register(user, socialUser.Password);
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(SocialUser socialUser)
        {
            IdentityUser user = await _identityService.FindByMailId(socialUser.MailId);
            AuthenticationProperties authenticationProperties = new AuthenticationProperties();
            if (await _identityService.CheckCredentials(user, socialUser.Password))
            {
                authenticationProperties.AllowRefresh = true;
                authenticationProperties.ExpiresUtc = DateTime.Now.AddMinutes(_configuration.GetValue("TokenLifeTimeInMins", 150));
                authenticationProperties.RedirectUri = "https://www.google.com";
                await _identityService.SignIn(user, authenticationProperties);
            }
            return Redirect(authenticationProperties.RedirectUri);
        }

        [HttpGet]
        [Route("/logout")]
        public async void LogOut()
        {
            await _identityService.SignOut();
        }
    }
}
