using MessageBus.MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Events.EventModel
{
    public class UserLikedEventModel: BaseEventModel
    {
        public string OwnerUserId { get; set; }

        public string LikedBy { get; set; }
    }
}
