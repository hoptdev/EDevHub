using ChatService.RabbitMQ;

namespace ChatService;

public class Program
{
    public static void Main(string[] args)
    {
        var rabbitConsumer = new RabbitConsumer("websocket_messages");
        rabbitConsumer.Consume(RabbitConsumer.GetMessage);

        string hostname = "http://0.0.0.0.0";

        Builder.Run(new string[1] { $"--urls={hostname}:8443/" }, true);
    }
}