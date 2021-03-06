using IdentityAndAccessManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement.Services
{
    public interface IIdentityService
    {
        Task<SocialUser> FindByMailId(string mailId);

        Task<SocialUser> FindByUserId(string userId);

        Task<bool> CheckCredentials(SocialUser user, string password);

        Task<SignInResult> SignIn(LoginModel user);

        Task<IdentityResult> Register(SocialUser user, string password);

        public Task<IdentityResult> ConfirmEmail(SocialUser user, string token);

        Task SignOut();

        public Task<SocialUser> GetUserByMailId(string userId);

        public Task<string> GenerateEmailConfirmationTokenAsync(SocialUser user);
    }
}
