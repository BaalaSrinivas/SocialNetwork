using MessageBus.MessageBusCore;
using MessageBusCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.RabbitMQ
{
    public class Queue<T>: IQueue<T> where T : BaseEventModel
    {
        private RabbitMQCore _rabbitMQCore;
        private string _queueName;
        private IModel _channel;

        IEventHandler<T> _subscriber;

        public Queue(RabbitMQCore rabbitMQCore, string queueName)
        {
            _rabbitMQCore = rabbitMQCore;
            _queueName = queueName;
            InitializeQueue();
        }

        private void InitializeQueue()
        {
            if (!_rabbitMQCore.IsConnected)
            {
                bool connection = _rabbitMQCore.Connect();
                if (!connection)
                {
                    return;
                }
            }

            _channel = _rabbitMQCore.CreateModel();
            _channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += MessageEventHandler;

            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private void MessageEventHandler(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            T message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(basicDeliverEventArgs.Body.Span));
            _subscriber.Handle(message);
        }

        public void Publish(T message)
        {
            var messageString = JsonConvert.SerializeObject(message);

            _channel.BasicPublish(exchange: string.Empty, routingKey: _queueName, body: Encoding.UTF8.GetBytes(messageString));
        }

        public IQueue<T> AddSubscriber<EH>() where EH: IEventHandler<T>, new()
        {
            _subscriber = new EH();
            return this;
        }
    }
}
