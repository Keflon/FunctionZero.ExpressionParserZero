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

            vSet.UnregisterVariable(inScope);

            try
            {
                var variable = vSet.GetVariable("Horace");
                Assert.Fail("Did not throw expected exception");
            }
            catch (KeyNotFoundException knfex)
            {
            }
            catch
            {
                Assert.Fail("Did not throw expected exception");
            }
        }

        [TestMethod]
        public void UnregisterVariableByName()
        {
            VariableSet vSet = new VariableSet();

            Variable inScope = new Variable("Horace", OperandType.String, "I am hungry");

            vSet.RegisterVariable(inScope);

            Assert.IsTrue(vSet.GetVariable("Horace") == inScope);

            try
            {
                vSet.UnregisterVariable("Horace");
                var variable = vSet.GetVariable("Horace");
                Assert.Fail("Did not throw expected exception");
            }
            catch (KeyNotFoundException knfex)
            {
            }
            catch
            {
                Assert.Fail("Did not throw expected exception");
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
        public void UnregisterNestedVariable()
        {
            VariableSet vSet = new VariableSet();
            VariableSet childVset = new VariableSet();

            Variable inScope = new Variable("Horace", OperandType.String, "I am hungry");

            vSet.RegisterVariable(OperandType.VSet, "child", childVset);
            childVset.RegisterVariable(inScope);

            Assert.IsTrue(vSet.GetVariable("child.Horace") == inScope);

            vSet.UnregisterVariable("child.Horace");

            try
            {
                var variable = vSet.GetVariable("Horace");
                Assert.Fail("Did not throw expected exception");
            }
            catch (KeyNotFoundException knfex)
            {
            }
            catch
            {
                Assert.Fail("Did not throw expected exception");
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
            catch
            {
                Assert.Fail("Expected exception was not thrown");
            }
        }
    }
}
