using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RentNRide.Common.Domain.Interfaces;
using System.Text;
using System.Text.Json;

namespace RentNRide.Providers.RabbitMqPublisher;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly ConnectionFactory _factory;

    public RabbitMqPublisher(IConfiguration configuration)
    {
        _factory = new ConnectionFactory()
        {
            HostName = configuration["RABBITMQ_HOST"],
            Port = int.Parse(configuration["RABBITMQ_PORT"]),
            UserName = configuration["RABBITMQ_USER"],
            Password = configuration["RABBITMQ_PASSWORD"]
        };
    }

    public Task PublishAsync<T>(string queueName, T message)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);

        return Task.CompletedTask;
    }
}
