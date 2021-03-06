using MessageBus.MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Events.EventModel
{
    public class UserCommentedEventModel: BaseEventModel
    {
        public string OwnerUserId { get; set; }

        public string CommentedBy { get; set; }

        public string CommentText { get; set; }

        public string PostId { get; set; }
    }
}
