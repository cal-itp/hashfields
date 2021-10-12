using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Cli.Tests
{
    [TestClass]
    public class ConfigurationTests
    {
        private IConfigurationRoot _configuration;

        [TestInitialize]
        public void Init()
        {
            var config = new Dictionary<string, string>()
            {
                ["DataOptions:Delimiter"] = "delim",
                ["DataOptions:Drop:0"] = "a",
                ["DataOptions:Drop:1"] = "b",
                ["DataOptions:HashAlgorithm"] = "algo",
                ["DataOptions:Skip:0"] = "1",
                ["DataOptions:Skip:1"] = "2",
                ["DataOptions:Skip:2"] = "3",
                ["StreamOptions:Input:Channel"] = "input:channel",
                ["StreamOptions:Input:Type"] = "input:type",
                ["StreamOptions:Output:Channel"] = "output:channel",
                ["StreamOptions:Output:Type"] = "output:type",
            };

            _configuration = new ConfigurationBuilder().AddInMemoryCollection(config).Build();
        }

        private DataOptions GetDataOptions() =>
            _configuration.GetSection(DataOptions.ConfigurationSectionName).Get<DataOptions>();

        private StreamOptions GetStreamOptions() =>
            _configuration.GetSection(StreamOptions.ConfigurationSectionName).Get<StreamOptions>();

        [TestMethod]
        public void DataOptions_Delimiter()
        {
            var dataOptions = GetDataOptions();

            Assert.AreEqual("delim", dataOptions.Delimiter);
        }

        [TestMethod]
        public void DataOptions_Default_Delimiter()
        {
            var config = new ConfigurationBuilder()
                // don't provide a Delimiter key-value
                .AddInMemoryCollection(new[] { KeyValuePair.Create("DataOptions:HashAlgorithm", "algo") })
                .Build();

            var dataOptions = config.GetSection("DataOptions").Get<DataOptions>();

            Assert.IsNotNull(dataOptions.Delimiter);
        }

        [TestMethod]
        public void DataOptions_Drop()
        {
            var dataOptions = GetDataOptions();

            Assert.IsTrue(new[] { "a", "b" }.SequenceEqual(dataOptions.Drop));
        }

        [TestMethod]
        public void DataOptions_HashAlgorithm()
        {
            var dataOptions = GetDataOptions();

            Assert.AreEqual("algo", dataOptions.HashAlgorithm);
        }

        [TestMethod]
        public void DataOptions_Default_HashAlgorithm()
        {
            var config = new ConfigurationBuilder()
                // don't provide a HashAlgorithm key-value
                .AddInMemoryCollection(new[] { KeyValuePair.Create("DataOptions:Delimiter", "delim") })
                .Build();

            var dataOptions = config.GetSection("DataOptions").Get<DataOptions>();

            Assert.IsNotNull(dataOptions.HashAlgorithm);
        }

        [TestMethod]
        public void DataOptions_Skip()
        {
            var dataOptions = GetDataOptions();

            Assert.IsTrue(new[] { "1", "2", "3" }.SequenceEqual(dataOptions.Skip));
        }

        [TestMethod]
        public void StreamOptions_Input_Source()
        {
            var streamOptions = GetStreamOptions();

            Assert.AreEqual("input:channel", streamOptions.Input.Channel);
        }

        [TestMethod]
        public void StreamOptions_Input_Type()
        {
            var streamOptions = GetStreamOptions();

            Assert.AreEqual("input:type", streamOptions.Input.Type);
        }

        [TestMethod]
        public void StreamOptions_Output_Source()
        {
            var streamOptions = GetStreamOptions();

            Assert.AreEqual("output:channel", streamOptions.Output.Channel);
        }

        [TestMethod]
        public void StreamOptions_Output_Type()
        {
            var streamOptions = GetStreamOptions();

            Assert.AreEqual("output:type", streamOptions.Output.Type);
        }
    }
}
