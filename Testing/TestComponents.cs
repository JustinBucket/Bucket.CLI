using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bucket.CLI;
using Testing.TestObjects;

namespace Testing
{
    [TestClass]
    public class TestComponents
    {
        [TestMethod]
        public void TestComponentInfoGeneration()
        {
            var component = new TestComponent();

            var componentInfo = component.GenerateInfo();

            Assert.AreEqual("test: function for testing", componentInfo);
        }

        private static Component GenerateComponent()
        {
            return new TestComponent("test menu", "menu for testing");
        }

        [TestMethod]
        public void TestParentAssociation()
        {
            var menu = GenerateComponent();
            var function = new TestComponent("test function", "function for testing");

            menu.Children.Add(function);

            Assert.AreEqual(function.Parent, menu);
        }

        [TestMethod]
        public void TestMenuParentAssociation()
        {
            var parentMenu = GenerateComponent();
            var childMenu = GenerateComponent();

            parentMenu.Children.Add(childMenu);

            Assert.AreEqual(childMenu.Parent, parentMenu);
        }

        [TestMethod]
        public void TestIgnoreFromTraversal()
        {
            var ignoredMenu = new TestComponent("ignored", "menu to be ignored", true);
            var secondMenu = new TestComponent("test", "function for testing");

            ignoredMenu.Children.Add(secondMenu);

            var args = new string[] { "test" };

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                ignoredMenu.HandleCommand(args);

                string expected = "Validating arguments for test function\r\nExecuted test function\r\n";
                Assert.AreEqual(expected, sw.ToString());
            }
        }
    }
}