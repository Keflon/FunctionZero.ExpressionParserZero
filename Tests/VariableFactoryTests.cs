using FunctionZero.ExpressionParserZero;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Variables;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class VariableFactoryTests
    {
        [TestMethod]
        public void TestSpecializedBools()
        {
            ExpressionParser e = new ExpressionParser();
            VariableSet variables = new VariableSet(null, new TestVariableFactory());

            variables.RegisterVariable(OperandType.NullableBool, "Left", true);
            variables.RegisterVariable(OperandType.Bool, "Right", true);

            variables.SetVariableValue("Right", false);
            variables.SetVariableValue("Right", true);
            variables.SetVariableValue("Right", false);

            bool? left = true;
            bool right = false;

            bool expectedResult = left == right;

            var compiledExpression = e.Parse("Left == Right");

            var evalResult = compiledExpression.Evaluate(variables);
            Assert.AreEqual(1, evalResult.Count);
            bool actualResult = (bool)evalResult.Pop().GetValue();
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(3, ((CrazyBool)variables.GetVariable("Right")).ChangeCount);
        }
    }

    public class TestVariableFactory : IVariableFactory
    {
        public Variable CreateVariable(string name, OperandType type, object defaultValue, object state)
        {
            if (type == OperandType.Bool)
                return new CrazyBool(name, defaultValue);
            else
                return new Variable(name, type, defaultValue, state);
        }
    }

    public class CrazyBool : Variable
    {
        internal long ChangeCount { get; private set; }

        public CrazyBool(string name, object defaultValue) : base(name, OperandType.Bool, defaultValue, null)
        {
            this.VariableChanged += BoolChanged;
        }

        private void BoolChanged(object sender, VariableChangedEventArgs variableChangedEventArgs)
        {
            ChangeCount++;
        }
    }
}
