using System;
using System.Collections.Generic;
using System.Diagnostics;
using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Tokens;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParserUnitTests
{
	[TestClass]
	public class ExpressionEvaluatorTests
	{

		[TestMethod]
		public void TestTypeFail()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.Long, "Age", 42);
			variables.RegisterVariable(OperandType.String, "Name", "Brian");

			var compiledExpression = e.Parse("Age * Name");

			try
			{
				var evalResult = compiledExpression.Evaluate(variables);
			}
			catch(ExpressionEvaluatorException ex)
			{
				Assert.AreEqual(ex.Cause, ExpressionEvaluatorException.ExceptionCause.BadOperand);
				Assert.AreEqual(ex.Offset, 4); 
			}
		}

		[TestMethod]
		public void TestUnaryTypeFail()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.String, "Name", "Brian");

			var compiledExpression = e.Parse("!Name");

			try
			{
				var evalResult = compiledExpression.Evaluate(variables);
				Assert.Fail("Did not throw correct exception");
			}
			catch(ExpressionEvaluatorException ex)
			{
				Assert.AreEqual(ex.Cause, ExpressionEvaluatorException.ExceptionCause.BadUnaryOperand);
				Assert.AreEqual(ex.Offset, 0);
			}
		}

		[TestMethod]
		public void TestTrueAsBool()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.String, "Name", "Brian");

			var compiledExpression = e.Parse("true");
			bool expectedResult = true;
			var evalResult = compiledExpression.Evaluate(variables);

			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestCompareToNull()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.String, "Name", "Brian");

			var compiledExpression = e.Parse("Name != null");
			bool expectedResult = true;
			var evalResult = compiledExpression.Evaluate(variables);

			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestUnknownVariable()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.String, "Name", "Brian");

			var compiledExpression = e.Parse("Banana");
			string expectedResult = "Banana";
			var evalResult = compiledExpression.Evaluate(variables);
			// TODO: THINK: Should this throw an 'unknown variable exception'. Nothing is trying to resolve the variable, therefore
			// TODO: it is probably correct to return the actual variable rather than attempting to resolve it. 
			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (string)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}


		[TestMethod]
		public void TestComplexExpression()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.Long, "Age", 42);
			variables.RegisterVariable(OperandType.Double, "Opacity", 0.23);
			variables.RegisterVariable(OperandType.String, "Name", "Brian");

			double expectedResult = ((42 + 3) * (7 - 4) / 12 + -5) / 0.23;

			var compiledExpression = e.Parse("((Age + 3)*(7 - 4)/12 + -5)/Opacity");

			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (double)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);

		}

		[TestMethod]
		public void TestNestedFunctionCalls()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.Long, "Age", 42);
			variables.RegisterVariable(OperandType.Double, "Opacity", 0.23);
			variables.RegisterVariable(OperandType.String, "Name", "Brian");

			double expectedResult = (mul(42 + 3, mul(9, 6)) * mul(7 - 4, mul(5, 6)) / 12 + -5) / 0.23;

			var compiledExpression = e.Parse("(_debug_mul(Age + 3, _debug_mul(9,6))*_debug_mul(7 - 4, _debug_mul(5,6))/12 + -5)/Opacity");

			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (double)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}





		[TestMethod]

		public void TestVariableValue()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();
			variables.RegisterVariable(OperandType.String, "Banana", "Hello Banana");

			var compiledExpression = e.Parse("Banana");
			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(1, evalResult.Count);
			var actualResult = OperatorActions.PopAndResolve(evalResult, variables).GetValue();
			Assert.AreEqual("Hello Banana", actualResult);
		}


		[TestMethod]
		public void TestMissingVariable()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			var compiledExpression = e.Parse("Banana");
			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(1, evalResult.Count);
			try
			{
				var actualResult = OperatorActions.PopAndResolve(evalResult, variables).GetValue();
			}
			catch(ExpressionEvaluatorException ex)
			{
				Assert.AreEqual(ex.Cause, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable);
				Assert.AreEqual(ex.Offset, 0);

				return;
			}
		}

		[TestMethod]
		public void TestMissingVariable2()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.String, "Banana", "Hello Banana!");

			var compiledExpression = e.Parse("Banana + Feet");
			try
			{
				var evalResult = compiledExpression.Evaluate(variables);
			}
			catch(ExpressionEvaluatorException ex)
			{
				Assert.AreEqual(ex.Cause, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable);
				Assert.AreEqual(ex.Offset, 9);

				return;
			}
		}

		/// <summary>
		/// Tests that Banana can resolve but Melon cannot.
		/// Tests that the correct parser position for Melon is recorded.
		/// </summary>
		[TestMethod]
		public void TestMissingVariable3()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.String, "Banana", "Hello Banana!");

			IOperand actualResult1 = null;
			Stack<IOperand> evalResult = null;

			bool shortedOut = true;
			var compiledExpression = e.Parse("Banana , Melon");

			try
			{
				evalResult = compiledExpression.Evaluate(variables);
				Assert.AreEqual(2, evalResult.Count);
				shortedOut = false;
				actualResult1 = OperatorActions.PopAndResolve(evalResult, variables);
			}
			catch(ExpressionEvaluatorException ex)
			{
				Assert.AreEqual(shortedOut, false);
				Assert.AreEqual(actualResult1, null);

				var actualResult2 = OperatorActions.PopAndResolve(evalResult, variables).GetValue();
				Assert.AreEqual((string)actualResult2, "Hello Banana!");
				Assert.AreEqual(ex.Cause, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable);
				Assert.AreEqual(ex.Offset, 9);
				Assert.AreEqual(ex.Message, "'Melon'");
			}
		}

		[TestMethod]
		public void TestSimpleAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.Long, "Age", 42);
			var compiledExpression = e.Parse("Age=9");
			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (long)evalResult.Pop().GetValue();
			var variable = variables.GetVariable("Age");

			Assert.AreEqual(OperandType.Long, (variable.VariableType));
			Assert.AreEqual(9, (long)(variable.Value));
			Assert.AreEqual(9, actualResult);
		}


		[TestMethod]
		public void TestAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.Long, "Age", 42);
			variables.RegisterVariable(OperandType.Double, "Opacity", 4.2);
			variables.RegisterVariable(OperandType.Double, "Answer", 4.2);

			var compiledExpression = e.Parse("Answer=5+(((Age*Opacity)+6)*9.771)-2");

			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (double)evalResult.Pop().GetValue();
			var expectedResult = 5 + (((42 * 4.2) + 6) * 9.771) - 2;
			var variable = variables.GetVariable("Answer");
			Assert.AreEqual(OperandType.Double, (variable.VariableType));
			Assert.AreEqual(expectedResult, (double)(variable.Value));
			Assert.AreEqual(expectedResult, actualResult);
		}


		[TestMethod]
		public void TestDoubleAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.Double, "Answer1", 4.2);
			variables.RegisterVariable(OperandType.Double, "Answer2", 4.2);

			var compiledExpression = e.Parse("Answer1=5.1,Answer2=6.2");

			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(2, evalResult.Count);
			var actualResult2 = (double)evalResult.Pop().GetValue();
			var actualResult1 = (double)evalResult.Pop().GetValue();

			var variable1 = variables.GetVariable("Answer1");
			var variable2 = variables.GetVariable("Answer2");
			var answer1 = 5.1;
			var answer2 = 6.2;

			Assert.AreEqual(OperandType.Double, (variable1.VariableType));
			Assert.AreEqual(OperandType.Double, (variable2.VariableType));
			Assert.AreEqual(answer1, (double)(variable1.Value));
			Assert.AreEqual(answer2, (double)(variable2.Value));
			Assert.AreEqual(answer1, actualResult1);
			Assert.AreEqual(answer2, actualResult2);
		}


		[TestMethod]
		public void TestDoubleAssignmentSecondToFirst()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.Double, "Answer1", 4.2);
			variables.RegisterVariable(OperandType.Double, "Answer2", 4.2);

			var compiledExpression = e.Parse("Answer1=5.1,Answer2=Answer1");

			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(2, evalResult.Count);
			var actualResult2 = (double)evalResult.Pop().GetValue();
			var actualResult1 = (double)evalResult.Pop().GetValue();

			var variable1 = variables.GetVariable("Answer1");
			var variable2 = variables.GetVariable("Answer2");
			var answer1 = 5.1;
			var answer2 = 5.1;

			Assert.AreEqual(OperandType.Double, (variable1.VariableType));
			Assert.AreEqual(OperandType.Double, (variable2.VariableType));
			Assert.AreEqual(answer1, (double)(variable1.Value));
			Assert.AreEqual(answer2, (double)(variable2.Value));
			Assert.AreEqual(answer1, actualResult1);
			Assert.AreEqual(answer2, actualResult2);
		}

		[TestMethod]
		public void TestDoubleAssignmentSecondToSelf()
		{
			ExpressionParser e = new ExpressionParser();
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.Long, "Age", 42);
			variables.RegisterVariable(OperandType.Double, "Opacity", 4.2);
			variables.RegisterVariable(OperandType.Double, "Answer", 4.2);

			var compiledExpression = e.Parse("Answer=5.1,Answer=Answer");

			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(2, evalResult.Count);
			var actualResult2 = evalResult.Pop();
			var actualResult1 = evalResult.Pop();

			var variable1 = variables.GetVariable("Answer");
			var answer1 = 5.1;
			var answer2 = 5.1;

			Assert.AreEqual(OperandType.Double, (variable1.VariableType));
			Assert.AreEqual(answer1, (double)(variable1.Value));
			Assert.AreEqual(answer1, actualResult1.GetValue());
			Assert.AreEqual(answer2, actualResult2.GetValue());
		}


		[TestMethod]
		public void TestNestedParenthesis()
		{
			ExpressionParser e = new ExpressionParser();

			double expectedResult = ((((2 + 3) * 3) + 4) * 5) + 5;

			var compiledExpression = e.Parse("((((2+3)*3)+4)*5)+5");

			var evalResult = compiledExpression.Evaluate(null);
			Assert.AreEqual(1, evalResult.Count);

			Assert.AreEqual(expectedResult, (long)evalResult.Pop().GetValue());
		}

		long mul(long a, long b)
		{
			return (a * b);
		}

		[TestMethod]
		public void TestMatchSimpleString()
		{
			ExpressionParser e = new ExpressionParser();
			e.RegisterFunction("StringContains", DoStringContains, 2);
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.String, "Name", "This is a test");
			variables.RegisterVariable(OperandType.String, "SubString", "is");

			var compiledExpression = e.Parse("StringContains(Name, SubString)");
			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(1, evalResult.Count);
			Assert.AreEqual(true, (bool)evalResult.Pop().GetValue());

		}

		[TestMethod]
		public void TestMisMatchSimpleString()
		{
			ExpressionParser e = new ExpressionParser();
			e.RegisterFunction("StringContains", DoStringContains, 2);
			VariableSet variables = new VariableSet();

			variables.RegisterVariable(OperandType.String, "Name", "This is a test");
			variables.RegisterVariable(OperandType.String, "SubString", "pis");

			var compiledExpression = e.Parse("StringContains(Name, SubString)");

			var evalResult = compiledExpression.Evaluate(variables);
			Assert.AreEqual(1, evalResult.Count);
			Assert.AreEqual(false, (bool)evalResult.Pop().GetValue());

		}

		[TestMethod]
		public void SimpleTestMissingOperatorInput()
		{
			ExpressionParser e = new ExpressionParser();

			try
			{
				var compiledExpression = e.Parse("5(");
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
		public void SimpleTestUnmatchedBraceInput()
		{
			ExpressionParser e = new ExpressionParser();

			try
			{
				var compiledExpression = e.Parse("55)");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(2, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.UnmatchedClosingBrace, ex.Cause);
				return;
			}

			Assert.Fail("Did not throw correct exception");
		}

		[TestMethod]
		public void SimpleTestDoubleDotInput()
		{
			ExpressionParser e = new ExpressionParser();

			try
			{
				var result = e.Parse("..");
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(1, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.MultipleDecimalPoint, ex.Cause);
				return;
			}

			Assert.Fail("Did not throw correct exception");
		}



#if false
		#region Registered alphanumeric operators

        [TestMethod]
		public void SimpleTestRegisteredAlphanumericOperators()
		{
			Parser parser = new Parser();
			ExpressionEvaluator ev = new ExpressionEvaluator();

			parser.RegisterOperator("AND", 4, OperatorActions.DoLogicalAnd);
			parser.RegisterOperator("OR", 3, OperatorActions.DoLogicalOr);
			parser.RegisterOperator("LT", 9, OperatorActions.DoLessThan);
			parser.RegisterOperator("GT", 9, OperatorActions.DoGreaterThan);
			parser.RegisterOperator("LTE", 9, OperatorActions.DoLessOrEqual);
			parser.RegisterOperator("GTE", 9, OperatorActions.DoGreaterOrEqual);

			try
			{
				var result = parser.Parse("3 AND 5");
				var evalResult = ev.Evaluate(result, null);
			}
			catch(ExpressionEvaluatorException ex)
			{
				Assert.AreEqual(2, ex.Offset);
				Assert.AreEqual(ExpressionEvaluatorException.ExceptionCause.BadOperand, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}

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

			ExpressionEvaluator ev = new ExpressionEvaluator();

			var result = parser.Parse("(3 OR white) AND (6 LTE purple)");
			//Assert.AreEqual("3 white OR 6 purple LTE AND ", Stringify(result));
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
			//Assert.AreEqual("3 white OR LTE5 LTE6 LTE AND ", Stringify(result));
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

		#endregion
#endif



		private void DoStringContains(Stack<IOperand> operands, IBackingStore variables, long parserPosition)
		{
			IOperand second = OperatorActions.PopAndResolve(operands, variables);
			IOperand first = OperatorActions.PopAndResolve(operands, variables);
			operands.Push(new Operand(-1, OperandType.Bool, ((string)first.GetValue()).Contains((string)second.GetValue())));
		}


		public enum Borg
		{ A, B, C, D, E }

		public enum Berg
		{ A, B, C, D, E }

		public void EnumTest0(Borg b)
		{
			EnumTest1(b);
			EnumTest1(Borg.B);
			EnumTest1(Berg.B);
		}


		public void EnumTest1(Enum b)
		{
			var r = b.HasFlag(Borg.C);
		}



	}
}
