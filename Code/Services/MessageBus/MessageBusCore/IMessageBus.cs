using System;

namespace MessageBus.MessageBusCore
{
    public interface IMessageBus
    {
        void Publish(Message message);

        public void Subscribe(Action<Message> callbackMethod);

        public void UnSubscribe(Action<Message> callbackMethod);
    }
}
