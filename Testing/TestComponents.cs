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

        [TestMethod]
        public void TestOptionalParameterComponent()
        {
            var optionalParamComponent = new TestOptionalParameterComponent();

            var args = new string[] { "optionalparametertest", "--optional" };

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                optionalParamComponent.HandleCommand(args);

                string expectedWithParams = "optionalparametertest\r\n--optional\r\n";
                Assert.AreEqual(expectedWithParams, sw.ToString());
            }
        }

        [TestMethod]
        public void TestBat()
        {
            var rootComponent = new TestComponent("root", "root component", true);
            var configEditor = new TestComponent("configeditor", "edits config files");

            rootComponent.Children.Add(configEditor);

            var args = new string[] {"configEditor", "--music"};

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                rootComponent.HandleCommand(args);

                string expectedWithParams = "Validating arguments for configeditor function\r\nExecuted configeditor function\r\n";
                Assert.AreEqual(expectedWithParams, sw.ToString());
            }
        }

        [TestMethod]
        public void TestIgnoreFromTraversalComponents()
        {
            // this ones funky
            var rootComponent = new TestComponent("root", "root component", true);
            var music = new TestComponent("music", "music root", true);
            var albumUnzipper = new TestComponent("albumunzipper", "album unzipper");

            rootComponent.Children.Add(music);
            music.Children.Add(albumUnzipper);

            var args = new string[] { "albumunzipper" };

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                rootComponent.HandleCommand(args);

                string expectedWithParams = "Validating arguments for albumunzipper function\r\nExecuted albumunzipper function\r\n";
                Assert.AreEqual(expectedWithParams, sw.ToString());
            }
        }
    }
}