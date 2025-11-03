using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testing.TestObjects;
using Bucket.CLI;

namespace Testing
{
    [TestClass]
    public class TestExtensions
    {
        [TestMethod]
        public void TestTreeGeneration()
        {
            var rComp = new TestComponent("root", "root component");
            var tComp = new TestComponent("test", "test component");
            var tComp2 = new TestComponent("test2", "test component 2");

            rComp.Children.Add(tComp);
            rComp.Children.Add(tComp2);

            var treeLines = rComp.GenerateTree(0);

            File.WriteAllLines("OutputTree.txt", treeLines);
            var outputTree = string.Join(Environment.NewLine, treeLines);
            Assert.AreEqual("not eaqual", outputTree);
        }
    }
}