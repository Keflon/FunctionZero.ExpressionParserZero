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
	public class CastTests
	{
		[TestMethod]
		public void TestLongCastMinus()
		{
			ExpressionParser parser = new ExpressionParser();

			var compiledExpression = parser.Parse("(Long)5.2");

			Debug.WriteLine(compiledExpression.ToString());

			//Assert.AreEqual("5 UnaryMinus ", Stringify(result));

			long expectedResult = 5;
			var evalResult = compiledExpression.Evaluate(null);

			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (long)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}







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
