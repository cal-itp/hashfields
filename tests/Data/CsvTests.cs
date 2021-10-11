using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Data.Tests
{
    [TestClass]
    public class CsvTests
    {
        [TestMethod]
        public void New_StreamEmpty_InitializesEmpty()
        {
            var csv = new Csv(new MemoryStream(Array.Empty<byte>()));

            Assert.IsNotNull(csv);
            Assert.IsNotNull(csv.Header);
        }

        [TestMethod]
        public void New_TextEmpty_InitializesEmpty()
        {
            var csv = new Csv("");

            Assert.IsNotNull(csv);
            Assert.IsNotNull(csv.Header);
        }

        [TestMethod]
        public void New_NullStream_InitializesEmpty()
        {
            var csv = new Csv((Stream)null);

            Assert.IsNotNull(csv);
            Assert.IsNotNull(csv.Header);
        }

        [TestMethod]
        public void New_NullText_InitializesEmpty()
        {
            var csv = new Csv((string)null);

            Assert.IsNotNull(csv);
            Assert.IsNotNull(csv.Header);
        }

        [TestMethod]
        public void New_Stream_Columnar()
        {
            var data = Encoding.UTF8.GetBytes("1, 2, 3");
            var expected = new Csv.Columnar(new MemoryStream(data));

            var csv = new Csv(new MemoryStream(data));

            Assert.AreEqual(expected, csv._columnar);
        }

        [TestMethod]
        public void New_Text_Columnar()
        {
            const string text = "1, 2, 3";
            var data = Encoding.UTF8.GetBytes(text);
            var expected = new Csv.Columnar(new MemoryStream(data));

            var csv = new Csv(text);

            Assert.AreEqual(expected, csv._columnar);
        }

        [TestMethod]
        public void Write_Stream()
        {
            var data = Encoding.UTF8.GetBytes("1,2,3\na,b,c\n!,@,#\n");

            using var source = new MemoryStream(data);
            var csv = new Csv(source);

            var destination = new MemoryStream();
            CollectionAssert.AreEquivalent(destination.ToArray(), Array.Empty<byte>());

            csv.Write(destination);

            CollectionAssert.AreEquivalent(destination.ToArray(), data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Write_DestinationNull_ArgumentNullException()
        {
            var csv = new Csv("");

            csv.Write((Stream)null);
        }
    }
}
