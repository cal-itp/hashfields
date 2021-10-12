using System;
using System.Collections.Generic;
using System.IO;

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

            _configuration = GetInMemoryConfigRoot(config);
        }

        private static IConfigurationRoot GetInMemoryConfigRoot(IEnumerable<KeyValuePair<string, string>> config) =>
            new ConfigurationBuilder().AddInMemoryCollection(config).Build();

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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InputStream_InvalidType_Throws()
        {
            var streamOptions = GetStreamOptions();

            _ = streamOptions.InputStream();
        }

        [TestMethod]
        public void InputStream_File()
        {
            var tempFile = Path.GetTempFileName();

            var configData = new Dictionary<string, string>()
            {
                ["StreamOptions:Input:Channel"] = tempFile,
                ["StreamOptions:Input:Type"] = "File",
            };

            var config = GetInMemoryConfigRoot(configData);

            var streamOptions = GetStreamOptions(config);
            using var stream = streamOptions.InputStream();

            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.CanRead);

            File.Delete(tempFile);
        }

        [TestMethod]
        public void InputStream_StdIn()
        {
            var configData = new Dictionary<string, string>()
            {
                ["StreamOptions:Input:Type"] = "StdIn",
            };

            var config = GetInMemoryConfigRoot(configData);

            var streamOptions = GetStreamOptions(config);
            using var stream = streamOptions.InputStream();

            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.CanRead);
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void OutputStream_InvalidType_Throws()
        {
            var streamOptions = GetStreamOptions();

            _ = streamOptions.OutputStream();
        }

        [TestMethod]
        public void OutputStream_File()
        {
            var tempFile = Path.GetTempFileName();

            var configData = new Dictionary<string, string>()
            {
                ["StreamOptions:Output:Channel"] = tempFile,
                ["StreamOptions:Output:Type"] = "File",
            };

            var config = GetInMemoryConfigRoot(configData);

            var streamOptions = GetStreamOptions(config);
            using var stream = streamOptions.OutputStream();

            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.CanWrite);

            File.Delete(tempFile);
        }

        [TestMethod]
        public void OutputStream_StdOut()
        {
            var configData = new Dictionary<string, string>()
            {
                ["StreamOptions:Output:Type"] = "StdOut",
            };

            var config = GetInMemoryConfigRoot(configData);

            var streamOptions = GetStreamOptions(config);
            using var stream = streamOptions.OutputStream();

            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.CanWrite);
        }
    }
}
