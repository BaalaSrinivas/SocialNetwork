using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.RabbitMQ
{
    public class RabbitMQCore
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        public RabbitMQCore(ConnectionInfo connectionInfo)
        {
            _connectionFactory = new ConnectionFactory() { HostName = connectionInfo.HostName, 
                UserName = connectionInfo.UserName, 
                Password = connectionInfo.Password,
                Port =connectionInfo.Port
            };
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen == true;
            }
        }

        public bool Connect()
        {
            bool result = false;
            try
            {
                _connection = _connectionFactory.CreateConnection();
                result = _connection != null;
            }
            catch(Exception ex)
            {

            }
            return result;
        }

        public IModel CreateModel()
        {
            return _connection.CreateModel();
        }
    }
}
