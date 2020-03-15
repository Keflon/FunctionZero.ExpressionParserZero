using FunctionZero.ExpressionParserZero;
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

            childVarbs.RegisterLong("childLong", 6);

            var actualResult = (long)childVarbs.GetVariable("childLong").Value;

            long expectedResult = 6;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestSimpleDottedLookup()
        {
            VariableSet daddyVarbs = new VariableSet();
            VariableSet childVarbs = new VariableSet();

            daddyVarbs.RegisterLong("daddyLong", 5);
            childVarbs.RegisterLong("childLong", 6);
            childVarbs.RegisterVSet("daddy", daddyVarbs);

            var actualResult = (long)childVarbs.GetVariable("daddy.daddyLong").Value;

            long expectedResult = 5;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestDottedLookup()
        {
            ExpressionParser e = new ExpressionParser();
            ExpressionEvaluator ev = new ExpressionEvaluator();
            VariableSet daddyVarbs = new VariableSet();
            VariableSet childVarbs = new VariableSet();

            daddyVarbs.RegisterLong("daddyLong", 5);
            childVarbs.RegisterLong("childLong", 6);
            childVarbs.RegisterVSet("daddy", daddyVarbs);

            long expectedResult = 5 + 6;

            var result = e.Parse("childLong + daddy.daddyLong");

            var evalResult = ev.Evaluate(result, childVarbs);

            Assert.AreEqual(1, evalResult.Count);
            var actualResult = (long)evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestSimpleDottedAssignment()
        {
            VariableSet daddyVarbs = new VariableSet();
            VariableSet childVarbs = new VariableSet();

            daddyVarbs.RegisterLong("daddyLong", 5);
            childVarbs.RegisterLong("childLong", 6);
            childVarbs.RegisterVSet("daddy", daddyVarbs);

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
            ExpressionEvaluator ev = new ExpressionEvaluator();
            VariableSet daddyVarbs = new VariableSet();
            VariableSet childVarbs = new VariableSet();

            daddyVarbs.RegisterLong("daddyLong", 5);
            childVarbs.RegisterLong("childLongA", 6);
            childVarbs.RegisterLong("childLongB", 7);
            childVarbs.RegisterVSet("daddy", daddyVarbs);

            long expectedResult = 6 + 7;

            var result = e.Parse("daddy.daddyLong = childLongA + childLongB");

            var evalResult = ev.Evaluate(result, childVarbs);

            Assert.AreEqual(1, evalResult.Count);
            long actualResult = (long)daddyVarbs.GetVariable("daddyLong").Value;

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
