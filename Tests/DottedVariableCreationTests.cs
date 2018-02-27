using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            VariableSet varbs = new VariableSet();
            VariableSet childVarbs = new VariableSet();

            varbs.RegisterVSet("Child", childVarbs);

            varbs.RegisterLong("Child.childLong", 6);

            var actualResult = (long)varbs.GetVariable("Child.childLong").Value;

            long expectedResult = 6;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TestDoubleDottedAssignment()
        {
            VariableSet varbs = new VariableSet();
            VariableSet childVarbs = new VariableSet();
            VariableSet grandchildVarbs = new VariableSet();

            varbs.RegisterVSet("Child", childVarbs);
            varbs.RegisterVSet("Child.Child", grandchildVarbs);
            varbs.RegisterLong("Child.Child.childLong", 6);

            var actualResult = (long)varbs.GetVariable("Child.Child.childLong").Value;

            long expectedResult = 6;

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
