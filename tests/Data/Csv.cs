using System.Collections.Generic;
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
    }
}
