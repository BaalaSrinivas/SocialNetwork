using MessageBus.MessageBusCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationService.Events.EventModel;
using NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("notificationapi/v1/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private IQueue<ContentEventModel> _contentQueue;

        public NotificationController(ILogger<NotificationController> logger, IQueue<ContentEventModel> contentQueue)
        {
            _logger = logger;
            _contentQueue = contentQueue;
        }

        [HttpGet]
        public IEnumerable<Notification> GetUndeliveredNotifications()
        {
            _contentQueue.Publish(new ContentEventModel() { MessageText = $"Get called at {DateTime.Now}", PostId = Guid.NewGuid() });
            return new List<Notification>() { new Notification() { } };
        }
    }
}
