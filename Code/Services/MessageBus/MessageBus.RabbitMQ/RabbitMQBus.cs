using MessageBus.MessageBusCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MessageBus.RabbitMQ
{
    public class RabbitMQBus : IMessageBus<BasicDeliverEventArgs>
    {
        private RabbitMQCore _rabbitMQCore;
        public RabbitMQBus(RabbitMQCore rabbitMQCore)
        {
            _rabbitMQCore = rabbitMQCore;
        }
        public void Consume(string queueName, EventHandler<BasicDeliverEventArgs> callbackMethod)
        {
            if (!_rabbitMQCore.IsConnected)
            {
                bool connection = _rabbitMQCore.Connect();
                if (!connection)
                {
                    return;
                }
            }
            IModel model = _rabbitMQCore.CreateModel();
            model.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(model);
            consumer.Received += callbackMethod;

            model.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void Publish(string queueName, Message message)
        {
            if(!_rabbitMQCore.IsConnected)
            {
                bool connection = _rabbitMQCore.Connect();
                if(!connection)
                {
                    return;
                }
            }
            IModel model = _rabbitMQCore.CreateModel();

            model.QueueDeclare(queueName, durable:false, exclusive: false, autoDelete: false);

            var messageString = JsonConvert.SerializeObject(message);

            model.BasicPublish(exchange: string.Empty, routingKey: queueName, body: Encoding.UTF8.GetBytes(messageString));
        }

    }
}
