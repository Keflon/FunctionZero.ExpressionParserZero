using FunctionZero.ExpressionParserZero.Binding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using zBindTests;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class PathBindTests
    {
        [TestMethod]
        public void TestBindToInt()
        {
            var host = new TestClass(null, 5);
            var binding = new PathBind(host, nameof(TestClass.TestInt));
            Assert.AreEqual(5, binding.Value);

            host.TestInt++;
            Assert.AreEqual(6, binding.Value);
        }

        [TestMethod]
        public void TestBindToNestedInt()
        {
            var host = new TestClass(new TestClass(null, 6), 5);
            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}");
            Assert.AreEqual(6, binding.Value);

            host.Child.TestInt++;
            Assert.AreEqual(7, binding.Value);
        }

        [TestMethod]
        public void TestBindToReplacedNestedInt()
        {
            var host = new TestClass(new TestClass(null, 6), 5);
            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}");
            Assert.AreEqual(6, binding.Value);

            host.Child.TestInt++;
            Assert.AreEqual(7, binding.Value);

            host.Child = new TestClass(null, -11);
            Assert.AreEqual(-11, binding.Value);
        }

        [TestMethod]
        public void TestBindToReplacedNestedNestedInt()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 41), 6), 5);
            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}");
            Assert.AreEqual(41, binding.Value);

            host.Child.Child.TestInt++;
            Assert.AreEqual(42, binding.Value);

            host.Child = new TestClass(new TestClass(null, -42), -99);
            Assert.AreEqual(-42, binding.Value);
        }

        [TestMethod]
        public void TestBindToProperty()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 41), 6), 5);

            int target = 0;

            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}", (o) => target = (int)o);
            Assert.AreEqual(41, binding.Value);
            Assert.AreEqual(binding.Value, target);

            host.Child.Child.TestInt++;
            Assert.AreEqual(42, binding.Value);

            host.Child = new TestClass(new TestClass(null, -42), -99);
            Assert.AreEqual(-42, binding.Value);
            Assert.AreEqual(binding.Value, target);
        }
    }
}