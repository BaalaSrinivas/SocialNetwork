using MessageBus.MessageBusCore;
using MessageBusCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Events.EventModel;
using UserManagement.Models;
using UserManagement.Repository;

namespace UserManagement.Events.EventHandler
{
    public class UserLikedEventHandler : IEventHandler<UserLikedEventModel>
    {
        ILogger<UserLikedEventHandler> _logger;
        private ISMUserRepository _smUserRepository;

        private IQueue<NotificationEventModel> _notificationQueue;

        public UserLikedEventHandler(ISMUserRepository smUserRepository, IQueue<NotificationEventModel> notificationQueue, ILogger<UserLikedEventHandler> logger)
        {
            _smUserRepository = smUserRepository;
            _notificationQueue = notificationQueue;
            _logger = logger;
        }

        public void Handle(UserLikedEventModel message)
        {
            SMUser user = _smUserRepository.GetUser(message.LikedBy);

            NotificationEventModel notificationEventModel = new NotificationEventModel()
            {
                UserId = message.OwnerUserId,
                MessageText = message.MessageText.Replace("<UserName>", user.Name),
                ProfileImageUrl = user.ProfileImageUrl
            };
            _logger.LogInformation(notificationEventModel.MessageText);
            _notificationQueue.Publish(notificationEventModel);
        }
    }
}
