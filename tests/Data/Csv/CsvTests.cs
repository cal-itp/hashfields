using System;
using System.IO;
using System.Linq;
using System.Text;

using HashFields.Data.Tests.Fakes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Data.Csv.Tests
{
    [TestClass]
    public class CsvTests
    {
        [TestMethod]
        public void New_Delimiter_InitializesDelimiter()
        {
            const string delimiter = "delim";
            var csv = new Csv("", delimiter);

            Assert.AreEqual(delimiter, csv.Delimiter);
        }

        [TestMethod]
        public void New_DelimiterNull_InitializesDefaultDelimiter()
        {
            var csv = new Csv("");

            Assert.AreEqual(Csv.DefaultDelimiter, csv.Delimiter);
        }

        [TestMethod]
        public void New_StreamEmpty_InitializesEmpty()
        {
            var csv = new Csv(new MemoryStreamProvider(Array.Empty<byte>()));

            Assert.IsNotNull(csv);
            Assert.IsNotNull(csv.Header);
        }

        [TestMethod]
        public void New_StreamNull_InitializesEmpty()
        {
            var csv = new Csv((MemoryStreamProvider)null);

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
        public void New_TextNull_InitializesEmpty()
        {
            var csv = new Csv((string)null);

            Assert.IsNotNull(csv);
            Assert.IsNotNull(csv.Header);
        }

        [TestMethod]
        public void New_Stream_Columnar()
        {
            var data = Encoding.UTF8.GetBytes("1, 2, 3");
            var expected = new Columnar(new MemoryStream(data), ",");

            var csv = new Csv(new MemoryStreamProvider(data));

            Assert.AreEqual(expected, csv._columnar);
        }

        [TestMethod]
        public void New_Text_Columnar()
        {
            const string text = "1, 2, 3";
            var data = Encoding.UTF8.GetBytes(text);
            var expected = new Columnar(new MemoryStream(data), ",");

            var csv = new Csv(text);

            Assert.AreEqual(expected, csv._columnar);
        }

        [DataTestMethod]
        [DataRow(new[] { "1", "2" })]
        [DataRow(new[] { "1", "3" })]
        [DataRow(new[] { "2", "3" })]
        public void Remove_Removes_Column(string[] columns)
        {
            const string text = "1, 2, 3";

            var expectedHeader = text.Split(",", StringSplitOptions.TrimEntries)
                                     .Except(columns)
                                     .ToArray();

            var csv = new Csv(text);
            csv.Remove(columns);

            CollectionAssert.AreEqual(expectedHeader, csv.Header);
        }

        [DataTestMethod]
        [DataRow(new string[0])]
        [DataRow(null)]
        public void Remove_DoesNotRemove_EmptyOrNullHeaders(string[] columns)
        {
            const string text = "1, 2, 3";

            var expectedHeader = new[] { "1", "2", "3" };

            var csv = new Csv(text);
            csv.Remove(columns);

            CollectionAssert.AreEqual(expectedHeader, csv.Header);
        }

        [TestMethod]
        public void Write_Stream()
        {
            var data = Encoding.UTF8.GetBytes("1,2,3\na,b,c\n!,@,#\n");

            var csv = new Csv(new MemoryStreamProvider(data));

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
