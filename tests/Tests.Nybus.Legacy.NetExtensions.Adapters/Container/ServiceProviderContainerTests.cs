using System;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Nybus.Container;

namespace Tests.Container
{
    [TestFixture]
    public class ServiceProviderContainerTests
    {
        [Test, AutoMoqData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ServiceProviderContainer).GetConstructors());
        }

        [Test, AutoMoqData]
        public void BeginScope_forwards_call_to_ServiceProvider([Frozen] IServiceProvider serviceProvider, ServiceProviderContainer sut, IServiceScopeFactory scopeFactory)
        {
            Mock.Get(serviceProvider).Setup(p => p.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactory);

            var scope = sut.BeginScope();

            Assert.That(scope, Is.InstanceOf<ServiceProviderScope>());

            Mock.Get(serviceProvider).Verify(p => p.GetService(typeof(IServiceScopeFactory)));
        }
    }
}