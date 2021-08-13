using MessageBus.MessageBusCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationService.Context;
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
        NotificationDbContext _notificationDbContext;

        public NotificationController(ILogger<NotificationController> logger, 
            IHubContext<NotificationHub> notificationHub,
            NotificationDbContext notificationDbContext)
        {
            _logger = logger;
            _notificationHub = notificationHub;
            _notificationDbContext = notificationDbContext;
        }

        [HttpGet]
        [Route("getunreadnotifications")]
        public IEnumerable<Notification> GetUnreadNotifications()
        {
            return _notificationDbContext.Notifications.Where(n => n.IsRead == false).OrderByDescending(s => s.Timestamp).Take(10);
        }

        [HttpPost]
        [Route("updatenotificationreadstatus")]
        public bool UpdateNotificationReadStatus(Notification notification)
        {
            Notification notificationObject = _notificationDbContext.Notifications.FirstOrDefault(n => n.Id == notification.Id);
            notificationObject.IsRead = true;
            _notificationDbContext.Update(notificationObject);
            return _notificationDbContext.SaveChanges() > 0;
        }
    }
}
