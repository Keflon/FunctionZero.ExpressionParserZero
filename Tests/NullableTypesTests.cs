using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class NullableTypesTests
    {
        [TestMethod]
        public void TestSimpleNullableBoolToBoolEquality()
        {
            ExpressionParser e = new ExpressionParser();
            ExpressionEvaluator ev = new ExpressionEvaluator();
            VariableSet variables = new VariableSet();

            variables.RegisterNullableBool("Left", true);
            variables.RegisterBool("Right", false);

            bool? left = true;
            bool right = false;

            bool expectedResult = left == right;

            var result = e.Parse("Left == Right");

            var evalResult = ev.Evaluate(result, variables);
            Assert.AreEqual(1, evalResult.Count);
            bool actualResult = (bool)evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestSimpleBoolToNullableBoolEquality()
        {
            ExpressionParser e = new ExpressionParser();
            ExpressionEvaluator ev = new ExpressionEvaluator();
            VariableSet variables = new VariableSet();

            variables.RegisterBool("Left", true);
            variables.RegisterNullableBool("Right", false);

            bool left = true;
            bool? right = false;

            bool expectedResult = left == right;

            var result = e.Parse("Left == Right");

            var evalResult = ev.Evaluate(result, variables);
            Assert.AreEqual(1, evalResult.Count);
            bool actualResult = (bool)evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestSimpleNullableBoolToNullableBoolEquality()
        {
            ExpressionParser e = new ExpressionParser();
            ExpressionEvaluator ev = new ExpressionEvaluator();
            VariableSet variables = new VariableSet();

            variables.RegisterNullableBool("Left", true);
            variables.RegisterNullableBool("Right", null);

            bool? left = true;
            bool? right = false;

            bool expectedResult = left == right;

            var result = e.Parse("Left == Right");

            var evalResult = ev.Evaluate(result, variables);
            Assert.AreEqual(1, evalResult.Count);
            bool actualResult = (bool)evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);
        }

	    [TestMethod]
	    public void TestLongAddNullableLong()
	    {
		    ExpressionParser e = new ExpressionParser();
		    ExpressionEvaluator ev = new ExpressionEvaluator();
		    VariableSet variables = new VariableSet();

		    variables.RegisterLong("Left", 4);
		    variables.RegisterNullableLong("Right", -5);

		    long left = 4;
		    long? right = -5;

		    long? expectedResult = left + right;

		    var result = e.Parse("Left + Right");

		    var evalResult = ev.Evaluate(result, variables);
		    Assert.AreEqual(1, evalResult.Count);
		    long? actualResult = (long?)evalResult.Pop().GetValue();
		    Assert.AreEqual(expectedResult, actualResult);
	    }

	    [TestMethod]
	    public void TestLongAddNullableLongNull()
	    {
		    ExpressionParser e = new ExpressionParser();
		    ExpressionEvaluator ev = new ExpressionEvaluator();
		    VariableSet variables = new VariableSet();

		    variables.RegisterLong("Left", 4);
		    variables.RegisterNullableLong("Right", null);

		    long left = 4;
		    long? right = null;

		    long? expectedResult = left + right;

		    var result = e.Parse("Left + Right");

		    var evalResult = ev.Evaluate(result, variables);
		    Assert.AreEqual(1, evalResult.Count);
		    long? actualResult = (long?)evalResult.Pop().GetValue();
		    Assert.AreEqual(expectedResult, actualResult);
	    }

	    [TestMethod]
	    public void TestNullableLongAddLong()
	    {
		    ExpressionParser e = new ExpressionParser();
		    ExpressionEvaluator ev = new ExpressionEvaluator();
		    VariableSet variables = new VariableSet();

		    variables.RegisterNullableLong("Left", 4);
		    variables.RegisterLong("Right", -5);

		    long? left = 4;
		    long right = -5;

		    long? expectedResult = left + right;

		    var result = e.Parse("Left + Right");

		    var evalResult = ev.Evaluate(result, variables);
		    Assert.AreEqual(1, evalResult.Count);
		    long? actualResult = (long?)evalResult.Pop().GetValue();
		    Assert.AreEqual(expectedResult, actualResult);
	    }

	    [TestMethod]
	    public void TestNullableLongNullAddLong()
	    {
		    ExpressionParser e = new ExpressionParser();
		    ExpressionEvaluator ev = new ExpressionEvaluator();
		    VariableSet variables = new VariableSet();

		    variables.RegisterNullableLong("Left", null);
		    variables.RegisterLong("Right", 5);

		    long? left = null;
		    long right = 5;

		    long? expectedResult = left + right;

		    var result = e.Parse("Left + Right");

		    var evalResult = ev.Evaluate(result, variables);
		    Assert.AreEqual(1, evalResult.Count);
		    long? actualResult = (long?)evalResult.Pop().GetValue();
		    Assert.AreEqual(expectedResult, actualResult);
	    }
	}
}
