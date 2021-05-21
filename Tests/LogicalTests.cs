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
    public class LogicalTests : INotifyPropertyChanged
    {
        private int _intA;
        private int _intB;
        private LogicalTests _myObject;

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

        public int IntA
        {
            get => _intA;
            set => SetProperty(ref _intA, value);
        }
        public int IntB
        {
            get => _intB;
            set => SetProperty(ref _intB, value);
        }
        public LogicalTests MyObject
        {
            get => _myObject;
            set => SetProperty(ref _myObject, value);
        }


        [TestMethod]
        public void TestExpression0()
        {
            var binding = new ExpressionBind(this, "5>6 && 6>7");
            Assert.AreEqual(false, (bool)binding.Result);
        }

		[TestMethod]
        public void TestExpression1()
        {
            var binding = new ExpressionBind(this, "5>6 && 6>5");
            Assert.AreEqual(false, (bool)binding.Result);
        }


        [TestMethod]
        public void TestExpression2()
        {
            var binding = new ExpressionBind(this, "6>5 && 7>8");
            Assert.AreEqual(false, (bool)binding.Result);
        }

		[TestMethod]
        public void TestExpression3()
        {
            var binding = new ExpressionBind(this, "6>5 && 8>7");
            Assert.AreEqual(true, (bool)binding.Result);
        }


        //

        [TestMethod]
        public void TestExpression4()
        {
            IntA = 5;
            IntB = 6;
            var binding = new ExpressionBind(this, "IntA > IntB && IntB > IntA");
            Assert.AreEqual(false, (bool)binding.Result);
        }
        [TestMethod]
        public void TestExpression5()
        {
            IntA = 5;
            IntB = 6;
            MyObject = null;
            var binding = new ExpressionBind(this, "(False && True) && (SomethingHorrid)");
            Assert.AreEqual(false, (bool)binding.Result);
        }



    }
}