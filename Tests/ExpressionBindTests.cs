using FunctionZero.ExpressionParserZero.Binding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using zBindTests;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class ExpressionBindTests
    {
        [TestMethod]
        public void TestExpression()
        {
            var host = new TestClass(null, 5);
            var binding = new ExpressionBind(host, "TestInt * 2");
            Assert.AreEqual(10, (int)binding.Result);
            
            host.TestInt++;
            Assert.AreEqual(12, (int)binding.Result);
        }

        [TestMethod]
        public void TestAutoExpression()
        {
            var host = new TestClass(null, 5);
            var binding = new ExpressionBind(host, "TestInt * 2");
            Assert.AreEqual(10, (int)binding.Result);

            host.TestInt++;
            Assert.AreEqual(12, (int)binding.Result);
        }

        [TestMethod]
        public void TestNestedExpression()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 41), 6), 5);

            var binding = new ExpressionBind(host, $"(Child.TestInt + Child.Child.TestInt) * TestInt");
            Assert.AreEqual((6 + 41) * 5, (int)binding.Result);

            host.Child.TestInt++;
            Assert.AreEqual((7 + 41) * 5, (int)binding.Result);
        }

        [TestMethod]
        public void TestAutoNestedExpression()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 41), 6), 5);

            var binding = new ExpressionBind(host, $"(Child.TestInt + Child.Child.TestInt) * TestInt");
            Assert.AreEqual((6 + 41) * 5, (int)binding.Result);
            host.Child.TestInt++;
            Assert.AreEqual((7 + 41) * 5, (int)binding.Result);
        }


        public int? MyNullableInt { get; set; }

        [TestMethod]
        public void TestNullableExpression()
        {
            MyNullableInt = 42;

            var binding = new ExpressionBind(this, $"MyNullableInt");
            Assert.AreEqual(42, (int?)binding.Result);
        }

        [TestMethod]
        public void TestNullNullableExpression()
        {
            MyNullableInt = null;

            var binding = new ExpressionBind(this, $"MyNullableInt");
            Assert.AreEqual(null, (int?)binding.Result);
        }

        [TestMethod]
        public void TestFloatParserExpression()
        {
            var binding = new ExpressionBind(this, "0.1");
            Assert.AreEqual(0.1, (double)binding.Result);
        }

        [TestMethod]
        public void TestNegativeFloatParserExpression()
        {
            var binding = new ExpressionBind(this, "-2.2");
            Assert.AreEqual(-2.2, (double)binding.Result);
        }


        //var binding = new ExpressionBind(/*...*/, "0.1"); // Result gives 1 instead of 0.1
        //var binding = new ExpressionBind(/*...*/, "-2.2"); // Result gives -22 instead of -2.2
    }
}