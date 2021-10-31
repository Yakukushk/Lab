using Microsoft.VisualStudio.TestTools.UnitTesting;
using LabCalculator;
using System;
using System.Collections.Generic;
using System.Text;


namespace LabCalculator.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public class CalculatorTests
        {
            [TestMethod()]
            public void EvaluateTestExponent()
            {
                Assert.AreEqual(Calculator.Evaluate("0 ^ 0"), 1);
                Assert.AreEqual(Calculator.Evaluate("1 ^ 10"), 1);
                Assert.AreEqual(Calculator.Evaluate("10 ^ 1"), 10);
                Assert.AreEqual(Calculator.Evaluate("10 ^ 2"), 100);
                Assert.AreEqual(Calculator.Evaluate("10 ^ 0"), 1);
            }
        }
        [TestMethod()]
        public void EvaluateTestMinExpression()
        {
            Assert.AreEqual(Calculator.Evaluate("min(12, 10)"), 10);
            Assert.AreEqual(Calculator.Evaluate("min(1.2, 10)"), 1.2);
            Assert.AreEqual(Calculator.Evaluate("min(1, 1)"), 1);
            Assert.AreEqual(Calculator.Evaluate("min(1, 2)"), 1);
            Assert.AreEqual(Calculator.Evaluate("min(20,23)"), 20);
        }
        [TestMethod()]
        public void EvaluateTestMaxExpreContext()
        {
            Assert.AreEqual(Calculator.Evaluate("max(12, 10)"), 12);
            Assert.AreEqual(Calculator.Evaluate("max(1.2, 10)"), 10);
            Assert.AreEqual(Calculator.Evaluate("max(1, 1)"), 1);
            Assert.AreEqual(Calculator.Evaluate("max(-1, 0)"), 0);
            Assert.AreEqual(Calculator.Evaluate("max(20,-23)"), 20);
        }
        [TestMethod()]
        public void EvaluateTestUnaryMinusExpr()
        {
            Assert.AreEqual(Calculator.Evaluate("1 - 2"), -1);
            Assert.AreEqual(Calculator.Evaluate("33 - 21"), 12);
            Assert.AreEqual(Calculator.Evaluate("11 - 21"), -10);
            Assert.AreEqual(Calculator.Evaluate("2 - 1"), 1);
            Assert.AreEqual(Calculator.Evaluate("1 - 1"), 0);
        }
        [TestMethod()]
        public void EvaluateTestModExpression()
        {
            Assert.AreEqual(Calculator.Evaluate("12 mod 2"), 0);
            Assert.AreEqual(Calculator.Evaluate("13 mod 2"), 1);
            Assert.IsTrue(Math.Abs(Calculator.Evaluate("13.1 mod 2") - 1.1) < 0.1);
            Assert.AreEqual(Calculator.Evaluate("0 mod 2"), 0);
            Assert.AreEqual(Calculator.Evaluate("13 mod 2"), 1);
        }
    }
}
