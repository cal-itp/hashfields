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

            var optionsService = host.Services.GetRequiredService<IOptions<DataOptions>>();

            Assert.IsNotNull(optionsService);
            Assert.IsNotNull(optionsService.Value);
            Assert.IsInstanceOfType(optionsService.Value, typeof(DataOptions));
        }

        [TestMethod]
        public void Program_Registers_StreamOptions()
        {
            var host = NewHost(emptyArgs);

            var optionsService = host.Services.GetRequiredService<IOptions<StreamOptions>>();

            Assert.IsNotNull(optionsService);
            Assert.IsNotNull(optionsService.Value);
            Assert.IsInstanceOfType(optionsService.Value, typeof(StreamOptions));
        }
    }
}
