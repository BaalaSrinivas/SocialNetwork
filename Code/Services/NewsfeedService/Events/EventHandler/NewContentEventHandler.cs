using MessageBus.MessageBusCore;
using MessageBusCore;
using NewsfeedService.Events.EventModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsfeedService.Events.EventHandler
{
    public class NewContentEventHandler : IEventHandler<NewContentEventModel>
    {
        public void Handle(NewContentEventModel message)
        {
            throw new NotImplementedException();
        }
    }
}
