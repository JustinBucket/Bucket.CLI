using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bucket.CLI;
using Testing.TestObjects;

namespace Testing
{
    [TestClass]
    public class TestMenus
    {
        private static Menu GenerateMenu()
        {
            return new Menu("test menu", "menu for testing");
        }

        private static TestFunction GenerateTestFunction()
        {
            return new TestFunction();
        }

        [TestMethod]
        public void TestFunctionParentAssociation()
        {
            var menu = GenerateMenu();
            var function = GenerateTestFunction();

            menu.Functions.Add(function);

            Assert.AreEqual(function.Parent, menu);
        }

        [TestMethod]
        public void TestMenuParentAssociation()
        {
            var parentMenu = GenerateMenu();
            var childMenu = GenerateMenu();

            parentMenu.Menus.Add(childMenu);

            Assert.AreEqual(childMenu.Parent, parentMenu);
        }
    }
}