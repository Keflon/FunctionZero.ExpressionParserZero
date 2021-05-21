using System;
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
        public void TestLongCast()
        {
            ExpressionParser parser = new ExpressionParser();

            var compiledExpression = parser.Parse("(Long)5.2");

            Debug.WriteLine(compiledExpression.ToString());

            //Debug.WriteLine(Stringify(compiledExpression));
            Debug.WriteLine(TokenService.TokensAsString(compiledExpression.RpnTokens));
            //Assert.AreEqual("5 UnaryMinus ", Stringify(result));

            long expectedResult = 5;
            var evalResult = compiledExpression.Evaluate(null);

            Assert.AreEqual(1, evalResult.Count);
            var actualResult = (long)evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestMethod]
        public void TestBoolCast()
        {
            ExpressionParser parser = new ExpressionParser();

            try
            {
                var compiledExpression = parser.Parse("(Bool)5.2");
                var evalResult = compiledExpression.Evaluate(null);
            }
            catch (ExpressionEvaluatorException eex)
            {
                Assert.AreEqual(ExpressionEvaluatorException.ExceptionCause.BadUnaryOperand, eex.Cause);
            }
        }

        [TestMethod]
        public void TestBoolCastNullToBool()
        {
            ExpressionParser parser = new ExpressionParser();

            try
            {
                var compiledExpression = parser.Parse("(Bool)null");
                var evalResult = compiledExpression.Evaluate(null);
            }
            catch (ExpressionEvaluatorException eex)
            {
                Assert.AreEqual(ExpressionEvaluatorException.ExceptionCause.BadUnaryOperand, eex.Cause);
            }
        }


        [TestMethod]
        public void TestBoolCasta()
        {
            ExpressionParser parser = new ExpressionParser();

            var compiledExpression = parser.Parse("(NullableBool)null");
            var evalResult = compiledExpression.Evaluate(null);

            object expectedResult = null;

            Assert.AreEqual(1, evalResult.Count);
            var actualResult = evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestLongCastWithUnaryPlus()
        {
            ExpressionParser parser = new ExpressionParser();

            var compiledExpression = parser.Parse("(Long)+5.2");
            var evalResult = compiledExpression.Evaluate(null);

            long expectedResult = 5;

            Assert.AreEqual(1, evalResult.Count);
            var actualResult = evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestUnaryPlus()
        {
            ExpressionParser parser = new ExpressionParser();

            var compiledExpression = parser.Parse("+5.2");
            var evalResult = compiledExpression.Evaluate(null);

            double expectedResult = 5.2;

            Assert.AreEqual(1, evalResult.Count);
            var actualResult = evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);
        }







        //public static string Stringify(IList<IToken> result)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    foreach (var token in result)
        //    {
        //        sb.Append(token.ToString());
        //        sb.Append(' ');
        //    }
        //    string retVal = sb.ToString();
        //    Debug.WriteLine(retVal);
        //    return retVal;
        //}

    }
}
