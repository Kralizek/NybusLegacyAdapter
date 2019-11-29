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

namespace Producer
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
                    builder.SubscribeToEvent<ItemProduced>(async e =>
                    {
                        await Console.Out.WriteLineAsync($"Received: {e.Message.Quantity} of {e.Message.ItemId} were produced!");
                    });
                },
                MassTransitConfigurator = options =>
                {
                    options.EventQueueStrategy = new TemporaryQueueStrategy();

                    options.ServiceBusFactory = new RabbitMqServiceBusFactory(Environment.ProcessorCount);
                }
            });

            var serviceProvider = services.BuildServiceProvider();

            var hostedService = serviceProvider.GetRequiredService<IHostedService>();

            await hostedService.StartAsync(default);

            var bus = serviceProvider.GetRequiredService<IBus>();

            var correlationId = Guid.NewGuid();

            for (var i = 0; i < 100; i++)
            {
                await bus.InvokeCommand(new ProduceItem { ItemId = Guid.NewGuid(), Quantity = 1f }, correlationId);
            }

            await hostedService.StopAsync(default);
        }
    }
}
