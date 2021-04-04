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
        Task<IdentityUser> FindByMailId(string mailId);

        Task<bool> CheckCredentials(IdentityUser user, string password);

        Task SignIn(IdentityUser user, AuthenticationProperties properties);

        Task<IdentityResult> Register(IdentityUser user, string password);

        Task SignOut();
    }
}
