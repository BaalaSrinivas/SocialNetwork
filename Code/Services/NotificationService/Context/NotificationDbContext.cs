using Microsoft.EntityFrameworkCore;
using NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Context
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Notification> Notifications { get; set; }
    }
}
