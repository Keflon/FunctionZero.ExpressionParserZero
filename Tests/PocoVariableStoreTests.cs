using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Binding;
using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using zBindTests;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class PocoVariableStoreTests
    {



        public int IntA { get; set; }
        public int IntB { get; set; }
        public TestClass MyTestObject { get; set; }


        [TestMethod]
        public void TestStore0()
        {
            IntA = 5;
            IntB = 9;
            MyTestObject = new TestClass(21);
            var backingStore = new PocoBackingStore(this);
            ExpressionParser e = new ExpressionParser();

            int expectedResult = (IntA + IntB) * MyTestObject.TestInt;

            var compiledExpression = e.Parse("(IntA + IntB) * MyTestObject.TestInt");

            var evalResult = compiledExpression.Evaluate(backingStore);


            Assert.AreEqual(1, evalResult.Count);
            var actualResult = (int)evalResult.Pop().GetValue();


            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestStore1()
        {
            IntA = 5;
            IntB = 9;
            MyTestObject = new TestClass(21);
            var backingStore = new PocoBackingStore(this);
            ExpressionParser e = new ExpressionParser();


            var compiledExpression = e.Parse("(IntA + IntB) * MyTestObject.TestInt");

            int expectedResult = (IntA + IntB) * MyTestObject.TestInt;
            var evalResult = compiledExpression.Evaluate(backingStore);
            Assert.AreEqual(1, evalResult.Count);
            var actualResult = (int)evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);


            IntA++;
            IntB++;
            MyTestObject.TestInt++;

            expectedResult = (IntA + IntB) * MyTestObject.TestInt;
            evalResult = compiledExpression.Evaluate(backingStore);
            Assert.AreEqual(1, evalResult.Count);
            actualResult = (int)evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);

        }

        [TestMethod]
        public void TestStore2()
        {
            IntA = 5;
            IntB = 9;
            MyTestObject = new TestClass(21);
            var backingStore = new PocoBackingStore(this);
            ExpressionParser e = new ExpressionParser();


            var compiledExpression = e.Parse("IntB = MyTestObject.TestInt");

            var evalResult = compiledExpression.Evaluate(backingStore);



            Assert.AreEqual(1, evalResult.Count);
            Assert.AreEqual(IntB, MyTestObject.TestInt);
            Assert.AreEqual(IntB, 21);

            var actualResult = (int)evalResult.Pop().GetValue();

            Assert.AreEqual(21, actualResult);

        }
        [TestMethod]
        public void TestStore3()
        {
            IntA = 5;
            IntB = 9;
            MyTestObject = new TestClass(21);
            var backingStore = new PocoBackingStore(this);
            ExpressionParser e = new ExpressionParser();


            var compiledExpression = e.Parse("MyTestObject.TestInt = IntA");

            var evalResult = compiledExpression.Evaluate(backingStore);

            Assert.AreEqual(1, evalResult.Count);
            Assert.AreEqual(MyTestObject.TestInt, IntA);
            Assert.AreEqual(IntA, 5);

            var actualResult = (int)evalResult.Pop().GetValue();

            Assert.AreEqual(5, actualResult);

        }

        [TestMethod]

        public void TestStore4()
        {
            try
            {
                IntA = 5;
                IntB = 9;
                MyTestObject = new TestClass(21);
                var backingStore = new PocoBackingStore(this);
                ExpressionParser e = new ExpressionParser();


                var compiledExpression = e.Parse("MyTestObject.TestInt_BROKEN = IntA");

                var evalResult = compiledExpression.Evaluate(backingStore);
                throw new Exception("Broken");
            }
            catch (ExpressionEvaluatorException eeex)
            {
                Assert.AreEqual(eeex.Cause, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable);
                Assert.AreEqual(eeex.Message, "'MyTestObject.TestInt_BROKEN'");
            }
        }
        [TestMethod]

        public void TestStore5()
        {
            try
            {
                IntA = 5;
                IntB = 9;
                MyTestObject = new TestClass(21);
                var backingStore = new PocoBackingStore(this);
                ExpressionParser e = new ExpressionParser();


                var compiledExpression = e.Parse("MyTestObject.TestIn = IntA_BROKEN");

                var evalResult = compiledExpression.Evaluate(backingStore);
                throw new Exception("Broken");
            }
            catch (ExpressionEvaluatorException eeex)
            {
                Assert.AreEqual(eeex.Cause, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable);
                Assert.AreEqual(eeex.Message, "'IntA_BROKEN'");
            }
        }

        [TestMethod]
        public void TestStore6()
        {
            try
            {
                IntA = 5;
                IntB = 9;
                MyTestObject = new TestClass(21);
                var backingStore = new PocoBackingStore(this);
                ExpressionParser e = new ExpressionParser();

                var compiledExpression = e.Parse("IntB_ROKEN = MyTestObject.TestInt");


                var evalResult = compiledExpression.Evaluate(backingStore);
                throw new Exception("Broken");
            }
            catch (ExpressionEvaluatorException eeex)
            {
                Assert.AreEqual(eeex.Cause, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable);
                Assert.AreEqual(eeex.Message, "'IntB_ROKEN'");
            }
        }
    }
}