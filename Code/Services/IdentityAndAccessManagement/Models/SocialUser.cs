using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement.Models
{
    public class SocialUser: IdentityUser
    {
        public string Name { get; set; }
    }
}
