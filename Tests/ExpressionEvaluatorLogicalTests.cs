using System;
using System.Collections.Generic;
using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Tokens;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParserUnitTests
{
	[TestClass]
	public class ExpressionEvaluatorLogicalTests
	{

		//[TestMethod]
		//public void TestSimpleLogicalExpression()
		//{ 
		//	Parser e = new Parser();
		//	ExpressionEvaluator ev = new ExpressionEvaluator();
		//	VariableSet variables = new VariableSet();

		//	variables.RegisterLong("Age", 42);
		//	variables.RegisterDouble("Opacity", 0.23);
		//	variables.RegisterString("Name", "Brian");

		//	double expectedResult = ((42 + 3) * (7 - 4) / 12 + -5) / 0.23;

		//	var result = e.Parse("((Age + 3)*(7 - 4)/12 + -5)/Opacity");

		//	var evalResult = ev.Evaluate(result, variables);
		//	Assert.AreEqual(1, evalResult.Count);
		//	var actualResult = (double)evalResult.Pop().GetValue();
		//	Assert.AreEqual(expectedResult, actualResult);

		//}


		#region Block0

		[TestMethod]
		public void TestSimpleLogicalOrBool()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", true);
			variables.RegisterBool("IsEnabled", false);

			bool expectedResult = true | false;

			var result = e.Parse("IsVisible | IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestSimpleLogicalOrBool2()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", false);
			variables.RegisterBool("IsEnabled", false);

			bool expectedResult = false;

			var result = e.Parse("IsVisible | IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestLogicalOrBoolWithAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", false);
			variables.RegisterBool("IsEnabled", false);
			variables.RegisterBool("Result", false);

			bool expectedResult = false;

			var result = e.Parse("Result = IsVisible | IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, variables.GetVariable("Result").Value);
			Assert.AreEqual(expectedResult, actualResult);
		}

		/// <summary>
		/// //////////////////////////////////////////////////////
		/// </summary>
		/// 

		[TestMethod]
		public void TestSimpleLogicalOrLong()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			long expectedResult = 42 | -402;

			var result = e.Parse("Var0 | Var1");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestSimpleLogicalOrLong2()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			long expectedResult = 42 | -402 | 9;

			var result = e.Parse("Var0 | Var1 | 9");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestLogicalOrLongWithAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -112);
			variables.RegisterLong("Result", -999);

			long expectedResult = 42 | -112 | 9;

			var result = e.Parse("Result = Var0 | Var1 | 9");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, variables.GetVariable("Result").Value);
			Assert.AreEqual(expectedResult, actualResult);
		}
		#endregion

		#region Block1

		[TestMethod]
		public void TestSimpleLogicalAndBool()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", true);
			variables.RegisterBool("IsEnabled", false);

			bool expectedResult = true & false;

			var result = e.Parse("IsVisible & IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestSimpleLogicalAndBool2()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", false);
			variables.RegisterBool("IsEnabled", false);

			bool expectedResult = false;

			var result = e.Parse("IsVisible & IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestLogicalOrAndWithAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", false);
			variables.RegisterBool("IsEnabled", false);
			variables.RegisterBool("Result", false);

			bool expectedResult = false;

			var result = e.Parse("Result = IsVisible & IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, variables.GetVariable("Result").Value);
			Assert.AreEqual(expectedResult, actualResult);
		}

		/// <summary>
		/// //////////////////////////////////////////////////////
		/// </summary>
		/// 

		[TestMethod]
		public void TestSimpleLogicalAndLong()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			long expectedResult = 42 & -402;

			var result = e.Parse("Var0 & Var1");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestSimpleLogicalAndLong2()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			long expectedResult = 42 & -402 & 9;

			var result = e.Parse("Var0 & Var1 & 9");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestLogicalAndLongWithAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -129);
			variables.RegisterLong("Result", -999);

			long expectedResult = 42 & -129 & 9;

			var result = e.Parse("Result = Var0 & Var1 & 9");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, variables.GetVariable("Result").Value);
			Assert.AreEqual(expectedResult, actualResult);
		}



		[TestMethod]
		public void TestComplexLogicalOperators()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("L0", 42);
			variables.RegisterLong("L1", -129);
			variables.RegisterLong("L2", -475);
			variables.RegisterBool("B0", false);
			variables.RegisterBool("B1", true);
			variables.RegisterBool("B2", true);
			variables.RegisterBool("Result", false);

			long l0 = 42, l1 = -129, l2 = -475;
			bool b0 = false, b1 = true, b2 = true;

			bool expectedResult = b0 | (b1 & b2) == true & ((l0 & l1) > 11);

			var result = e.Parse("Result = B0 | (B1 & B2) == True & ( (L0 & L1) > 11)");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, variables.GetVariable("Result").Value);
			Assert.AreEqual(expectedResult, actualResult);
		}








		#endregion

		#region Block2


		[TestMethod]
		public void TestSimpleLogicalComplementLong()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			long expectedResult = 42 + ~-402;

			var result = e.Parse("Var0 +~ Var1");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestSimpleLogicalComplementLong2()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			long expectedResult = ~42 + ~-402 - ~9;

			var result = e.Parse("~Var0 + ~Var1 - ~9");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestLogicalAndComplementWithAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -129);
			variables.RegisterLong("Result", -999);

			long expectedResult = 42 ^ ~-129 ^ ~9;

			var result = e.Parse("Result = Var0 ^~ Var1 ^~ 9");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, variables.GetVariable("Result").Value);
			Assert.AreEqual(expectedResult, actualResult);
		}

		#endregion

		#region Block3

		[TestMethod]
		public void TestSimpleLogicalXorBool()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", true);
			variables.RegisterBool("IsEnabled", false);

			bool expectedResult = true ^ false;

			var result = e.Parse("IsVisible ^ IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestSimpleLogicalComplementBool2()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", false);
			variables.RegisterBool("IsEnabled", false);

			bool expectedResult = false ^ false;

			var result = e.Parse("IsVisible ^ IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestSimpleLogicalComplementBool3()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", true);
			variables.RegisterBool("IsEnabled", true);

			bool expectedResult = true ^ true;

			var result = e.Parse("IsVisible ^ IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		/// <summary>
		/// //////////////////////////////////////////////////////
		/// </summary>
		/// 

		[TestMethod]
		public void TestSimpleLogicalXorLong()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			long expectedResult = 42 ^ -402;

			var result = e.Parse("Var0 ^ Var1");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestSimpleLogicalXorLong2()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			long expectedResult = 42 ^ -402 ^ 9;

			var result = e.Parse("Var0 ^ Var1 ^ 9");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestLogicalAndXorWithAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -129);
			variables.RegisterLong("Result", -999);

			long expectedResult = 42 ^ -129 ^ 9;

			var result = e.Parse("Result = Var0 ^ Var1 ^ 9");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, variables.GetVariable("Result").Value);
			Assert.AreEqual(expectedResult, actualResult);
		}

		#endregion

		#region Block4

		[TestMethod]
		public void TestSimpleLogicalNotBool()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", true);
			variables.RegisterBool("IsEnabled", false);

			bool expectedResult = true | !false;

			var result = e.Parse("IsVisible |! IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestSimpleLogicalNotBool2()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", false);
			variables.RegisterBool("IsEnabled", false);

			bool expectedResult = false | !false;

			var result = e.Parse("IsVisible |! IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestLogicalNotBoolWithAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterBool("IsVisible", false);
			variables.RegisterBool("IsEnabled", false);
			variables.RegisterBool("Result", false);

			bool expectedResult = false | !false;

			var result = e.Parse("Result = IsVisible |! IsEnabled");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			bool actualResult = (bool)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, variables.GetVariable("Result").Value);
			Assert.AreEqual(expectedResult, actualResult);
		}

		/// <summary>
		/// //////////////////////////////////////////////////////
		/// </summary>
		/// 

		[TestMethod]
		public void TestSimpleLogicalNotLong()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			try
			{
				var result = e.Parse("Var0 |! Var1");
				var evalResult = ev.Evaluate(result, variables);
			}
			catch(ExpressionEvaluatorException ex)
			{
				Assert.AreEqual(6, ex.Offset);
				Assert.AreEqual(ExpressionEvaluatorException.ExceptionCause.BadUnaryOperand, ex.Cause);
				return;
			}
			Assert.Fail("Did not throw correct exception");
		}

		#endregion

		#region Block5
		
		[TestMethod]
		public void TestSimpleModuloLong()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			long expectedResult = 42 % -402;

			var result = e.Parse("Var0 % Var1");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestSimpleModuloLong2()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -402);

			long expectedResult = ~42 % -402 % ~9;

			var result = e.Parse("~Var0 % ~Var1 % ~9");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestModuloLongWithAssignment()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Var0", 42);
			variables.RegisterLong("Var1", -129);
			variables.RegisterLong("Result", -999);

			long expectedResult = 42 % ~-129 % ~9;

			var result = e.Parse("Result = Var0 %~ Var1 %~ 9");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			long actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, variables.GetVariable("Result").Value);
			Assert.AreEqual(expectedResult, actualResult);
		}

		#endregion
		
		[TestMethod]
		public void SimpleTestUnmatchedBraceInput()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();

			try
			{
				var result = e.Parse("55)");
				var evalResult = ev.Evaluate(result, null);
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(2, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.UnmatchedClosingBrace, ex.Cause);
				return;
			}

			Assert.Fail("Did not throw correct exception");
		}


	}
}
