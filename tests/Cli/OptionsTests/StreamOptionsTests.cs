using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Cli.Options.Tests
{
    [TestClass]
    public class StreamOptionsTests
    {
        private StreamOptions _options;

        [TestInitialize]
        public void Init()
        {
            var config = new Dictionary<string, string>()
            {
                ["StreamOptions:Input:Path"] = "Input:Path",
                ["StreamOptions:Input:Type"] = "Input:Type",
                ["StreamOptions:Output:Path"] = "Output:Path",
                ["StreamOptions:Output:Type"] = "Output:Type",
            };

            _options = new ConfigurationBuilder()
                        .AddInMemoryCollection(config)
                        .Build()
                        .GetSection(StreamOptions.ConfigurationSectionName)
                        .Get<StreamOptions>();
        }

        [TestMethod]
        public void Input_Path()
        {
            Assert.AreEqual("Input:Path", _options.Input.Path);
        }

        [TestMethod]
        public void Input_Type()
        {
            Assert.AreEqual("Input:Type", _options.Input.Type);
        }

        [TestMethod]
        public void Output_Path()
        {
            Assert.AreEqual("Output:Path", _options.Output.Path);
        }

        [TestMethod]
        public void Output_Type()
        {
            Assert.AreEqual("Output:Type", _options.Output.Type);
        }
    }
}
