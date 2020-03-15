using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class VariableFactoryTests
    {
        [TestMethod]
        public void TestSpecializedBools()
        {
            ExpressionParser e = new ExpressionParser();
            ExpressionEvaluator ev = new ExpressionEvaluator();
            VariableSet variables = new VariableSet(new TestVariableFactory());

            variables.RegisterNullableBool("Left", true);
            variables.RegisterBool("Right", true);

            variables.SetVariableValue("Right", false);
            variables.SetVariableValue("Right", true);
            variables.SetVariableValue("Right", false);

            bool? left = true;
            bool right = false;

            bool expectedResult = left == right;

            var result = e.Parse("Left == Right");

            var evalResult = ev.Evaluate(result, variables);
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
                return new Variable(name, type, defaultValue);
        }
    }

    public class CrazyBool : Variable
    {
        internal long ChangeCount { get; private set; }

        public CrazyBool(string name, object defaultValue) : base(name, OperandType.Bool, defaultValue)
        {
            this.VariableChanged += BoolChanged;
        }

        private void BoolChanged(object sender, VariableChangedEventArgs variableChangedEventArgs)
        {
            ChangeCount++;
        }
    }
}
