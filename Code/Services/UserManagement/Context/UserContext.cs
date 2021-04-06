using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Context
{
    public class UserContext: DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {
                
        }

        public DbSet<User> Users { get; set; }
    }
}
