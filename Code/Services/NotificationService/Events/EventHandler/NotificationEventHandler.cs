using MessageBusCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NotificationService.Events.EventModel;
using NotificationService.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Events.EventHandler
{
    public class NotificationEventHandler : IEventHandler<NotificationEventModel>
    {
        ILogger<NotificationEventHandler> _logger;
        IHubContext<NotificationHub> _notificationHub;
        public NotificationEventHandler(IHubContext<NotificationHub> notificationHub,ILogger<NotificationEventHandler> logger)
        {
            _logger = logger;
            _notificationHub = notificationHub;
        }

        public async void Handle(NotificationEventModel message)
        {
            _logger.LogInformation($"To:{message.UserId}, Message:{message.MessageText}");
            await _notificationHub.Clients.Group(message.UserId).SendAsync("ReceiveMessage", message.MessageText);
        }
    }
}
