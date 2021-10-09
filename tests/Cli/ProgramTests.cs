using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Cli.Tests
{
    [TestClass]
    public class ProgramTests
    {
        private readonly TextWriter stdOut = Console.Out;
        private TextWriter testOut;

        [TestInitialize]
        public void Init()
        {
            testOut = new StringWriter();
            Console.SetOut(testOut);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Console.SetOut(stdOut);
        }

        [TestMethod]
        public void Main_Says_Hello()
        {
            Program.Main(Array.Empty<string>());

            StringAssert.Contains(testOut.ToString().ToLower(), "hello hashfields.cli");
        }
    }
}
