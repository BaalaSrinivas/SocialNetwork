using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.MessageBusCore
{
    public class Message
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string MessageText { get; set; }
    }
}
