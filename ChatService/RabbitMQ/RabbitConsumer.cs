using ChatService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Channels;
using UserService.Controllers;

namespace ChatService.RabbitMQ
{
    public class RabbitConsumer
    {
        private ConnectionFactory _factory { get; set; }

        private IConnection _connection { get; set; }

        private IModel _channel { get; set; }

        private QueueDeclareOk _queue { get; set; }

        public RabbitConsumer(string queueName)
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

        public void Consume(Func<object?, BasicDeliverEventArgs, Task> received)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) => {
                await received(sender, eventArgs);
                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(
                queue: _queue.QueueName,
                autoAck: false,
                consumer: consumer);

        }

        public static async Task GetMessage(object? sender, BasicDeliverEventArgs args)
        {
            try
            {
                var message = Encoding.UTF8.GetString(args.Body.ToArray());
                Console.WriteLine("[ChatService] RabbitMQ GetMessage: " + message);

                var model = JsonConvert.DeserializeObject<RabbitModel>(message);

                switch (model.Operation)
                {
                    case "send_message":
                        await MessageHandler.SendMessage(model.Data["msgData"]);
                        break;
                }
            }
            catch { }
        }
    }
}
