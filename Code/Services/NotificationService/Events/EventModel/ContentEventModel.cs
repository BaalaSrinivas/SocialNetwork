using MessageBus.MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Events.EventModel
{
    public class ContentEventModel: BaseEventModel
    {
        public Guid PostId;
    }
}
