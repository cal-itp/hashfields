using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Data.Csv.Tests
{
    [TestClass]
    public class ColumnarTests
    {
        private static readonly byte[] _data = Encoding.UTF8.GetBytes(@"
            1, a, !
            2, b, @
            3, c, #"
        );

        private static Columnar NewColumnar(byte[] data, string delimiter = ",") => new(new MemoryStream(data), delimiter);

        [TestMethod]
        public void Apply_Modifies_Columns()
        {
            var data = Encoding.UTF8.GetBytes(@"
            2, 3, 5
            4, 6, 10
            6, 9, 15
            ");

            var columnar = NewColumnar(data);

            columnar.Apply(val => val + "_mod", "2");

            CollectionAssert.AreEqual(new[] { "4_mod", "6_mod" }, columnar["2"]);
            CollectionAssert.AreEqual(new[] { "6", "9" }, columnar["3"]);
            CollectionAssert.AreEqual(new[] { "10", "15" }, columnar["5"]);
        }

        [TestMethod]
        public void Apply_DoesNotModify_NonExistentColumns()
        {
            var data = Encoding.UTF8.GetBytes(@"
            2, 3, 5
            4, 6, 10
            6, 9, 15
            ");

            var columnar = NewColumnar(data);

            columnar.Apply(val => val + "_mod", "z");

            CollectionAssert.AreEqual(new[] { "2", "3", "5" }, columnar.Header);
            CollectionAssert.AreEqual(new[] { "4", "6" }, columnar["2"]);
            CollectionAssert.AreEqual(new[] { "6", "9" }, columnar["3"]);
            CollectionAssert.AreEqual(new[] { "10", "15" }, columnar["5"]);
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
        public void New_BlankLines_Trimmed()
        {
            var data = Encoding.UTF8.GetBytes(@"


            1, 2, 3


            ");

            var columnar = NewColumnar(data);

            var header = columnar.Header.ToArray();
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, header);

            Assert.AreEqual(3, columnar.Columns.Count);

            foreach (var column in columnar.Columns)
            {
                CollectionAssert.AreEqual(new List<string>(), column);
            }
        }

        [TestMethod()]
        public void New_Empty_Initializes_Empty()
        {
            var columnar = NewColumnar(Array.Empty<byte>());

            Assert.IsNotNull(columnar);
            Assert.IsNotNull(columnar.Header);
            Assert.IsNotNull(columnar.Columns);
        }

        [TestMethod]
        public void New_MultiRow_Initializes_HeaderAndColumns()
        {
            var columnar = NewColumnar(_data);

            var header = columnar.Header.ToArray();
            CollectionAssert.AreEqual(new[] { "1", "a", "!" }, header);

            Assert.AreEqual(3, columnar.Columns.Count);

            foreach (var column in header)
            {
                Assert.AreEqual(2, columnar[column].Count);

                if (column == "1")
                {
                    CollectionAssert.AreEqual(new[] { "2", "3" }, columnar[column]);
                }
                else if (column == "a")
                {
                    CollectionAssert.AreEqual(new[] { "b", "c" }, columnar[column]);
                }
                else if (column == "!")
                {
                    CollectionAssert.AreEqual(new[] { "@", "#" }, columnar[column]);
                }
            }
        }

        [TestMethod]
        public void New_MultiRow_Delimiter_Initializes_HeaderAndColumns()
        {
            var data = Encoding.UTF8.GetBytes(@"
            2<>3<>5
            4<>6<>10
            6<>9<>15
            ");

            const string delimiter = "<>";
            var columnar = NewColumnar(data, delimiter);

            foreach (var elem in columnar["2"].Union(columnar["3"].Union(columnar["5"])))
            {
                CollectionAssert.Contains(new[] { "4", "6", "9", "10", "15" }, elem);
            }
        }

        [TestMethod]
        public void New_MultiRow_BlankValues_Initializes_HeaderAndColumns()
        {
            var data = Encoding.UTF8.GetBytes(@"
                1, a, !
                2, b,
                3,, #
                4, d, $
            ");

            var columnar = NewColumnar(data);

            var header = columnar.Header.ToArray();
            CollectionAssert.AreEqual(new[] { "1", "a", "!" }, header);

            Assert.AreEqual(3, columnar.Columns.Count);

            foreach (var column in header)
            {
                Assert.AreEqual(3, columnar[column].Count);

                if (column == "1")
                {
                    CollectionAssert.AreEqual(new[] { "2", "3", "4" }, columnar[column]);
                }
                else if (column == "a")
                {
                    CollectionAssert.AreEqual(new[] { "b", "", "d" }, columnar[column]);
                }
                else if (column == "!")
                {
                    CollectionAssert.AreEqual(new[] { "", "#", "$" }, columnar[column]);
                }
            }
        }

        [TestMethod]
        public void New_SingleRow_Initializes_HeaderAndColumns()
        {
            var data = Encoding.UTF8.GetBytes("1, 2, 3");
            var columnar = NewColumnar(data);

            var header = columnar.Header.ToArray();
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, header);

            Assert.AreEqual(3, columnar.Columns.Count);

            foreach (var column in columnar.Columns)
            {
                CollectionAssert.AreEqual(new List<string>(), column);
            }
        }

        [TestMethod]
        public void New_TrimsFieldValues()
        {
            var data = Encoding.UTF8.GetBytes(@"
            1,      a,           !
            2,      b,           @
            3,      c,           #
            4,      d,           $
            ");

            var columnar = NewColumnar(data);

            var header = columnar.Header.ToArray();
            CollectionAssert.AreEqual(new[] { "1", "a", "!" }, header);

            foreach (var column in columnar.Columns)
            {
                foreach (var value in column)
                {
                    Assert.IsFalse(value.StartsWith(" "));
                    Assert.IsFalse(value.EndsWith(" "));
                }
            }
        }

        [DataTestMethod]
        [DataRow(new[] { "1", "a" })]
        [DataRow(new[] { "1", "!" })]
        [DataRow(new[] { "a", "!" })]
        public void Remove_Removes_Columns(string[] columns)
        {
            var columnar = NewColumnar(_data);

            var expectedHeader = columnar.Header.Except(columns).Single();
            var expectedColumn = columnar[expectedHeader];

            columnar.Remove(columns);

            Assert.AreEqual(1, columnar.Header.Count);
            Assert.AreEqual(1, columnar.Columns.Count);
            Assert.IsTrue(columnar.Header.Contains(expectedHeader));
            CollectionAssert.AreEqual(columnar.Columns[0], expectedColumn);
        }

        [DataTestMethod]
        [DataRow(new string[0])]
        [DataRow(new[] { "z", "&" })]
        public void Remove_DoesNotRemove_EmptyOrNonExistentColumns(string[] columns)
        {
            var columnar = NewColumnar(_data);

            columnar.Remove(columns);

            Assert.AreEqual(3, columnar.Header.Count);
            Assert.AreEqual(3, columnar.Columns.Count);
            Assert.IsTrue(columnar.Header.Contains("a"));
            Assert.IsTrue(columnar.Header.Contains("1"));
            Assert.IsTrue(columnar.Header.Contains("!"));
        }

        [TestMethod]
        public void Rows_MultiRow()
        {
            var columnar = NewColumnar(_data);

            var rows = columnar.Rows();

            Assert.AreEqual(3, rows.Count);
            CollectionAssert.AreEqual(new[] { "1", "a", "!" }, rows[0]);
            CollectionAssert.AreEqual(new[] { "2", "b", "@" }, rows[1]);
            CollectionAssert.AreEqual(new[] { "3", "c", "#" }, rows[2]);
        }

        [TestMethod]
        public void Rows_MultiRow_Duplicates()
        {
            var data = Encoding.UTF8.GetBytes(@"
                1, a, !
                2, a, @
                3, c, #
                4, d, #
            ");

            var columnar = NewColumnar(data);

            var rows = columnar.Rows();

            Assert.AreEqual(4, rows.Count);
            CollectionAssert.AreEqual(new[] { "1", "a", "!" }, rows[0]);
            CollectionAssert.AreEqual(new[] { "2", "a", "@" }, rows[1]);
            CollectionAssert.AreEqual(new[] { "3", "c", "#" }, rows[2]);
            CollectionAssert.AreEqual(new[] { "4", "d", "#" }, rows[3]);
        }

        [TestMethod]
        public void Rows_SingleRow()
        {
            var data = Encoding.UTF8.GetBytes("1, a, !");

            var columnar = NewColumnar(data);

            var rows = columnar.Rows();

            Assert.AreEqual(1, rows.Count);
            CollectionAssert.AreEqual(new[] { "1", "a", "!" }, rows[0]);
        }

        [TestMethod]
        public void Write_EndsWithNewline()
        {
            var text = String.Join(Environment.NewLine, new[] { "1, a, !", "2, b, @", "3, c, #" });

            Assert.IsFalse(text.EndsWith(Environment.NewLine));

            var data = Encoding.UTF8.GetBytes(text);
            var columnar = NewColumnar(data);
            var destination = new MemoryStream();

            columnar.Write(destination);

            var result = Encoding.UTF8.GetString(destination.ToArray());

            StringAssert.EndsWith(result, Environment.NewLine);
        }
    }
}
