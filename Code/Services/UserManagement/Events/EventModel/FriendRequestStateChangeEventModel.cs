using MessageBus.MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Events.EventModel
{
    public class FriendRequestStateChangeEventModel: BaseEventModel
    {
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string State { get; set; }
    }
}
