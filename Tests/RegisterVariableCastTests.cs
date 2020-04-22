using System;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class RegisterVariableCastTests
    {
        [TestMethod]
        public void CastTest()
        {
            VariableSet vSet = new VariableSet();

            // Poke one of each of these into a variable of every type and see what comes back.
            // long, int, short, char, byte, double, float, string, Variable, bool, VSet, null

            var testVariable = new Variable("Dave", OperandType.Bool, false);

            TestRegister(vSet, OperandType.Long, (long)5, (long)5, true);
            TestRegister(vSet, OperandType.Long, (int)5, (long)5, true);
            TestRegister(vSet, OperandType.Long, (short)5, (long)5, true);
            TestRegister(vSet, OperandType.Long, (char)5, (long)5, true);
            TestRegister(vSet, OperandType.Long, (byte)5, (long)5, true);
            TestRegister(vSet, OperandType.Long, (double)5, null, false);
            TestRegister(vSet, OperandType.Long, (float)5, null, false);
            TestRegister(vSet, OperandType.Long, (string)"5", null, false);
            TestRegister(vSet, OperandType.Long, (Variable)testVariable, null, false);
            TestRegister(vSet, OperandType.Long, (bool)false, null, false);
            TestRegister(vSet, OperandType.Long, (VariableSet)vSet, null, false);
            TestRegister(vSet, OperandType.Long, null, (long)5, false);

            TestRegister(vSet, OperandType.NullableLong, (long)5, (long)5, true);
            TestRegister(vSet, OperandType.NullableLong, (int)5, (long)5, true);
            TestRegister(vSet, OperandType.NullableLong, (short)5, (long)5, true);
            TestRegister(vSet, OperandType.NullableLong, (char)5, (long)5, true);
            TestRegister(vSet, OperandType.NullableLong, (byte)5, (long)5, true);
            TestRegister(vSet, OperandType.NullableLong, (double)5, null, false);
            TestRegister(vSet, OperandType.NullableLong, (float)5, null, false);
            TestRegister(vSet, OperandType.NullableLong, (string)"5", null, false);
            TestRegister(vSet, OperandType.NullableLong, (Variable)testVariable, null, false);
            TestRegister(vSet, OperandType.NullableLong, (bool)false, null, false);
            TestRegister(vSet, OperandType.NullableLong, (VariableSet)vSet, null, false);
            TestRegister(vSet, OperandType.NullableLong, null, null, true);

            TestRegister(vSet, OperandType.Double, (long)5, (double)5, true);
            TestRegister(vSet, OperandType.Double, (int)5, (double)5, true);
            TestRegister(vSet, OperandType.Double, (short)5, (double)5, true);
            TestRegister(vSet, OperandType.Double, (char)5, (double)5, true);
            TestRegister(vSet, OperandType.Double, (byte)5, (double)5, true);
            TestRegister(vSet, OperandType.Double, (double)5, (double)5, true);
            TestRegister(vSet, OperandType.Double, (float)5, (double)5, true);
            TestRegister(vSet, OperandType.Double, (string)"5", null, false);
            TestRegister(vSet, OperandType.Double, (Variable)testVariable, null, false);
            TestRegister(vSet, OperandType.Double, (bool)false, null, false);
            TestRegister(vSet, OperandType.Double, (VariableSet)vSet, null, false);
            TestRegister(vSet, OperandType.Double, null, null, false);

            TestRegister(vSet, OperandType.NullableDouble, (long)5, (double)5, true);
            TestRegister(vSet, OperandType.NullableDouble, (int)5, (double)5, true);
            TestRegister(vSet, OperandType.NullableDouble, (short)5, (double)5, true);
            TestRegister(vSet, OperandType.NullableDouble, (char)5, (double)5, true);
            TestRegister(vSet, OperandType.NullableDouble, (byte)5, (double)5, true);
            TestRegister(vSet, OperandType.NullableDouble, (double)5, (double)5, true);
            TestRegister(vSet, OperandType.NullableDouble, (float)5, (double)5, true);
            TestRegister(vSet, OperandType.NullableDouble, (string)"5", null, false);
            TestRegister(vSet, OperandType.NullableDouble, (Variable)testVariable, null, false);
            TestRegister(vSet, OperandType.NullableDouble, (bool)false, null, false);
            TestRegister(vSet, OperandType.NullableDouble, (VariableSet)vSet, null, false);
            TestRegister(vSet, OperandType.NullableDouble, null, null, true);

            TestRegister(vSet, OperandType.String, (long)5, null, false);
            TestRegister(vSet, OperandType.String, (int)5, null, false);
            TestRegister(vSet, OperandType.String, (short)5, null, false);
            TestRegister(vSet, OperandType.String, (char)5, null, false);
            TestRegister(vSet, OperandType.String, (byte)5, null, false);
            TestRegister(vSet, OperandType.String, (double)5, null, false);
            TestRegister(vSet, OperandType.String, (float)5, null, false);
            TestRegister(vSet, OperandType.String, (string)"5", "5", true);
            TestRegister(vSet, OperandType.String, (Variable)testVariable, null, false);
            TestRegister(vSet, OperandType.String, (bool)false, null, false);
            TestRegister(vSet, OperandType.String, (VariableSet)vSet, null, false);
            TestRegister(vSet, OperandType.String, null, null, true);

            //TestRegister(vSet, OperandType.Variable, (long)5, null, false);
            //TestRegister(vSet, OperandType.Variable, (int)5, null, false);
            //TestRegister(vSet, OperandType.Variable, (short)5, null, false);
            //TestRegister(vSet, OperandType.Variable, (char)5, null, false);
            //TestRegister(vSet, OperandType.Variable, (byte)5, null, false);
            //TestRegister(vSet, OperandType.Variable, (double)5, null, false);
            //TestRegister(vSet, OperandType.Variable, (float)5, null, false);
            //TestRegister(vSet, OperandType.Variable, (string)"5", null, false);
            //TestRegister(vSet, OperandType.Variable, (Variable)testVariable, testVariable, true);
            //TestRegister(vSet, OperandType.Variable, (bool)false, null, false);
            //TestRegister(vSet, OperandType.Variable, (VariableSet)vSet, null, false);
            //TestRegister(vSet, OperandType.Variable, null, null, true);

            TestRegister(vSet, OperandType.Bool, (long)5, null, false);
            TestRegister(vSet, OperandType.Bool, (int)5, null, false);
            TestRegister(vSet, OperandType.Bool, (short)5, null, false);
            TestRegister(vSet, OperandType.Bool, (char)5, null, false);
            TestRegister(vSet, OperandType.Bool, (byte)5, null, false);
            TestRegister(vSet, OperandType.Bool, (double)5, null, false);
            TestRegister(vSet, OperandType.Bool, (float)5, null, false);
            TestRegister(vSet, OperandType.Bool, (string)"5", null, false);
            TestRegister(vSet, OperandType.Bool, (Variable)testVariable, null, false);
            TestRegister(vSet, OperandType.Bool, (bool)false, false, true);
            TestRegister(vSet, OperandType.Bool, (VariableSet)vSet, null, false);
            TestRegister(vSet, OperandType.Bool, null, null, false);

            TestRegister(vSet, OperandType.NullableBool, (long)5, null, false);
            TestRegister(vSet, OperandType.NullableBool, (int)5, null, false);
            TestRegister(vSet, OperandType.NullableBool, (short)5, null, false);
            TestRegister(vSet, OperandType.NullableBool, (char)5, null, false);
            TestRegister(vSet, OperandType.NullableBool, (byte)5, null, false);
            TestRegister(vSet, OperandType.NullableBool, (double)5, null, false);
            TestRegister(vSet, OperandType.NullableBool, (float)5, null, false);
            TestRegister(vSet, OperandType.NullableBool, (string)"5", null, false);
            TestRegister(vSet, OperandType.NullableBool, (Variable)testVariable, null, false);
            TestRegister(vSet, OperandType.NullableBool, (bool)false, false, true);
            TestRegister(vSet, OperandType.NullableBool, (VariableSet)vSet, null, false);
            TestRegister(vSet, OperandType.NullableBool, null, null, true);

            TestRegister(vSet, OperandType.VSet, (long)5, null, false);
            TestRegister(vSet, OperandType.VSet, (int)5, null, false);
            TestRegister(vSet, OperandType.VSet, (short)5, null, false);
            TestRegister(vSet, OperandType.VSet, (char)5, null, false);
            TestRegister(vSet, OperandType.VSet, (byte)5, null, false);
            TestRegister(vSet, OperandType.VSet, (double)5, null, false);
            TestRegister(vSet, OperandType.VSet, (float)5, null, false);
            TestRegister(vSet, OperandType.VSet, (string)"5", null, false);
            TestRegister(vSet, OperandType.VSet, (Variable)testVariable, null, false);
            TestRegister(vSet, OperandType.VSet, (bool)false, null, false);
            TestRegister(vSet, OperandType.VSet, (VariableSet)vSet, vSet, true);
            TestRegister(vSet, OperandType.VSet, null, null, true);

            TestRegister(vSet, OperandType.Object, (long)5, (long)5, true);
            TestRegister(vSet, OperandType.Object, (int)5, (int)5, true);
            TestRegister(vSet, OperandType.Object, (short)5, (short)5, true);
            TestRegister(vSet, OperandType.Object, (char)5, (char)5, true);
            TestRegister(vSet, OperandType.Object, (byte)5, (byte)5, true);
            TestRegister(vSet, OperandType.Object, (double)5, (double)5, true);
            TestRegister(vSet, OperandType.Object, (float)5, (float)5, true);
            TestRegister(vSet, OperandType.Object, (string)"5", "5", true);
            TestRegister(vSet, OperandType.Object, (Variable)testVariable, testVariable, true);
            TestRegister(vSet, OperandType.Object, (bool)false, false, true);
            TestRegister(vSet, OperandType.Object, (VariableSet)vSet, vSet, true);
            TestRegister(vSet, OperandType.Object, null, null, true);
        }

        private void TestRegister(VariableSet vSet, OperandType type, object initialValue, object expectedValue, bool expectedSucceed)
        {
            try
            {
                var variable = vSet.RegisterVariable(type, "testName", initialValue);
                Assert.AreEqual(true, expectedSucceed);
                Assert.AreEqual(expectedValue, vSet.GetVariable("testName").Value);
                vSet.UnregisterVariable(variable);
            }
            catch (InvalidCastException ice)
            {
                Assert.AreEqual(false, expectedSucceed);
            }
            catch (NullReferenceException nre)
            {
                Assert.AreEqual(false, expectedSucceed);
            }
        }


    }
}
