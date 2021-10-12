using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Cli.Tests
{
    [TestClass]
    public class StreamOptionsTests
    {
        private IConfigurationRoot _configuration;

        [TestInitialize]
        public void Init()
        {
            var config = new Dictionary<string, string>()
            {
                ["StreamOptions:Input:Channel"] = "input:channel",
                ["StreamOptions:Input:Type"] = "input:type",
                ["StreamOptions:Output:Channel"] = "output:channel",
                ["StreamOptions:Output:Type"] = "output:type",
            };

            _configuration = new ConfigurationBuilder().AddInMemoryCollection(config).Build();
        }

        private StreamOptions GetStreamOptions(IConfigurationRoot config = null) =>
            (config ?? _configuration).GetSection(StreamOptions.ConfigurationSectionName).Get<StreamOptions>();

        [TestMethod]
        public void Input_Channel()
        {
            var streamOptions = GetStreamOptions();

            Assert.AreEqual("input:channel", streamOptions.Input.Channel);
        }

        [TestMethod]
        public void Input_Type()
        {
            var streamOptions = GetStreamOptions();

            Assert.AreEqual("input:type", streamOptions.Input.Type);
        }

        [TestMethod]
        public void Output_Channel()
        {
            var streamOptions = GetStreamOptions();

            Assert.AreEqual("output:channel", streamOptions.Output.Channel);
        }

        [TestMethod]
        public void Output_Type()
        {
            var streamOptions = GetStreamOptions();

            Assert.AreEqual("output:type", streamOptions.Output.Type);
        }
    }
}
