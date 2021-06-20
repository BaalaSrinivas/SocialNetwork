using ContentService.Controllers;
using ContentService.Events.EventModel;
using MessageBusCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentService.Events.EventHandler
{
    public class ContentEventHandler : IEventHandler<ContentEventModel>
    {
        public void Handle(ContentEventModel message)
        {
           Console.WriteLine("New Message from Content Queue");
        }
    }
}
