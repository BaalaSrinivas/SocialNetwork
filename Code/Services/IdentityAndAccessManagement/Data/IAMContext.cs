using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement.Data
{
    public class IAMContext : IdentityDbContext
    {
        public IAMContext(DbContextOptions<IAMContext> options) : base(options)
        {
        }
    }
}
