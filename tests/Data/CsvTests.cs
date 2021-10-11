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
            Assert.IsNotNull(csv.Columnar);
        }

        [TestMethod]
        public void New_TextEmpty_InitializesEmpty()
        {
            var csv = new Csv("");

            Assert.IsNotNull(csv);
            Assert.IsNotNull(csv.Columnar);
        }

        [TestMethod]
        public void New_NullStream_InitializesEmpty()
        {
            var csv = new Csv((Stream)null);

            Assert.IsNotNull(csv);
            Assert.IsNotNull(csv.Columnar);
        }

        [TestMethod]
        public void New_NullText_InitializesEmpty()
        {
            var csv = new Csv((string)null);

            Assert.IsNotNull(csv);
            Assert.IsNotNull(csv.Columnar);
        }

        [TestMethod]
        public void New_Stream_Columnar()
        {
            var data = Encoding.UTF8.GetBytes("1, 2, 3");
            var expected = new Columnar(new MemoryStream(data));

            var csv = new Csv(new MemoryStream(data));

            Assert.AreEqual(expected, csv.Columnar);
        }

        [TestMethod]
        public void New_Text_Columnar()
        {
            const string text = "1, 2, 3";
            var data = Encoding.UTF8.GetBytes(text);
            var expected = new Columnar(new MemoryStream(data));

            var csv = new Csv(text);

            Assert.AreEqual(expected, csv.Columnar);
        }

        [TestMethod]
        public void Write_Stream()
        {
            var data = Encoding.UTF8.GetBytes("1, 2, 3");

            using var source = new MemoryStream(data);
            var csv = new Csv(source);

            var destination = new MemoryStream();
            CollectionAssert.AreEquivalent(destination.ToArray(), Array.Empty<byte>());

            csv.Write(destination);

            CollectionAssert.AreEquivalent(destination.ToArray(), data);
        }

        [TestMethod]
        public void Write_Stream_RewindsStreams()
        {
            var data = Encoding.UTF8.GetBytes("1, 2, 3");

            using var source = new MemoryStream(data);
            var csv = new Csv(source);

            var destination = new MemoryStream();
            csv.Write(destination);

            Assert.AreEqual(0, source.Position);
            Assert.AreEqual(0, destination.Position);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Write_StreamNull_InvalidOperationException()
        {
            var csv = new Csv((Stream)null);
            var destination = new MemoryStream();

            csv.Write(destination);
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
