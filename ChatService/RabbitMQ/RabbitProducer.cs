using RabbitMQ.Client;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Channels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChatService.RabbitMQ
{
    public class RabbitProducer
    {
        private ConnectionFactory _factory { get; set; }

        private IConnection _connection { get; set; }

        private IModel _channel { get; set; }

        private QueueDeclareOk _queue { get; set; }

        public RabbitProducer(string queueName)
        {
            _factory = new ConnectionFactory { HostName = "localhost" };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _queue = _channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: ImmutableDictionary<string, object>.Empty);
        }

        public void Publish(string serillizeData, string? key = null)
        {
            _channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: key ?? _queue.QueueName,
                    body: Encoding.UTF8.GetBytes(serillizeData)
                );
        }
    }
}
