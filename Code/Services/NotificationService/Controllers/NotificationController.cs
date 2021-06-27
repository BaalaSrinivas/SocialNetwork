using MessageBus.MessageBusCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationService.Events.EventModel;
using NotificationService.Models;
using NotificationService.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("notificationapi/v1/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        IHubContext<NotificationHub> _notificationHub;

        public NotificationController(ILogger<NotificationController> logger, IHubContext<NotificationHub> notificationHub)
        {
            _logger = logger;
            _notificationHub = notificationHub;
        }

        [HttpGet]
        public async Task<IEnumerable<Notification>> GetUndeliveredNotifications()
        {
            return new List<Notification>() { new Notification() { } };
        }
    }
}
