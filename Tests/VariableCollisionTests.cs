using System;
using System.Collections.Generic;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class VariableCollisionTests
    {
        [TestMethod]
        public void UnregisterVariable()
        {
            VariableSet vSet = new VariableSet();

            Variable inScope = new Variable("Horace", OperandType.String, "I am hungry");

            vSet.RegisterVariable(inScope);

            Assert.IsTrue(vSet.GetVariable("Horace") == inScope);

            try
            {
                vSet.UnregisterVariable(inScope);
                var variable = vSet.GetVariable("Horace");

            }
            catch (KeyNotFoundException knfex)
            {
            }
        }

        [TestMethod]
        public void UnregisterMissingVariable()
        {
            VariableSet vSet = new VariableSet();

            Variable inScope = new Variable("Horace", OperandType.String, "I am hungry");

            vSet.RegisterVariable(inScope);

            Assert.IsTrue(vSet.GetVariable("Horace") == inScope);

            try
            {
                vSet.UnregisterVariable(inScope);

            }
            catch (KeyNotFoundException knfex)
            {

            }
        }

        [TestMethod]
        public void UnregisterWrongVariable()
        {
            VariableSet vSet = new VariableSet();

            Variable inScope = new Variable("Horace", OperandType.String, "I am hungry");
            Variable outScope = new Variable("Horace", OperandType.String, "I am not hungry");

            vSet.RegisterVariable(inScope);

            try
            {
                vSet.UnregisterVariable(outScope);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (ValueNotFoundException vnfex)
            {

            }
        }
    }
}
