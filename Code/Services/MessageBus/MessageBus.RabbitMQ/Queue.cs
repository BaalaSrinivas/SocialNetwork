using MessageBus.MessageBusCore;
using MessageBusCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBus.RabbitMQ
{
    public class Queue<T>: IQueue<T> where T : BaseEventModel
    {
        private RabbitMQCore _rabbitMQCore;
        private string _queueName;
        private IModel _channel;
        private IServiceProvider _serviceProvider;
        private bool _isSubscribed;

        IEventHandler<T> _subscriber;

        public Queue(RabbitMQCore rabbitMQCore, string queueName, IServiceProvider serviceProvider, bool hasConsumer = true)
        {
            _serviceProvider = serviceProvider;
            _rabbitMQCore = rabbitMQCore;
            _queueName = queueName;
            InitializeQueue(hasConsumer);
        }

        private void InitializeQueue(bool hasConsumer)
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
            _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);

            if (hasConsumer)
            {
                var consumer = new EventingBasicConsumer(_channel);
                
                consumer.Received += MessageEventHandler;
                _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
            }
        }

        private void MessageEventHandler(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            T message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(basicDeliverEventArgs.Body.Span));

            if (_isSubscribed)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    _subscriber = (IEventHandler<T>)scope.ServiceProvider.GetRequiredService(typeof(IEventHandler<T>));
                    _subscriber.Handle(message);
                }
            }
        }

        public void Publish(T message)
        {
            var messageString = JsonConvert.SerializeObject(message);

            _channel.BasicPublish(exchange: string.Empty, routingKey: _queueName, body: Encoding.UTF8.GetBytes(messageString));
        }

        public IQueue<T> AddSubscriber<EH>() where EH: IEventHandler<T>
        {
            _isSubscribed = true;
            return this;
        }
    }
}
