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
            var ep = new ExpressionParser();
            var compiledExpression = ep.Parse("(6+2)*5");
            Debug.WriteLine(compiledExpression.ToString());
        }
        /*
        Output:
        [Operand:6][Operand:2][Operator:+][Operand:5][Operator:*]
        */

        [TestMethod]
        public void DocTest1()
        {
            ExpressionParser parser = new ExpressionParser();

            // Parse ...
            var compiledExpression = parser.Parse("(6+2)*5");
            Debug.WriteLine(compiledExpression.ToString());

            // Evaluate ...
            var resultStack = compiledExpression.Evaluate(null);
            Debug.WriteLine(resultStack.ToString());

            // Pop the result from the stack...
            IOperand result = resultStack.Pop();
            Debug.WriteLine($"{result.Type}, {result.GetValue()}");
            int answer = (int)result.GetValue();
            Debug.WriteLine(answer);
        }
        /*
        Output:
        [Operand:6][Operand:2][Operator:+][Operand:5][Operator:*]
        [Operand:40]
        Long, 40
        40
        */

        [TestMethod]
        public void DocTest2()
        {
            ExpressionParser parser = new ExpressionParser();

            // Parse ...
            var compiledExpression = parser.Parse("(6+2)*5");
            Debug.WriteLine(compiledExpression.ToString());

            // Evaluate ...
            var resultStack = compiledExpression.Evaluate(null);
            Debug.WriteLine(resultStack.ToString());

            // Pop the result from the stack...
            IOperand result = resultStack.Pop();
            Debug.WriteLine($"{result.Type}, {result.GetValue()}");
            int answer = (int)result.GetValue();
            Debug.WriteLine(answer);
        }
        /*
        Output:
        [Operand:6][Operand:2][Operator:+][Operand:5][Operator:*]
        [Operand:40]
        Long, 40
        40
        */


        [TestMethod]
        public void DocTest3()
        {
            // Parse ...
            ExpressionParser parser = new ExpressionParser();
            var compiledExpression = parser.Parse("(cabbages+onions)*bananas");
            Debug.WriteLine(compiledExpression.ToString());

            // Variables ...
            VariableSet vSet = new VariableSet();
            vSet.RegisterVariable(OperandType.Double, "cabbages", 6);
            vSet.RegisterVariable(OperandType.Long, "onions", 2);
            vSet.RegisterVariable(OperandType.Long, "bananas", 5);

            // Evaluate ...
            var resultStack = compiledExpression.Evaluate(vSet);
            Debug.WriteLine(TokenService.TokensAsString(resultStack));

            // Result ...
            IOperand result = resultStack.Pop();
            Debug.WriteLine($"{result.Type}, {result.GetValue()}");
            double answer = (double)result.GetValue();
            Debug.WriteLine(answer);
        }
    }
}
