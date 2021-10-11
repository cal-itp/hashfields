using System;
using System.Text;
using System.Security.Cryptography;

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
        [DataRow("sha256", "Some input text")]
        [DataRow("sha384", "Very diFfeRent !npUT .txt")]
        [DataRow("sha512", @"Very very very long input text that spans multiple lines.

        That was just the first line before, but this is a second line all on its own.

        Third line!!! There are some blank lines between lines as well!")]
        public void Hash_HashesString_UsingAlgorithm(string hashAlgorithm, string inputText)
        {
            var inputBytes = Encoding.UTF8.GetBytes(inputText);
            var expected = Encoding.UTF8.GetString(HashAlgorithm.Create(hashAlgorithm).ComputeHash(inputBytes));

            var hasher = new StringHasher(hashAlgorithm);
            var actual = hasher.Hash(inputText);

            Assert.AreEqual(expected, actual);
        }
    }
}
