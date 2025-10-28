using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testing.TestObjects;

namespace Testing
{
    [TestClass]
    public class TestComponents
    {
        [TestMethod]
        public void TestComponentInfoGeneration()
        {
            var function = new TestFunction();

            var functionInfo = function.GenerateInfo();

            Assert.AreEqual("test: function for testing", functionInfo);
        }
    }
}