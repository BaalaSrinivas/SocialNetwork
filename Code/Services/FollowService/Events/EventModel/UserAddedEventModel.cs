using MessageBus.MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Events.EventModel
{
    public class UserAddedEventModel: BaseEventModel
    {
        public string UserId { get; set; }
    }
}
