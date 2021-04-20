using System;

namespace MessageBus.MessageBusCore
{
    public interface IMessageBus<T>
    {
        void Publish(string queueName, Message message);

        public void Consume(string queueName, EventHandler<T> callbackMethod);
    }
}
