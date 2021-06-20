using MessageBus.MessageBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBusCore
{
    public interface IEventHandler<T> where T : BaseEventModel
    {
        void Handle(T message);
    }
}
