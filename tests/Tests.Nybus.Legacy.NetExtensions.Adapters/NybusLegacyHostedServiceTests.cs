using System.Threading.Tasks;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using Nybus;

namespace Tests
{
    [TestFixture]
    public class NybusLegacyHostedServiceTests
    {
        [Test, AutoMoqData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(NybusLegacyHostedService).GetConstructors());
        }

        [Test, AutoMoqData]
        public async Task StartAsync_starts_bus([Frozen] IBus bus, NybusLegacyHostedService sut)
        {
            await sut.StartAsync(default);

            Mock.Get(bus).Verify(p => p.Start());
        }

        [Test, AutoMoqData]
        public async Task StopAsync_stops_bus([Frozen] IBus bus, NybusLegacyHostedService sut)
        {
            await sut.StopAsync(default);

            Mock.Get(bus).Verify(p => p.Stop());
        }
    }
}