using MessageBus.MessageBusCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.RabbitMQ
{
    public class RabbitMQBus : IMessageBus 
    {
        private RabbitMQCore _rabbitMQCore;
        string _queueName;

        private delegate void Subscriber(Message message);

        private Subscriber _subscribers;

        public RabbitMQBus(RabbitMQCore rabbitMQCore, string queueName)
        {
            _rabbitMQCore = rabbitMQCore;
            _queueName = queueName;
            InitilalizeConsumer();
        }

        public void Subscribe(Action<Message> callbackMethod) 
        {
            _subscribers += new Subscriber(callbackMethod);
        }

        public void UnSubscribe(Action<Message> callbackMethod)
        {
            _subscribers -= new Subscriber(callbackMethod);
        }

        public void InitilalizeConsumer()
        {
            if (!_rabbitMQCore.IsConnected)
            {
                bool connection = _rabbitMQCore.Connect();
                if (!connection)
                {
                    return;
                }
            }
            IModel channel = _rabbitMQCore.CreateModel();
            channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += MessageEventHandler;

            channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void Publish(Message message)
        {
            if(!_rabbitMQCore.IsConnected)
            {
                bool connection = _rabbitMQCore.Connect();
                if(!connection)
                {
                    return;
                }
            }
            IModel channel = _rabbitMQCore.CreateModel();

            channel.QueueDeclare(_queueName, durable:false, exclusive: false, autoDelete: false);

            var messageString = JsonConvert.SerializeObject(message);

            channel.BasicPublish(exchange: string.Empty, routingKey: _queueName, body: Encoding.UTF8.GetBytes(messageString));
        }

        private void MessageEventHandler(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            Message message = JsonConvert.DeserializeObject<Message>(Encoding.UTF8.GetString(basicDeliverEventArgs.Body.Span));
            if (_subscribers != null)
            {
                _subscribers(message);
            }
        }

    }
}
