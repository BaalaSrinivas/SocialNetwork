using MessageBus.MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Events.EventModel
{
    public class NotificationEventModel : BaseEventModel
    {
        public string UserId { get; set; }

        public string ProfileImageUrl { get; set;}
    }
}
