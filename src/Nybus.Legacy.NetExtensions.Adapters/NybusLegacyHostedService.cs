using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Nybus
{
    public class NybusLegacyHostedService : IHostedService
    {
        private readonly IBus _bus;

        public NybusLegacyHostedService(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _bus.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _bus.Stop();
        }
    }
}