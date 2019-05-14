using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nybus.MassTransit;

namespace Nybus
{
    public static class NybusLegacyMassTransitServiceCollectionExtensions
    {
        public static IServiceCollection AddNybusLegacyWithMassTransit(this IServiceCollection services, NybusLegacyConfiguration configuration)
        {
            services.AddNybusLegacy(configuration.NybusConfigurator, configuration.SubscriptionsConfigurator);

            services.AddSingleton(sp =>
            {
                var options = new MassTransitOptions();

                configuration.MassTransitConfigurator?.Invoke(options);

                return options;
            });

            services.AddSingleton(sp =>
            {
                var cfg = sp.GetRequiredService<IConfiguration>();
                var connectionString = cfg.GetConnectionString(configuration.ConnectionStringName);

                return MassTransitConnectionDescriptor.Parse(connectionString);
            });

            services.AddSingleton<IBusEngine>(sp =>
            {
                var descriptor = sp.GetRequiredService<MassTransitConnectionDescriptor>();

                var options = sp.GetRequiredService<MassTransitOptions>();

                return new MassTransitBusEngine(descriptor, options);
            });

            return services;
        }
    }

    public delegate void MassTransitConfigurator(MassTransitOptions options);

    public class NybusLegacyConfiguration
    {
        public string ConnectionStringName { get; set; } = "ServiceBus";

        public SubscriptionsConfigurator SubscriptionsConfigurator { get; set; } = delegate { };

        public NybusConfigurator NybusConfigurator { get; set; } = delegate { };

        public MassTransitConfigurator MassTransitConfigurator { get; set; } = delegate { };
    }
}
