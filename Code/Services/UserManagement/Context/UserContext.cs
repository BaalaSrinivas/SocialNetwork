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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SMUser>().HasKey(s => s.MailId);
        }
        public DbSet<SMUser> SMUsers { get; set; }
    }
}
