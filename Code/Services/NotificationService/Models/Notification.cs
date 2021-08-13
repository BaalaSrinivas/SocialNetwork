using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserId { get; set; }

        public bool IsRead { get; set; }

        public string Content { get; set; }

        public string UserProfileUrl { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
