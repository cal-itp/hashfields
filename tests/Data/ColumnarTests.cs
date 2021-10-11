using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Data.Tests
{
    [TestClass]
    public class ColumnarTests
    {
        private static readonly byte[] _data = Encoding.UTF8.GetBytes(@"
            1, a, !
            2, b, @
            3, c, #"
        );

        private static Columnar NewColumnar(byte[] data) => new(new MemoryStream(data));

        [TestMethod()]
        public void New_Empty_Initializes_Empty()
        {
            var columnar = NewColumnar(Array.Empty<byte>());

            Assert.IsNotNull(columnar);
            Assert.IsNotNull(columnar.Header);
            Assert.IsNotNull(columnar.Columns);
        }

        [TestMethod]
        public void New_SingleRow_Initializes_HeaderAndColumns()
        {
            var data = Encoding.UTF8.GetBytes("1, 2, 3");
            var columnar = NewColumnar(data);

            var header = columnar.Header.ToArray();
            CollectionAssert.AreEquivalent(new[] { "1", "2", "3" }, header);

            Assert.AreEqual(3, columnar.Columns.Count);

            foreach (var column in columnar.Columns)
            {
                CollectionAssert.AreEquivalent(new List<string>(), column);
            }
        }

        [TestMethod]
        public void New_BlankLines_Trimmed()
        {
            var data = Encoding.UTF8.GetBytes(@"


            1, 2, 3


            ");

            var columnar = NewColumnar(data);

            var header = columnar.Header.ToArray();
            CollectionAssert.AreEquivalent(new[] { "1", "2", "3" }, header);

            Assert.AreEqual(3, columnar.Columns.Count);

            foreach (var column in columnar.Columns)
            {
                CollectionAssert.AreEquivalent(new List<string>(), column);
            }
        }

        [TestMethod]
        public void New_MultiRow_Returns_DictWithKeysAndLists()
        {
            var columnar = NewColumnar(_data);

            var header = columnar.Header.ToArray();
            CollectionAssert.AreEquivalent(new[] { "1", "a", "!" }, header);

            Assert.AreEqual(3, columnar.Columns.Count);

            foreach (var column in header)
            {
                Assert.AreEqual(2, columnar[column].Count);

                if (column == "1")
                {
                    CollectionAssert.AreEquivalent(new[] { "2", "3" }, columnar[column]);
                }
                else if (column == "a")
                {
                    CollectionAssert.AreEquivalent(new[] { "b", "c" }, columnar[column]);
                }
                else if (column == "!")
                {
                    CollectionAssert.AreEquivalent(new[] { "@", "#" }, columnar[column]);
                }
            }
        }

        [TestMethod]
        public void Equals_Other_Columnar()
        {
            var columnar1 = NewColumnar(_data);
            var columnar2 = NewColumnar(_data);

            Assert.IsTrue(columnar1.Equals(columnar2));
        }

        [TestMethod]
        public void Equals_Other_Object()
        {
            var columnar1 = NewColumnar(_data);
            var columnar2 = NewColumnar(_data);

            Assert.IsTrue(columnar1.Equals((object)columnar2));
        }

        [TestMethod]
        public void HashCode_Matches_Other()
        {
            var columnar1 = NewColumnar(_data);
            var columnar2 = NewColumnar(_data);

            Assert.AreEqual(columnar1.GetHashCode(), columnar2.GetHashCode());
        }

        [TestMethod]
        public void Remove_Removes_Columns()
        {
            var columnar = NewColumnar(_data);

            columnar.Remove("1", "!");

            Assert.AreEqual(1, columnar.Header.Count);
            Assert.AreEqual(1, columnar.Columns.Count);
            Assert.IsTrue(columnar.Header.Contains("a"));
            CollectionAssert.AreEquivalent(columnar.Columns[0], new string[] { "b", "c" }.ToList());
        }

        [TestMethod]
        public void Remove_DoesNotRemove_NonExistentColumns()
        {
            var columnar = NewColumnar(_data);

            columnar.Remove("z", "&");

            Assert.AreEqual(3, columnar.Header.Count);
            Assert.AreEqual(3, columnar.Columns.Count);
            Assert.IsTrue(columnar.Header.Contains("a"));
            Assert.IsTrue(columnar.Header.Contains("1"));
            Assert.IsTrue(columnar.Header.Contains("!"));
        }
    }
}
