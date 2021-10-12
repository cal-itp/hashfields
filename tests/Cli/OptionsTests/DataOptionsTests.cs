using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Cli.Options.Tests
{
    [TestClass]
    public class DataOptionsTests
    {
        private DataOptions _options;

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

            _options = new ConfigurationBuilder()
                        .AddInMemoryCollection(config)
                        .Build()
                        .GetSection(DataOptions.ConfigurationSectionName)
                        .Get<DataOptions>();
        }

        [TestMethod]
        public void Delimiter()
        {
            Assert.AreEqual("delim", _options.Delimiter);
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
            Assert.IsTrue(new[] { "a", "b" }.SequenceEqual(_options.Drop));
        }

        [TestMethod]
        public void HashAlgorithm()
        {
            Assert.AreEqual("algo", _options.HashAlgorithm);
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
            Assert.IsTrue(new[] { "1", "2", "3" }.SequenceEqual(_options.Skip));
        }
    }
}
