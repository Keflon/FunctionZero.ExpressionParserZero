using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Parser.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace ExpressionParserUnitTests
{
	[TestClass]
	public class ExpressionParserTests
	{
		[TestMethod]
		public void TestUnaryMinus()
		{
			ExpressionParser parser = new ExpressionParser();

			var result = parser.Parse("-5");
			Assert.AreEqual("5 UnaryMinus ", Stringify(result));
		}

		[TestMethod]
		public void TestCompoundUnaryMinus()
		{
			ExpressionParser parser = new ExpressionParser();

			var result = parser.Parse("-5--(-6--3)");
			Assert.AreEqual("5 UnaryMinus 6 UnaryMinus 3 UnaryMinus - UnaryMinus - ", Stringify(result));
		}

		[TestMethod]
		public void SimpleTestBang()
		{
			ExpressionParser parser = new ExpressionParser();

			var result = parser.Parse("!5");
			Assert.AreEqual("5 ! ", Stringify(result));
		}

		[TestMethod]
		public void SimpleTestBool()
		{
			ExpressionParser parser = new ExpressionParser();

			var result = parser.Parse("True | False");
			Assert.AreEqual("True False | ", Stringify(result));
		}

		[TestMethod]
		public void TestBang()
		{
			ExpressionParser parser = new ExpressionParser();

			var result = parser.Parse("!(5+!True)");
			Assert.AreEqual("5 True ! + ! ", Stringify(result));
		}

		[TestMethod]
		public void TestBrackets()
		{
			ExpressionParser parser = new ExpressionParser();

			var result = parser.Parse("(5.2+3)*(3+5)");
			Assert.AreEqual("5.2 3 + 3 5 + * ", Stringify(result));
		}

		[TestMethod]
		public void TestSimpleFunctionCall()
		{
			ExpressionParser parser = new ExpressionParser();

			var result = parser.Parse("day+_debug_mul(_debug_mul(58, 23), _debug_mul(9,4))");
			Assert.AreEqual("day 58 23 _debug_mul 9 4 _debug_mul _debug_mul + ", Stringify(result));
		}

		//[TestMethod]
		//public void TestSimpleFunctionCallTooManyParameters()
		//{
		//	ExpressionParser parser = new ExpressionParser();
		//	// TODO: This test is a placeholder copy of another test and is yet to be written.
		//	var result = parser.Parse("_debug_mul(3,5,6)");
		//	Assert.AreEqual("day 58 23 _debug_mul 9 4 _debug_mul _debug_mul + ", Stringify(result));
		//}

		[TestMethod]
		public void TestNestedFunctionCall()
		{
			ExpressionParser parser = new ExpressionParser();
			parser.RegisterFunction("F0", null, 2);
			parser.RegisterFunction("F1", null, 2);
			var result = parser.Parse("F0(a,b+c,F1(d,e))");
			Assert.AreEqual("a b c + d e F1 F0 ", Stringify(result));
		}

		[TestMethod]
		public void TestFunctionAfterUnaryOperator()
		{
			ExpressionParser parser = new ExpressionParser();
			parser.RegisterFunction("F0", null, 1);
			var result = parser.Parse("!F0(0)");

			Assert.AreEqual("0 F0 ! ", Stringify(result));
		}

		[TestMethod]
		public void TestMultipleUnaryMinusOperator()
		{
			ExpressionParser parser = new ExpressionParser();
			var result = parser.Parse("---5");
			Assert.AreEqual("5 UnaryMinus UnaryMinus UnaryMinus ", Stringify(result));

		}

		[TestMethod]
		public void TestMultipleUnaryPlusOperator()
		{
			ExpressionParser parser = new ExpressionParser();
			var result = parser.Parse("+++5");
			Assert.AreEqual("5 UnaryPlus UnaryPlus UnaryPlus ", Stringify(result));
		}

		[TestMethod]
		public void TestMultipleUnaryOperatorBang()
		{
			ExpressionParser parser = new ExpressionParser();
			var result = parser.Parse("!!!False");
			Assert.AreEqual("False ! ! ! ", Stringify(result));
		}

		[TestMethod]
		public void TestMultipleUnaryOperatorTwo()
		{
			ExpressionParser parser = new ExpressionParser();
			var result = parser.Parse("3+!50");
			Assert.AreEqual("3 50 ! + ", Stringify(result));
		}

		[TestMethod]
		public void TestMissingOperator()
		{
			ExpressionParser parser = new ExpressionParser();
			try
			{
				var result = parser.Parse("0(0)");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(1, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.UnexpectedOpenParenthesis, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}

		[TestMethod]
		public void TestFunctionOperand()
		{
			ExpressionParser parser = new ExpressionParser();
			try
			{
				var result = parser.Parse("_debug_mul+");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(10, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.OpenParenthesisExpected, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}

		[TestMethod]
		public void TestFunctionEof()
		{
			ExpressionParser parser = new ExpressionParser();
			try
			{
				var result = parser.Parse("_debug_mul");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(10, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.OpenParenthesisExpected, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}


		[TestMethod]
		public void TestFunctionOperandOperand()
		{
			ExpressionParser parser = new ExpressionParser();
			try
			{
				var result = parser.Parse("_debug_mul+5");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(ExpressionParserException.ExceptionCause.OpenParenthesisExpected, ex.Cause);
				Assert.AreEqual(10, ex.Offset);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}

		[TestMethod]
		public void TestFunctionFunction()
		{
			ExpressionParser parser = new ExpressionParser();
			try
			{
				var result = parser.Parse("_debug_mul _debug_mul");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(11, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.OpenParenthesisExpected, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}


        [TestMethod]
		public void SimpleTestRegisteredAlphanumericOperators()
		{
		    ExpressionParser parser = new ExpressionParser();
			parser.RegisterOperator("AND", 4, LogicalAndMatrix.Create());
			//parser.RegisterOperator("AND", 4, OperatorActions.DoLogicalAnd);
			//parser.RegisterOperator("OR", 3, OperatorActions.DoLogicalOr);
			//parser.RegisterOperator("LT", 9, OperatorActions.DoLessThan);
			//parser.RegisterOperator("GT", 9, OperatorActions.DoGreaterThan);
			//parser.RegisterOperator("LTE", 9, OperatorActions.DoLessOrEqual);
			//parser.RegisterOperator("GTE", 9, OperatorActions.DoGreaterOrEqual);

			var result = parser.Parse("true AND false");
			Assert.AreEqual("True False AND ", Stringify(result));
		}
#if false

		[TestMethod]
		public void TestRegisteredAlphanumericOperators()
		{
			Parser parser = new Parser();
			parser.RegisterOperator("AND", 4, OperatorActions.DoLogicalAnd);
			parser.RegisterOperator("OR", 3, OperatorActions.DoLogicalOr);
			parser.RegisterOperator("LT", 9, OperatorActions.DoLessThan);
			parser.RegisterOperator("GT", 9, OperatorActions.DoGreaterThan);
			parser.RegisterOperator("LTE", 9, OperatorActions.DoLessOrEqual);
			parser.RegisterOperator("GTE", 9, OperatorActions.DoGreaterOrEqual);

			var result = parser.Parse("(3 OR white) AND (6 LTE purple)");
			Assert.AreEqual("3 white OR 6 purple LTE AND ", Stringify(result));
		}


		[TestMethod]
		public void SimpleRegisteredOperatorParseFail()
		{
			Parser parser = new Parser();
			try
			{
				parser.RegisterOperator("AND", 4, OperatorActions.DoLogicalAnd);
				parser.RegisterOperator("OR", 3, OperatorActions.DoLogicalOr);
				parser.RegisterOperator("LT", 9, OperatorActions.DoLessThan);
				parser.RegisterOperator("GT", 9, OperatorActions.DoGreaterThan);
				parser.RegisterOperator("LTE", 9, OperatorActions.DoLessOrEqual);
				parser.RegisterOperator("GTE", 9, OperatorActions.DoGreaterOrEqual);

				var result = parser.Parse("6LTE5");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(1, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.MalformedSymbol, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}

		[TestMethod]
		public void SimpleRegisteredOperatorParseFail2()
		{
			Parser parser = new Parser();
			try
			{
				parser.RegisterOperator("AND", 4, OperatorActions.DoLogicalAnd);
				parser.RegisterOperator("OR", 3, OperatorActions.DoLogicalOr);
				parser.RegisterOperator("LT", 9, OperatorActions.DoLessThan);
				parser.RegisterOperator("GT", 9, OperatorActions.DoGreaterThan);
				parser.RegisterOperator("LTE", 9, OperatorActions.DoLessOrEqual);
				parser.RegisterOperator("GTE", 9, OperatorActions.DoGreaterOrEqual);

				var result = parser.Parse("6LTE 5");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(1, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.MalformedSymbol, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}

		[TestMethod]
		public void RegisteredOperatorParseFail()
		{
			Parser parser = new Parser();
			try
			{
				parser.RegisterOperator("AND", 4, OperatorActions.DoLogicalAnd);
				parser.RegisterOperator("OR", 3, OperatorActions.DoLogicalOr);
				parser.RegisterOperator("LT", 9, OperatorActions.DoLessThan);
				parser.RegisterOperator("GT", 9, OperatorActions.DoGreaterThan);
				parser.RegisterOperator("LTE", 9, OperatorActions.DoLessOrEqual);
				parser.RegisterOperator("GTE", 9, OperatorActions.DoGreaterOrEqual);

				var result = parser.Parse("((3 OR white) AND (6 LTEpurple)");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(21, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.UnexpectedOperand, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}

		[TestMethod]
		public void RegisteredOperatorReallyIsAnOperand()
		{
			Parser parser = new Parser();
			try
			{
				parser.RegisterOperator("AND", 4, OperatorActions.DoLogicalAnd);
				parser.RegisterOperator("OR", 3, OperatorActions.DoLogicalOr);
				parser.RegisterOperator("LT", 9, OperatorActions.DoLessThan);
				parser.RegisterOperator("GT", 9, OperatorActions.DoGreaterThan);
				parser.RegisterOperator("LTE", 9, OperatorActions.DoLessOrEqual);
				parser.RegisterOperator("GTE", 9, OperatorActions.DoGreaterOrEqual);

				var result = parser.Parse("((3 OR white) AND (6 LTE5)");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(21, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.UnexpectedOperand, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}


		[TestMethod]
		public void RegisteredOperatorParseWithSimilarOperands()
		{
			Parser parser = new Parser();
			parser.RegisterOperator("AND", 4, OperatorActions.DoLogicalAnd);
			parser.RegisterOperator("OR", 3, OperatorActions.DoLogicalOr);
			parser.RegisterOperator("LT", 9, OperatorActions.DoLessThan);
			parser.RegisterOperator("GT", 9, OperatorActions.DoGreaterThan);
			parser.RegisterOperator("LTE", 9, OperatorActions.DoLessOrEqual);
			parser.RegisterOperator("GTE", 9, OperatorActions.DoGreaterOrEqual);

			var result = parser.Parse("(3 OR white) AND (LTE5 LTE LTE6)");
			Assert.AreEqual("3 white OR LTE5 LTE6 LTE AND ", Stringify(result));
		}


		[TestMethod]
		public void RegisteredOperatorParseWithSimilarOperands2()
		{
			Parser parser = new Parser();
			try
			{
				parser.RegisterOperator("AND", 4, OperatorActions.DoLogicalAnd);
				parser.RegisterOperator("OR", 3, OperatorActions.DoLogicalOr);
				parser.RegisterOperator("LT", 9, OperatorActions.DoLessThan);
				parser.RegisterOperator("GT", 9, OperatorActions.DoGreaterThan);
				parser.RegisterOperator("LTE", 9, OperatorActions.DoLessOrEqual);
				parser.RegisterOperator("GTE", 9, OperatorActions.DoGreaterOrEqual);

				var result = parser.Parse("((3 OR white) AND (6 LTE5)");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(21, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.UnexpectedOperand, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}

#endif
        [TestMethod]
		public void SimpleTestMissingCloseBraceInput()
		{
			ExpressionParser e = new ExpressionParser();

			try
			{
				var compiledExpression = e.Parse("(5+(3*4)");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(8, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.ClosingBraceExpected, ex.Cause);
				return;
			}

			Assert.Fail("Did not throw correct exception");
		}




		[TestMethod]
		public void SimpleTestLiteralStringApostrophe()
		{
			ExpressionParser e = new ExpressionParser();

			var compiledExpression = e.Parse("'hello there'");

			var evalResult = compiledExpression.Evaluate(null);
			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (string)evalResult.Pop().GetValue();
			Assert.AreEqual("hello there", actualResult);
		}


		[TestMethod]
		public void TestComma()
		{
			ExpressionParser e = new ExpressionParser();

			var compiledExpression = e.Parse("5, 6");

			var evalResult = compiledExpression.Evaluate(null);
			Assert.AreEqual(2, evalResult.Count);
			var second = (long)evalResult.Pop().GetValue();
			var first = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(5, first);
			Assert.AreEqual(6, second);
		}


		[TestMethod]
		public void SimpleTestLiteralStringsApostrophe()
		{
			ExpressionParser e = new ExpressionParser();

			var compiledExpression = e.Parse("'hello there', 'keith'");

			var evalResult = compiledExpression.Evaluate(null);
			Assert.AreEqual(2, evalResult.Count);
			var actualResult2 = (string)evalResult.Pop().GetValue();
			var actualResult1 = (string)evalResult.Pop().GetValue();
			Assert.AreEqual("hello there", actualResult1);
			Assert.AreEqual("keith", actualResult2);
		}


		[TestMethod]
		public void SimpleTestLiteralStringByQuote()
		{
			ExpressionParser e = new ExpressionParser();

			var compiledExpression = e.Parse("\"hello there\"");

			var evalResult = compiledExpression.Evaluate(null);
			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (string)evalResult.Pop().GetValue();
			Assert.AreEqual("hello there", actualResult);
		}

		[TestMethod]
		public void SimpleTestLiteralStringsByQuote()
		{
			ExpressionParser e = new ExpressionParser();

			var compiledExpression = e.Parse("\"hello there\", \"keith\"");

			var evalResult = compiledExpression.Evaluate(null);
			Assert.AreEqual(2, evalResult.Count);
			var actualResult2 = (string)evalResult.Pop().GetValue();
			var actualResult1 = (string)evalResult.Pop().GetValue();
			Assert.AreEqual("hello there", actualResult1);
			Assert.AreEqual("keith", actualResult2);
		}

		[TestMethod]
		public void SimpleTestLiteralStringsMixed()
		{
			ExpressionParser e = new ExpressionParser();

			var compiledExpression = e.Parse("\"hello there\", 'keith'");

			var evalResult = compiledExpression.Evaluate(null);
			Assert.AreEqual(2, evalResult.Count);
			var actualResult2 = (string)evalResult.Pop().GetValue();
			var actualResult1 = (string)evalResult.Pop().GetValue();
			Assert.AreEqual("hello there", actualResult1);
			Assert.AreEqual("keith", actualResult2);
		}

		[TestMethod]
		public void SimpleTestLiteralStringMismatchLeadingQuote()
		{
			ExpressionParser e = new ExpressionParser();

			try
			{
				var compiledExpression = e.Parse("\"hello there'");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(13, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.UnterminatedString, ex.Cause);
				return;
			}
		}

		[TestMethod]
		public void SimpleTestLiteralStringMismatchLeadingApostrophe()
		{
			ExpressionParser e = new ExpressionParser();

			try
			{
				var compiledExpression = e.Parse("'hello there\"");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(13, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.UnterminatedString, ex.Cause);
				return;
			}
		}


		// TODO: Write tests that evaluate expressions, including variables and functions.
		// TODO: Test that Exceptions are thrown for malformed input e.g. 5+3(6)
		// TODO: Test Exceptions are thrown for missing or incorrectly typed variables in expressions.









		//public static string Stringify(IList<TokenWrapper> result)
		//{
		//	StringBuilder sb = new StringBuilder();

		//	foreach(var tokenWrapper in result)
		//	{
		//		sb.Append(tokenWrapper.WrappedToken.ToString());
		//		sb.Append(' ');
		//	}
		//	string retVal = sb.ToString();
		//	Debug.WriteLine(retVal);
		//	return retVal;
		//}

		public static string Stringify(IList<IToken> result)
		{
			StringBuilder sb = new StringBuilder();

			foreach(var token in result)
			{
				sb.Append(token.ToString());
				sb.Append(' ');
			}
			string retVal = sb.ToString();
			Debug.WriteLine(retVal);
			return retVal;
		}

	}
}
