using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentNRide.Common.Id;
using RentNRide.Data.Entities;
using RentNRide.Data.Entities.Motocycle;
using System.Text;
using System.Text.Json;

namespace RentNRide.Listener.MotorcycleConsumer;

public class MotorcycleConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ConnectionFactory _factory;

    public MotorcycleConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _factory = new ConnectionFactory()
        {
            HostName = configuration["RABBITMQ_HOST"],
            Port = int.Parse(configuration["RABBITMQ_PORT"]),
            UserName = configuration["RABBITMQ_USER"],
            Password = configuration["RABBITMQ_PASSWORD"]
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "motorcycle.created",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var evt = JsonSerializer.Deserialize<Motorcycle>(message);

            if (evt != null && evt.Year == 2024)
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var motorcycleEvent = new MotorcycleEvent
                {
                    MotorcycleId = evt.MotorcycleId,
                    MotorcycleEventId = await IdGenerator.NewSync(6),
                    CreatedDatetime = DateTime.UtcNow.AddHours(-3),
                };

                unitOfWork.MotorcycleEventRepository.Add(motorcycleEvent);
                await unitOfWork.SaveChangesAsync();
            }
        };

        channel.BasicConsume(queue: "motorcycle.created",
                             autoAck: true,
                             consumer: consumer);
    }
}