using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement.Services
{
    public class IdentityService : IIdentityService
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public IdentityService(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> CheckCredentials(IdentityUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityUser> FindByMailId(string mailId)
        {
            return await _userManager.FindByEmailAsync(mailId);
        }

        public async Task<IdentityResult> Register(IdentityUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task SignIn(IdentityUser user, AuthenticationProperties properties)
        {
            await _signInManager.SignInAsync(user, properties);
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
