using MessageBus.MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Events.EventModel
{
    public class UserAddedEventModel: BaseEventModel
    {
        public string UserId { get; set; }
    }
}
