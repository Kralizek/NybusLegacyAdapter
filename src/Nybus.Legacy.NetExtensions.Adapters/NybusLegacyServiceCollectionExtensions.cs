using Microsoft.Extensions.DependencyInjection;
using Nybus.Configuration;
using Nybus.Container;
using Nybus.Logging;
using Nybus.Utils;

namespace Nybus
{
    public delegate void SubscriptionsConfigurator(IBusBuilder builder);

    public delegate void NybusConfigurator(INybusOptions options);

    public static class NybusLegacyServiceCollectionExtensions
    {
        public static IServiceCollection AddNybusLegacy(this IServiceCollection services, NybusConfigurator nybusConfigurator = null, SubscriptionsConfigurator subscriptionsConfigurator = null)
        {
            services.AddHostedService<NybusLegacyHostedService>();

            services.AddSingleton<IContainer, ServiceProviderContainer>();

            services.AddSingleton(Clock.Default);

            services.AddSingleton<ILoggerFactory, NybusLoggerFactoryAdapter>();

            services.AddSingleton<NybusOptions>();

            services.AddSingleton<IBusEngine, InMemoryBusEngine>();

            services.AddSingleton<IBusBuilder>(sp =>
            {
                var container = sp.GetRequiredService<IContainer>();

                var engine = sp.GetRequiredService<IBusEngine>();

                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

                var options = new NybusOptions
                {
                    Container = container,
                    LoggerFactory = loggerFactory
                };

                nybusConfigurator?.Invoke(options);

                var builder = new NybusBusBuilder(engine, options);

                subscriptionsConfigurator?.Invoke(builder);

                return builder;
            });

            services.AddSingleton(sp =>
            {
                var builder = sp.GetRequiredService<IBusBuilder>();

                return builder.Build();
            });

            return services;
        }

        public static void RegisterCommandHandler<TCommand, TCommandHandler>(this IServiceCollection services)
            where TCommandHandler : class, ICommandHandler<TCommand>
            where TCommand : class, ICommand
        {
            services.AddTransient<ICommandHandler<TCommand>, TCommandHandler>();
        }

        public static void RegisterEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
            where TEventHandler : class, IEventHandler<TEvent>
            where TEvent : class, IEvent
        {
            services.AddTransient<IEventHandler<TEvent>, TEventHandler>();
        }
    }
}