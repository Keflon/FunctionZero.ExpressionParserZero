using FunctionZero.ExpressionParserZero.Binding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using zBindTests;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class ExpressionSampleTests : INotifyPropertyChanged
    {
        private int _testInt;
        private float _testFloat;
        private TestClass _testObject;

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int TestInt
        {
            get => _testInt;
            set => SetProperty(ref _testInt, value);
        }
        public float TestFloat
        {
            get => _testFloat;
            set => SetProperty(ref _testFloat, value);
        }

        public TestClass TestObject
        {
            get => _testObject;
            set => SetProperty(ref _testObject, value);
        }

        [TestMethod]
        public void TestExpression()
        {
            int staleCount = 0;
            int changedCount = 0;

            TestObject = new TestClass(12);

            TestInt = 5;
            TestFloat = 6.2F;

            var binding = new ExpressionBind(this, "TestObject.TestInt * (TestInt + TestFloat * 2)");

            binding.ValueIsStale += (sender, ea) => staleCount++;
            binding.ResultChanged +=
                (sender, ea) =>
                {
                    Debug.WriteLine(ea.NewValue);
                    changedCount++;
                };

            Assert.AreEqual(TestObject.TestInt * (TestInt + TestFloat * 2), (float)binding.Result);
            Assert.AreEqual(0, staleCount);
            Assert.AreEqual(1, changedCount);

            TestObject.TestInt++;
            
            Assert.AreEqual(1, staleCount);
            Assert.AreEqual(1, changedCount);
            Assert.AreEqual(TestObject.TestInt * (TestInt + TestFloat * 2), (float)binding.Result);
            Assert.AreEqual(1, staleCount);
            Assert.AreEqual(2, changedCount);

            TestInt = 2;

            Assert.AreEqual(2, staleCount);
            Assert.AreEqual(2, changedCount);
            Assert.AreEqual(TestObject.TestInt * (TestInt + TestFloat * 2), (float)binding.Result);
            Assert.AreEqual(2, staleCount);
            Assert.AreEqual(3, changedCount);

            TestFloat = -4.6F;
            Assert.AreEqual(3, staleCount);
            Assert.AreEqual(3, changedCount);
            Assert.AreEqual(TestObject.TestInt * (TestInt + TestFloat * 2), (float)binding.Result);
            Assert.AreEqual(3, staleCount);
            Assert.AreEqual(4, changedCount);
        }


        [TestMethod]
        public void TestPathBind()
        {
            TestObject = new TestClass(12);

            var binding = new PathBind(this, "TestObject.TestInt");
            
            Assert.AreEqual(12, binding.Value);
            
            TestObject.TestInt++;
            Assert.AreEqual(13, binding.Value);
        }
    }
}