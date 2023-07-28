using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Data.Tests
{
    [TestClass]
    public class StringHasherTests
    {
        [TestMethod]
        public void New_DefaultSupportedAlgorithm()
        {
            var hasher = new StringHasher();

            Assert.IsNotNull(hasher);
        }

        [DataTestMethod]
        [DataRow("sha256")]
        [DataRow("SHA256")]
        [DataRow("sHa256")]
        [DataRow("sha384")]
        [DataRow("SHA384")]
        [DataRow("sHa384")]
        [DataRow("sha512")]
        [DataRow("SHA512")]
        [DataRow("ShA512")]
        public void New_SupportedAlgorithm_Supported(string supported)
        {
            var hasher = new StringHasher(supported);

            Assert.IsNotNull(hasher);
        }

        [DataTestMethod]
        [DataRow("md5")]
        [DataRow("sha1")]
        [DataRow("not")]
        [ExpectedException(typeof(ArgumentException))]
        public void New_UnsupportedAlgorithm_Unsupported(string unsupported)
        {
            new StringHasher(unsupported);
        }

        [DataTestMethod]
        [DataRow("sha256", "Some input text", "13-26-5A-A4-B5-9F-21-67-46-00-1B-73-20-78-ED-8B-6C-CB-1D-04-70-D8-C3-DA-71-0D-85-39-21-4B-80-D3")]
        [DataRow("sha384", "Very diFfeRent !npUT .txt", "F2-2C-92-82-E6-DB-AC-C5-4E-E5-40-FE-6D-7F-AB-29-22-69-53-1D-29-03-CF-A8-FF-D0-F9-74-64-36-DE-BC-2C-38-EA-E5-69-44-B3-18-3B-DD-A4-E9-BE-74-4E-43")]
        [DataRow("sha512", @"Very very very long input text that spans multiple lines.

        That was just the first line before, but this is a second line all on its own.

        Third line!!! There are some blank lines between lines as well!", "BE-1C-83-A0-B4-3A-5C-84-64-58-8C-9C-83-99-E0-C9-CA-8B-6F-C4-6F-4F-DA-29-1E-DD-62-88-13-50-AD-6F-88-FD-20-7A-9C-1E-F2-5E-41-BB-DD-B3-F5-D9-37-B7-E7-FB-09-6A-50-8A-49-9F-36-DC-28-91-A4-AA-C7-AD")]
        public void Hash_HashesString_UsingAlgorithm(string hashAlgorithm, string inputText, string expected)
        {
            var hasher = new StringHasher(hashAlgorithm);
            var actual = hasher.Hash(inputText);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("sha256", "Some input text", "13-26-5a-a4-b5-9f-21-67-46-00-1b-73-20-78-ed-8b-6c-cb-1d-04-70-d8-c3-da-71-0d-85-39-21-4b-80-d3")]
        public void Hash_MakesLowercase(string hashAlgorithm, string inputText, string expected)
        {
            var hasher = new StringHasher(hashAlgorithm);
            var actual = hasher.Hash(inputText, lowercase: true);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("sha256", "Some input text", "13265AA4B59F216746001B732078ED8B6CCB1D0470D8C3DA710D8539214B80D3")]
        public void Hash_RemovesHyphens(string hashAlgorithm, string inputText, string expected)
        {
            var hasher = new StringHasher(hashAlgorithm);
            var actual = hasher.Hash(inputText, hyphens: false);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("sha256", "Some input text", "13265aa4b59f216746001b732078ed8b6ccb1d0470d8c3da710d8539214b80d3")]
        public void Hash_RemovesHyphensAndMakesLowercase(string hashAlgorithm, string inputText, string expected)
        {
            var hasher = new StringHasher(hashAlgorithm);
            var actual = hasher.Hash(inputText, hyphens: false, lowercase: true);

            Assert.AreEqual(expected, actual);
        }
    }
}
