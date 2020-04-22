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
        public DocumentationSnippets()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void AddTwoLongs()
        {
            ExpressionParser parser = new ExpressionParser();
            ExpressionEvaluator evaluator = new ExpressionEvaluator();

            IList<IToken> compiledExpression = parser.Parse("5+2");
            Debug.WriteLine("Compiled expression: " + TokenService.TokensAsString(compiledExpression));

            var evaluatedResult = evaluator.Evaluate(compiledExpression, null);
            Debug.WriteLine("Results Stack: " + TokenService.TokensAsString(evaluatedResult));

            long answer = (long)evaluatedResult.Pop().GetValue();
            Debug.WriteLine(answer);
        }

        [TestMethod]
        public void DocTest0()
        {
            ExpressionParser parser = new ExpressionParser();
            ExpressionEvaluator evaluator = new ExpressionEvaluator();

            IList<IToken> postfix = parser.Parse("(6+2)*5");
            Debug.WriteLine("Compiled expression: " + TokenService.TokensAsString(postfix, terse: true));

            var resultStack = evaluator.Evaluate(postfix, null);
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
            ExpressionEvaluator evaluator = new ExpressionEvaluator();
            VariableSet vSet = new VariableSet();

            vSet.RegisterVariable(OperandType.Double, "a", 6);
            vSet.RegisterVariable(OperandType.Long, "b", 2);
            vSet.RegisterVariable(OperandType.Long, "c", 5);

            IList<IToken> postfix = parser.Parse("(a+b)*c");
            Debug.WriteLine("Compiled expression: " + TokenService.TokensAsString(postfix, terse: true));

            var resultStack = evaluator.Evaluate(postfix, vSet);
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
            ExpressionEvaluator ee = new ExpressionEvaluator();
            VariableSet vSet = new VariableSet();

            var rpnResult = ep.Parse("5+2");

            //var evaluatedResult = ee.Evaluate()
        }
    }
}
