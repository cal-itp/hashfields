using System;
using System.IO;

using HashFields.Cli.Services;
using HashFields.Cli.Tests.Fakes;

using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Cli.Options.Tests
{
    [TestClass]
    public class OptionsStreamProvidersTests
    {
        private IConsoleService _console;

        private IFileService _file;

        [TestInitialize]
        public void Init()
        {
            _console = new FakeConsoleService();
            _file = new FakeFileService();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InputStream_InvalidType_Throws()
        {
            var options = new StreamOptions()
            {
                Input = new StreamOptions.Channel() { Type = "invalid type" }
            };

            var wrapper = new OptionsWrapper<StreamOptions>(options);

            var provider = new OptionsInputStreamProvider(_console, _file, wrapper);

            provider.Get();
        }

        [TestMethod]
        public void InputStream_File_CanRead()
        {
            var tempFile = Path.GetTempFileName();

            var options = new StreamOptions()
            {
                Input = new StreamOptions.Channel()
                {
                    Type = nameof(StreamOptions.Types.File),
                    Path = tempFile
                }
            };

            var wrapper = new OptionsWrapper<StreamOptions>(options);

            var provider = new OptionsInputStreamProvider(_console, _file, wrapper);

            var stream = provider.Get();

            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.CanRead);

            File.Delete(tempFile);
        }

        [TestMethod]
        public void InputStream_StdIn_CanRead()
        {
            var options = new StreamOptions()
            {
                Input = new StreamOptions.Channel() { Type = nameof(StreamOptions.Types.StdIn) }
            };

            var wrapper = new OptionsWrapper<StreamOptions>(options);

            var provider = new OptionsInputStreamProvider(_console, _file, wrapper);

            var stream = provider.Get();

            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.CanRead);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void OutputStream_InvalidType_Throws()
        {
            var options = new StreamOptions()
            {
                Output = new StreamOptions.Channel() { Type = "invalid type" }
            };

            var wrapper = new OptionsWrapper<StreamOptions>(options);

            var provider = new OptionsOutputStreamProvider(_console, _file, wrapper);

            provider.Get();
        }

        [TestMethod]
        public void OutputStream_File_CanWrite()
        {
            var tempFile = Path.GetTempFileName();

            var options = new StreamOptions()
            {
                Output = new StreamOptions.Channel()
                {
                    Type = nameof(StreamOptions.Types.File),
                    Path = tempFile
                }
            };

            var wrapper = new OptionsWrapper<StreamOptions>(options);

            var provider = new OptionsOutputStreamProvider(_console, _file, wrapper);

            var stream = provider.Get();

            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.CanWrite);

            File.Delete(tempFile);
        }

        [TestMethod]
        public void OutputStream_StdOut_CanWrite()
        {
            var options = new StreamOptions()
            {
                Output = new StreamOptions.Channel() { Type = nameof(StreamOptions.Types.StdOut) }
            };

            var wrapper = new OptionsWrapper<StreamOptions>(options);

            var provider = new OptionsOutputStreamProvider(_console, _file, wrapper);

            var stream = provider.Get();

            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.CanWrite);
        }
    }
}
