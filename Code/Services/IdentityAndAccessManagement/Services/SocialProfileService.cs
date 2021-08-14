using IdentityAndAccessManagement.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement.Services
{
    /// <summary>
    /// Fetches additonal profile information like Name for the users to UI
    /// </summary>
    public class SocialProfileService : IProfileService
    {
        UserManager<SocialUser> _userManager;

        public SocialProfileService(UserManager<SocialUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var id = sub.Claims.FirstOrDefault(s => s.Type == "sub").Value;

            var socialUser = await _userManager.FindByIdAsync(id);

            socialUser = socialUser ?? throw new Exception("Unable to find user");

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("name", socialUser.Name));
            claims.Add(new Claim("sub", socialUser.Id));
            claims.Add(new Claim("email", socialUser.Email));
            claims.Add(new Claim("preferred_username", socialUser.Name));

            context.IssuedClaims.AddRange(claims);
        }

        public async  Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var id = sub.Claims.FirstOrDefault(s => s.Type == "sub").Value;

            var socialUser = await _userManager.FindByIdAsync(id);

            context.IsActive = socialUser != null;
        }
    }
}
