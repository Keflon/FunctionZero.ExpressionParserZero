using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.Variables;
using System.Diagnostics;
using FunctionZero.ExpressionParserZero.Tokens;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Operands;

namespace ExpressionParserUnitTests
{
    /// <summary>
    /// Summary description for DocumentationSnippets
    /// </summary>
    [TestClass]
    public class DocumentationSnippets
    {

        [TestMethod]
        public void DocTest0()
        {
            ExpressionParser parser = new ExpressionParser();

            var compiledExpression = parser.Parse("(6+2)*5");
            Debug.WriteLine(compiledExpression.ToString());

            var resultStack = compiledExpression.Evaluate(null);

            Debug.WriteLine(TokenService.TokensAsString(resultStack));
            IOperand result = resultStack.Pop();
            Debug.WriteLine($"{result.Type}, {result.GetValue()}");
            long answer = (long)result.GetValue();
            Debug.WriteLine(answer);
        }

        [TestMethod]
        public void DocTest1()
        {
            ExpressionParser parser = new ExpressionParser();
            VariableSet vSet = new VariableSet();

            vSet.RegisterVariable(OperandType.Double, "a", 6);
            vSet.RegisterVariable(OperandType.Long, "b", 2);
            vSet.RegisterVariable(OperandType.Long, "c", 5);

            var compiledExpression = parser.Parse("(a+b)*c");
            Debug.WriteLine("Compiled expression: " + TokenService.TokensAsString(compiledExpression, terse: true));

            var resultStack = compiledExpression.Evaluate(vSet);
            Debug.WriteLine(TokenService.TokensAsString(resultStack));
            IOperand result = resultStack.Pop();
            Debug.WriteLine($"{result.Type}, {result.GetValue()}");
            double answer = (double)result.GetValue();
            Debug.WriteLine(answer);
        }

#if false
    public enum OperandType
    {
        Long = 0,
		NullableLong,
        Double,
		NullableDouble,
		String,
		Variable,
        Bool,
        NullableBool,
        VSet,
        Object, 
        
        Null
    }

#endif

        [TestMethod]
        public void AddTwoVariables()
        {
            ExpressionParser ep = new ExpressionParser();
            VariableSet vSet = new VariableSet();

            var rpnResult = ep.Parse("5+2");

            //var evaluatedResult = ee.Evaluate()
        }
    }
}
