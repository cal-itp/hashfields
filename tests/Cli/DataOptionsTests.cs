using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Cli.Tests
{
    [TestClass]
    public class DataOptionsTests
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
                ["DataOptions:Skip:2"] = "3"
            };

            _configuration = new ConfigurationBuilder().AddInMemoryCollection(config).Build();
        }

        private DataOptions GetDataOptions(IConfigurationRoot config = null) =>
            (config ?? _configuration).GetSection(DataOptions.ConfigurationSectionName).Get<DataOptions>();

        [TestMethod]
        public void Delimiter()
        {
            var dataOptions = GetDataOptions();

            Assert.AreEqual("delim", dataOptions.Delimiter);
        }

        [TestMethod]
        public void Default_Delimiter()
        {
            var config = new ConfigurationBuilder()
                // don't provide a Delimiter key-value
                .AddInMemoryCollection(new[] { KeyValuePair.Create("DataOptions:HashAlgorithm", "algo") })
                .Build();

            var dataOptions = config.GetSection("DataOptions").Get<DataOptions>();

            Assert.IsNotNull(dataOptions.Delimiter);
        }

        [TestMethod]
        public void Drop()
        {
            var dataOptions = GetDataOptions();

            Assert.IsTrue(new[] { "a", "b" }.SequenceEqual(dataOptions.Drop));
        }

        [TestMethod]
        public void HashAlgorithm()
        {
            var dataOptions = GetDataOptions();

            Assert.AreEqual("algo", dataOptions.HashAlgorithm);
        }

        [TestMethod]
        public void Default_HashAlgorithm()
        {
            var config = new ConfigurationBuilder()
                // don't provide a HashAlgorithm key-value
                .AddInMemoryCollection(new[] { KeyValuePair.Create("DataOptions:Delimiter", "delim") })
                .Build();

            var dataOptions = config.GetSection("DataOptions").Get<DataOptions>();

            Assert.IsNotNull(dataOptions.HashAlgorithm);
        }

        [TestMethod]
        public void Skip()
        {
            var dataOptions = GetDataOptions();

            Assert.IsTrue(new[] { "1", "2", "3" }.SequenceEqual(dataOptions.Skip));
        }
    }
}
