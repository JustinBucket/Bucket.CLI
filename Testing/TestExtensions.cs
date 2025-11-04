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
            var command1 = new TestComponent("command1", "first command");
            var command2 = new TestComponent("command2", "second command");
            var financials = new TestComponent("financials", "financial commands");
            var fcommand1 = new TestComponent("fcommand1", "first financial command");
            var fcommand2 = new TestComponent("fcommand2", "second financial command");
            var reports = new TestComponent("reports", "reports command");
            var sales = new TestComponent("sales", "sales command");
            var scommand1 = new TestComponent("scommand1", "sales subcommand 1");
            var banking = new TestComponent("banking", "banking commands");
            var bcommand1 = new TestComponent("bcommand1", "banking command 1");
            var bcommand2 = new TestComponent("bcommand2", "banking command 2");

            rComp.Children.Add(command1);
            rComp.Children.Add(command2);
            rComp.Children.Add(financials);
            financials.Children.Add(fcommand1);
            fcommand1.Children.Add(reports);
            financials.Children.Add(fcommand2);
            fcommand2.Children.Add(sales);
            sales.Children.Add(scommand1);
            rComp.Children.Add(banking);
            banking.Children.Add(bcommand1);
            banking.Children.Add(bcommand2);

            var treeLines = rComp.GenerateTree(0);

            File.WriteAllLines("OutputTree.txt", treeLines);
            var outputTree = string.Join(Environment.NewLine, treeLines);
            Assert.AreEqual("not equal", outputTree);
        }

        [TestMethod]
        public void TestSimpleTreeGeneration()
        {
            var rComp = new TestComponent("root", "root component");
            var command1 = new TestComponent("command1", "first command");
            var command2 = new TestComponent("command2", "second command");
            var financials = new TestComponent("financials", "financial commands");
            var fcommand1 = new TestComponent("fcommand1", "first financial command");
            var fcommand2 = new TestComponent("fcommand2", "second financial command");
            var reports = new TestComponent("reports", "reports command");
            var sales = new TestComponent("sales", "sales command");
            var scommand1 = new TestComponent("scommand1", "sales subcommand 1");
            var banking = new TestComponent("banking", "banking commands");
            var bcommand1 = new TestComponent("bcommand1", "banking command 1");
            var bcommand2 = new TestComponent("bcommand2", "banking command 2");

            rComp.Children.Add(command1);
            rComp.Children.Add(command2);
            rComp.Children.Add(financials);
            financials.Children.Add(fcommand1);
            fcommand1.Children.Add(reports);
            financials.Children.Add(fcommand2);
            fcommand2.Children.Add(sales);
            sales.Children.Add(scommand1);
            rComp.Children.Add(banking);
            banking.Children.Add(bcommand1);
            banking.Children.Add(bcommand2);

            var treeLines = rComp.GenerateTree();

            File.WriteAllLines("OutputTree.txt", treeLines);
            var outputTree = string.Join(Environment.NewLine, treeLines);
            Assert.AreEqual("not equal", outputTree);
        }

        [TestMethod]
        public void TestDepth2Generation()
        {
            var fcommand1 = new TestComponent("fcommand1", "first financial command");

            var treeString = fcommand1.GenerateTree(2);

            Assert.AreEqual("├   ├── fcommand1: first financial command", treeString.First());
        }

        [TestMethod]
        public void TestDepth3GenerationNested()
        {
            var scommand1 = new TestComponent("scommand1", "sales subcommand 1");

            var treeString = scommand1.GenerateTree(3);

            Assert.AreEqual("│   │   └── sales: sales command", treeString.First());
        }
    }
}