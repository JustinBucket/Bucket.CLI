using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bucket.CLI;
using Testing.TestObjects;

namespace Testing
{
    [TestClass]
    public class TestContext
    {
        [TestMethod]
        public void TestCommandParsing()
        {
            var root = new TestComponent("test", "this is a test component");
            var targetComponent = new TestComponent("target", "this is the target component");
            var args = new string[] { "test", "target" };
            root.Children.Add(targetComponent);

            var context = new Context(root, args);

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                context.Execute();

                string expected = "Executed target function\r\n";
                Assert.AreEqual(expected, sw.ToString());
            }

        }
    }
}