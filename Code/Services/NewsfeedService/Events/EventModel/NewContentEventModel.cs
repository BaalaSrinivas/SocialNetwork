using MessageBus.MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsfeedService.Events.EventModel
{
    public class NewContentEventModel: BaseEventModel
    {
        public Guid PostId { get; set; }

        public string PostUserId { get; set; }
    }
}
