using MessageBusCore;
using System;

namespace MessageBus.MessageBusCore
{
    public interface IQueue<T> where T : BaseEventModel
    {
        void Publish(T message);

        public IQueue<T> AddSubscriber<EH>() where EH : IEventHandler<T>, new();
    }
}
