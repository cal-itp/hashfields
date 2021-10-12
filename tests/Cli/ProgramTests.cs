using System;

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

        [TestMethod]
        public void Program_Registers_DataOptions()
        {
            var host = NewHost(emptyArgs);

            var dataOptionsService = host.Services.GetRequiredService<IOptions<DataOptions>>();

            Assert.IsNotNull(dataOptionsService);
            Assert.IsNotNull(dataOptionsService.Value);
            Assert.IsInstanceOfType(dataOptionsService.Value, typeof(DataOptions));
        }
    }
}
