using MessageBus.MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowService.Events.EventModel
{
    public class NewUserFollowEventModel: BaseEventModel
    {
        public string FollowerUserId { get; set; }

        public string FollowingUserId { get; set; }
    }
}
