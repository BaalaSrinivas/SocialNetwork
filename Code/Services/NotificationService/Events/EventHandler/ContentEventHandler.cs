using MessageBusCore;
using NotificationService.Events.EventModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Events.EventHandler
{
    public class ContentEventHandler : IEventHandler<ContentEventModel>
    {
        public void Handle(ContentEventModel message)
        {
            Console.WriteLine("New Message from Content Queue");
        }
    }
}
