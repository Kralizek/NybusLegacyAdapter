using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nybus;
using Nybus.Configuration;

namespace Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:ServiceBus"] = "host=rabbitmq://localhost;username=guest;password=guest"
            });

            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddLogging(logging => logging.AddConsole().SetMinimumLevel(LogLevel.Trace));

            services.AddNybusLegacyWithMassTransit(new NybusLegacyConfiguration
            {
                ConnectionStringName = "ServiceBus",
                SubscriptionsConfigurator = builder =>
                {
                    builder.SubscribeToCommand<ProduceItem>();
                },
                MassTransitConfigurator = options =>
                {
                    options.CommandQueueStrategy = new PrefixedTemporaryQueueStrategy("Queue");

                    options.ServiceBusFactory = new RabbitMqServiceBusFactory(Environment.ProcessorCount);
                }
            });

            services.RegisterCommandHandler<ProduceItem, ProduceItemCommandHandler>();

            var serviceProvider = services.BuildServiceProvider();

            var hostedService = serviceProvider.GetRequiredService<IHostedService>();

            await hostedService.StartAsync(default);

            Console.WriteLine("Press ENTER to close");
            Console.ReadLine();

            await hostedService.StopAsync(default);
        }
    }

    public class ProduceItemCommandHandler : ICommandHandler<ProduceItem>
    {
        private readonly IBus _bus;
        private readonly ILogger<ProduceItemCommandHandler> _logger;

        public ProduceItemCommandHandler(IBus bus, ILogger<ProduceItemCommandHandler> logger)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(CommandContext<ProduceItem> commandMessage)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10));

            await _bus.RaiseEvent(new ItemProduced
            {
                ItemId = commandMessage.Message.ItemId,
                Quantity = commandMessage.Message.Quantity
            });
        }
    }
}
