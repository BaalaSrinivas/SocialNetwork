using IdentityAndAccessManagement.Models;
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
        private UserManager<SocialUser> _userManager;
        private SignInManager<SocialUser> _signInManager;

        public IdentityService(UserManager<SocialUser> userManager,
            SignInManager<SocialUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> CheckCredentials(SocialUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<SocialUser> FindByMailId(string mailId)
        {
            return await _userManager.FindByEmailAsync(mailId);
        }

        public async Task<IdentityResult> Register(SocialUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<SignInResult> SignIn(LoginModel user)
        {            
            return await _signInManager.PasswordSignInAsync(user.MailId, user.Password, false, false);
        }

        public async Task<SocialUser> GetUserByMailId(string userId)
        {
            return await _userManager.FindByEmailAsync(userId);
        }
        

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ConfirmEmail(SocialUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<SocialUser> FindByUserId(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(SocialUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }
    }
}
