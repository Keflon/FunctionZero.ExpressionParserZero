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



        [TestMethod]
        public void TestOneWayToSourceProperty()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 5), 6), 7);

            int target = -9;

            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}", (o) => target = (int)o);
            binding.BindTo(nameof(host.TestInt), PathBindMode.OneWayToSource);

            Assert.AreEqual(5, binding.Value);
            Assert.AreEqual(binding.Value, target);

            Assert.AreEqual(5, host.TestInt);

            host.TestInt++;

            Assert.AreEqual(host.TestInt, 6);
            Assert.AreEqual(host.Child.Child.TestInt, 5);

            host.Child.Child.TestInt = 42;

            Assert.AreEqual(42, binding.Value);
            Assert.AreEqual(target, 42);
            Assert.AreEqual(host.TestInt, 42);

            //binding.SetValue(host.TestInt + 1);
            //Assert.AreEqual(host.Child.Child.TestInt, host.TestInt);
        }

        [TestMethod]
        public void TestTwoWayBind_1_Property()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 5), 6), 7);

            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}")
                .BindTo(nameof(host.TestInt), PathBindMode.TwoWay);

            Assert.AreEqual(7, host.TestInt);
            Assert.AreEqual(host.TestInt, binding.Value);

            host.TestInt++;

            Assert.AreEqual(8, binding.Value);

            host.TestInt++;

            Assert.AreEqual(9, binding.Value);

        }

        [TestMethod]
        public void TestTwoWayBind_2_Property()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 5), 6), 7);

            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}")
                .BindTo(nameof(host.TestInt), PathBindMode.TwoWay);

            Assert.AreEqual(7, host.TestInt);
            Assert.AreEqual(host.TestInt, binding.Value);

            host.Child.Child.TestInt++;

            Assert.AreEqual(8, host.TestInt);
            Assert.AreEqual(8, binding.Value);

            host.Child.Child.TestInt++;

            Assert.AreEqual(9, host.TestInt);
            Assert.AreEqual(9, binding.Value);

        }

        [TestMethod]
        public void TestOneWayToSourceBind_1_Property()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 5), 6), 7);

            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}")
                .BindTo(nameof(host.TestInt), PathBindMode.OneWayToSource);

            Assert.AreEqual(5, host.TestInt);
            Assert.AreEqual(host.TestInt, binding.Value);

            host.TestInt++;

            Assert.AreEqual(5, binding.Value);

            host.TestInt++;

            Assert.AreEqual(5, binding.Value);

        }

        [TestMethod]
        public void TestOneWayToSourceBind_2_Property()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 5), 6), 7);

            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}")
                .BindTo(nameof(host.TestInt), PathBindMode.OneWayToSource);

            Assert.AreEqual(5, host.TestInt);
            Assert.AreEqual(host.TestInt, binding.Value);

            host.TestInt++;

            Assert.AreEqual(5, binding.Value);

            host.TestInt++;

            Assert.AreEqual(5, binding.Value);

        }

        [TestMethod]
        public void TestOneShotBind_1_Property()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 5), 6), 7);

            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}")
                .BindTo(nameof(host.TestInt), PathBindMode.OneShot);

            Assert.AreEqual(7, host.TestInt);
            Assert.AreEqual(host.TestInt, binding.Value);

            host.TestInt++;

            Assert.AreEqual(7, binding.Value);

            host.TestInt++;

            Assert.AreEqual(7, binding.Value);

        }

        [TestMethod]
        public void TestOneShotBind_2_Property()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 5), 6), 7);

            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}")
                .BindTo(nameof(host.TestInt), PathBindMode.OneShot);

            Assert.AreEqual(7, host.TestInt);
            Assert.AreEqual(host.TestInt, binding.Value);

            host.Child.Child.TestInt++;

            Assert.AreEqual(7, host.TestInt);
            Assert.AreEqual(8, binding.Value);

            host.Child.Child.TestInt++;

            Assert.AreEqual(7, host.TestInt);
            Assert.AreEqual(9, binding.Value);

        }

        [TestMethod]
        public void TestOneWayBind_1_Property()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 5), 6), 7);

            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}")
                .BindTo(nameof(host.TestInt), PathBindMode.OneWay);

            Assert.AreEqual(7, host.TestInt);
            Assert.AreEqual(host.TestInt, binding.Value);

            host.TestInt++;

            Assert.AreEqual(8, binding.Value);

            host.TestInt++;

            Assert.AreEqual(9, binding.Value);

        }

        [TestMethod]
        public void TestOneWayBind_2_Property()
        {
            var host = new TestClass(new TestClass(new TestClass(null, 5), 6), 7);

            var binding = new PathBind(host, $"{nameof(TestClass.Child)}.{nameof(TestClass.Child)}.{nameof(TestClass.TestInt)}")
                .BindTo(nameof(host.TestInt), PathBindMode.OneWay);

            Assert.AreEqual(7, host.TestInt);
            Assert.AreEqual(host.TestInt, binding.Value);

            host.Child.Child.TestInt++;

            Assert.AreEqual(7, host.TestInt);
            Assert.AreEqual(8, binding.Value);

            host.Child.Child.TestInt++;

            Assert.AreEqual(7, host.TestInt);
            Assert.AreEqual(9, binding.Value);

        }
    }
}