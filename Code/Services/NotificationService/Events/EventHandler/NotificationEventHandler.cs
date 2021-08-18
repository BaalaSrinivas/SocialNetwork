using MessageBusCore;
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

namespace NotificationService.Events.EventHandler
{
    public class NotificationEventHandler : IEventHandler<NotificationEventModel>
    {
        private ILogger<NotificationEventHandler> _logger;
        private IHubContext<NotificationHub> _notificationHub;
        private NotificationDbContext _notificationDbContext;

        public NotificationEventHandler(IHubContext<NotificationHub> notificationHub,
            ILogger<NotificationEventHandler> logger,
            NotificationDbContext notificationDbContext)
        {
            _logger = logger;
            _notificationHub = notificationHub;
            _notificationDbContext = notificationDbContext;
        }

        public async void Handle(NotificationEventModel message)
        {
            Notification notification = new Notification() {
                Content = message.MessageText,
                IsRead = false,
                UserId = message.UserId,
                UserProfileUrl = message.ProfileImageUrl,
                Timestamp = message.Timestamp,
                PostId = message.PostId,
                Type = message.Type
            };

            _notificationDbContext.Add(notification);
            _notificationDbContext.SaveChanges();

            _logger.LogInformation($"To:{message.UserId}, Message:{message.MessageText}");
            await _notificationHub.Clients.Group(message.UserId).SendAsync("ReceiveMessage", notification);
        }
    }
}
