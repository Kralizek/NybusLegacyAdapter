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
    public class ServiceProviderScopeTests
    {
        [Test, AutoMoqData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ServiceProviderScope).GetConstructors());
        }

        [Test, AutoMoqData]
        public void Dispose_is_forwarded_to_inner_factory([Frozen] IServiceScope scope, ServiceProviderScope sut)
        {
            sut.Dispose();

            Mock.Get(scope).Verify(p => p.Dispose());
        }

        [Test, AutoMoqData]
        public void Resolve_forwards_to_inner_scope([Frozen] IServiceScope scope, ServiceProviderScope sut, TestClass testObject)
        {
            Mock.Get(scope.ServiceProvider).Setup(p => p.GetService(typeof(TestClass))).Returns(testObject);

            var result = sut.Resolve<TestClass>();

            Mock.Get(scope.ServiceProvider).Verify(p => p.GetService(typeof(TestClass)));
        }

        [Test, AutoMoqData]
        public void Exceptions_are_not_handled([Frozen] IServiceScope scope, ServiceProviderScope sut, Exception exception)
        {
            Mock.Get(scope.ServiceProvider).Setup(p => p.GetService(typeof(TestClass))).Throws(exception);

            Assert.Throws( Is.SameAs(exception), () => sut.Resolve<TestClass>());
        }

        [Test, AutoMoqData]
        public void Release_isnt_used([Frozen] IServiceScope scope, ServiceProviderScope sut, TestClass testObject)
        {
            sut.Release(testObject);

            Mock.Get(scope).VerifyNoOtherCalls();
        }
    }
}