using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashFields.Data.Tests
{
    [TestClass]
    public class CsvTests
    {
        [TestMethod]
        public void New_Initializes_Empty()
        {
            var csv = new Csv("");
            Assert.IsNotNull(csv);
        }

        [TestMethod]
        public void ToColumnar_Empty_Returns_NewDict()
        {
            var csv = new Csv("");
            var columnar = csv.ToColumnar();

            Assert.ReferenceEquals(new Dictionary<string, List<string>>(), columnar);
        }

        [TestMethod]
        public void ToColumnar_SingleRow_Returns_DictWithKeysAndEmptyLists()
        {
            var csv = new Csv("1, 2, 3");
            var columnar = csv.ToColumnar();

            var keys = columnar.Keys.ToArray();
            CollectionAssert.AreEquivalent(new[] { "1", "2", "3" }, keys);

            foreach (var column in columnar.Values)
            {
                CollectionAssert.AreEquivalent(new List<string>(), column);
            }
        }

        [TestMethod]
        public void ToColumnar_BlankLines_Trimmed()
        {
            const string text = @"
            1, 2, 3
            ";
            var csv = new Csv(text);
            var columnar = csv.ToColumnar();

            var keys = columnar.Keys.ToArray();

            CollectionAssert.AreEquivalent(new[] { "1", "2", "3" }, keys);

            foreach (var column in columnar.Values)
            {
                CollectionAssert.AreEquivalent(new List<string>(), column);
            }
        }

        [TestMethod]
        public void ToColumnar_MultiRow_Returns_DictWithKeysAndLists()
        {
            const string text = @"1, a, !
            2, b, @
            3, c, #";
            var csv = new Csv(text);
            var columnar = csv.ToColumnar();

            var keys = columnar.Keys.ToArray();

            CollectionAssert.AreEquivalent(new[] { "1", "a", "!" }, keys);

            foreach (var column in keys)
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
    }
}
