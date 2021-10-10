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
    }
}
