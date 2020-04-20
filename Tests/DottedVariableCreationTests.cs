using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class DottedVariableCreationTests
    {
        [TestMethod]
        public void TestDottedAssignment()
        {
            VariableSet vars = new VariableSet();
            VariableSet childVars = new VariableSet();


            vars.RegisterVariable(OperandType.VSet, "Child", childVars);

            vars.RegisterVariable(OperandType.Long, "Child.childLong", 6);

            var actualResultTemp = vars.GetVariable("Child.childLong").Value;
            var actualResult = (long)vars.GetVariable("Child.childLong").Value;

            long expectedResult = 6;

            Assert.AreEqual(expectedResult, actualResult);
        }


        [TestMethod]
        public void TestDottedAssignmentLong()
        {
            VariableSet vars = new VariableSet();
            VariableSet childVars = new VariableSet();


            vars.RegisterVariable(OperandType.VSet, "Child", childVars);

            vars.RegisterVariable(OperandType.Long, "Child.childLong", 6L);

            var actualResultTemp = vars.GetVariable("Child.childLong").Value;
            var actualResult = (long)vars.GetVariable("Child.childLong").Value;

            long expectedResult = 6;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestDoubleDottedAssignment()
        {
            VariableSet vars = new VariableSet();
            VariableSet childVars = new VariableSet();
            VariableSet grandchildVars = new VariableSet();

            vars.RegisterVariable(OperandType.VSet, "Child", childVars);
            vars.RegisterVariable(OperandType.VSet, "Child.Child", grandchildVars);
            vars.RegisterVariable(OperandType.Long, "Child.Child.childLong", 6);

            var actualResult = (long)vars.GetVariable("Child.Child.childLong").Value;

            long expectedResult = 6;

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
