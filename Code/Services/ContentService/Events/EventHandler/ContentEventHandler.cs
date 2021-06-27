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
    public class ContentEventHandler : IEventHandler<NewContentEventModel>
    {
        ILogger<ContentController> _logger;
        public ContentEventHandler(ILogger<ContentController> logger)
        {
            _logger = logger;
        }

        public void Handle(NewContentEventModel message)
        {
           _logger.LogInformation("New Message from Content Queue");
        }
    }
}
