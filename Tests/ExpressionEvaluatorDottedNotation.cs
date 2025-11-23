using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class ExpressionEvaluatorDottedNotation
    {
        [TestMethod]
        public void TestSimpleLookup()
        {
            VariableSet childVarbs = new VariableSet();

            childVarbs.RegisterVariable(OperandType.Long, "childLong", 6);

            var actualResult = (long)childVarbs.GetVariable("childLong").Value;

            long expectedResult = 6;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestSimpleDottedLookup()
        {
            VariableSet daddyVarbs = new VariableSet();
            VariableSet childVarbs = new VariableSet();

            daddyVarbs.RegisterVariable(OperandType.Long, "daddyLong", 5);
            childVarbs.RegisterVariable(OperandType.Long, "childLong", 6);
            childVarbs.RegisterVariable(OperandType.VSet, "daddy", daddyVarbs);

            var actualResult = (long)childVarbs.GetVariable("daddy.daddyLong").Value;

            long expectedResult = 5;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestDottedLookup()
        {
            ExpressionParser e = new ExpressionParser();
            VariableSet daddyVarbs = new VariableSet();
            VariableSet childVarbs = new VariableSet();
            
            daddyVarbs.RegisterVariable(OperandType.Long, "daddyLong", 5);
            childVarbs.RegisterVariable(OperandType.Long, "childLong", 6);
            childVarbs.RegisterVariable(OperandType.VSet, "daddy", daddyVarbs);

            long expectedResult = 5 + 6;

            var compiledExpression = e.Parse("childLong + daddy.daddyLong");

            var evalResult = compiledExpression.Evaluate(childVarbs);

            Assert.AreEqual(1, evalResult.Count);
            var actualResult = (long)evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestSimpleDottedAssignment()
        {
            VariableSet daddyVarbs = new VariableSet();
            VariableSet childVarbs = new VariableSet();

            daddyVarbs.RegisterVariable(OperandType.Long, "daddyLong", 5);
            childVarbs.RegisterVariable(OperandType.Long, "childLong", 6);
            childVarbs.RegisterVariable(OperandType.VSet, "daddy", daddyVarbs);

            childVarbs.SetVariableValue("daddy.daddyLong", (long)42);
            var actualResult = (long)childVarbs.GetVariable("daddy.daddyLong").Value;
            var actualResult2 = (long)daddyVarbs.GetVariable("daddyLong").Value;

            long expectedResult = 42;

            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedResult, actualResult2);
        }


        [TestMethod]
        public void TestDottedAssignment()
        {
            ExpressionParser e = new ExpressionParser();
            VariableSet daddyVarbs = new VariableSet();
            VariableSet childVarbs = new VariableSet();

            daddyVarbs.RegisterVariable(OperandType.Long, "daddyLong", 5);
            childVarbs.RegisterVariable(OperandType.Long, "childLongA", 6);
            childVarbs.RegisterVariable(OperandType.Long, "childLongB", 7);
            childVarbs.RegisterVariable(OperandType.VSet, "daddy", daddyVarbs);

            long expectedResult = 6 + 7;

            var compiledExpression = e.Parse("daddy.daddyLong = childLongA + childLongB");

            var evalResult = compiledExpression.Evaluate(childVarbs);

            Assert.AreEqual(1, evalResult.Count);
            long actualResult = (long)daddyVarbs.GetVariable("daddyLong").Value;

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
