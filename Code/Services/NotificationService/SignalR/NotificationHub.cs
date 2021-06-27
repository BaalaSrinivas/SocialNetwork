using MessageBus.MessageBusCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.SignalR
{
    [Authorize]
    public class NotificationHub: Hub
    {
        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Claims.First(i => i.Type.Contains("mail")).Value);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Claims.First(i => i.Type.Contains("mail")).Value);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
