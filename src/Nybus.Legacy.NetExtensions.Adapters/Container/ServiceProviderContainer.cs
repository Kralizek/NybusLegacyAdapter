using System;
using Microsoft.Extensions.DependencyInjection;

namespace Nybus.Container
{
    public class ServiceProviderContainer : IContainer
    {
        private readonly IServiceProvider _services;

        public ServiceProviderContainer(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IScope BeginScope()
        {
            var scope = _services.CreateScope();
            return new ServiceProviderScope(scope);
        }
    }
}