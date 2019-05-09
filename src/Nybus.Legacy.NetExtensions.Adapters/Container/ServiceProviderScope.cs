using System;
using Microsoft.Extensions.DependencyInjection;

namespace Nybus.Container
{
    public class ServiceProviderScope : IScope
    {
        private readonly IServiceScope _scope;

        public ServiceProviderScope(IServiceScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public void Dispose()
        {
            _scope.Dispose();
        }

        public T Resolve<T>()
        {
            return _scope.ServiceProvider.GetRequiredService<T>();
        }

        public void Release<T>(T component) { }
    }
}