using System;

using HashFields.Cli.Options;
using HashFields.Cli.Services;
using HashFields.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Cli.Tests
{
    [TestClass]
    public class ProgramTests
    {
        private static readonly string[] emptyArgs = Array.Empty<string>();

        private static IHost NewHost(string[] args) => Program.CreateHostBuilder(args).Build();

        [DataTestMethod]
        [DataRow(typeof(IOptions<DataOptions>))]
        [DataRow(typeof(IOptions<StreamOptions>))]
        [DataRow(typeof(OptionsInputStreamProvider))]
        [DataRow(typeof(OptionsOutputStreamProvider))]
        [DataRow(typeof(HashFieldsService))]
        [DataRow(typeof(IConsoleService))]
        [DataRow(typeof(IFileService))]
        [DataRow(typeof(IStreamProvider))]
        public void Registers_Service(Type serviceType)
        {
            var host = NewHost(emptyArgs);

            var service = host.Services.GetRequiredService(serviceType);

            Assert.IsNotNull(service);
        }

        [DataTestMethod]
        [DataRow(typeof(OptionsInputStreamProvider))]
        [DataRow(typeof(OptionsOutputStreamProvider))]
        [DataRow(typeof(HashFieldsService))]
        public void Registers_As_Singleton(Type serviceType)
        {
            var host = NewHost(emptyArgs);

            var service1 = host.Services.GetRequiredService(serviceType);
            var service2 = host.Services.GetRequiredService(serviceType);

            Assert.AreSame(service1, service2);
        }

        [DataTestMethod]
        [DataRow(typeof(IConsoleService))]
        [DataRow(typeof(IFileService))]
        [DataRow(typeof(IStreamProvider))]
        public void Registers_As_Transient(Type serviceType)
        {
            var host = NewHost(emptyArgs);

            var service1 = host.Services.GetRequiredService(serviceType);
            var service2 = host.Services.GetRequiredService(serviceType);

            Assert.AreNotSame(service1, service2);
        }
    }
}
