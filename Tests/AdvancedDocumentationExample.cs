using System;
using System.Collections.Generic;
using System.Diagnostics;
using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParserUnitTests
{
    [TestClass]
    public class AdvancedDocumentationExample
    {
        // A bag-of-horror method showing some advanced usage. Don't ever show it to my brother!
        [TestMethod]
        public void AdvancedDocumentationSample()
        {
            ///////////////////////////////////////////////
            // Create and configure the Parser object ...
            ///////////////////////////////////////////////
            ExpressionParser parser = new ExpressionParser();

            // Overload that will allow a Bool to be appended to a String
            // To add a String to a Bool you'll need to add another overload
            parser.RegisterOverload("+", OperandType.String, OperandType.Bool,
                (left, right) => new Operand(OperandType.String, (string)left.GetValue() + ((bool)right.GetValue()).ToString()));

            // A user-defined function
            // Note (at time of writing) validation of parameter count for functions isn't robust.
            parser.RegisterFunction("StringContains", DoStringContains, 3);

            ///////////////////////////////////////////////
            // Note the comma operator - this expression yields two results each time it is evaluated
            // It calls the function 'StringContains' registered above, passing in a variable, a constant and a bool
            // It uses the overload registered above to add a bool to a string
            ///////////////////////////////////////////////
            var compiledExpression = parser.Parse(
                "text = text + child.textPhrase, 'HammerCat found: ' + (StringContains(text, 'HammerCat', true) >= 0)"
                );

            ///////////////////////////////////////////////
            // Configure a VariableSet with a custom factory 
            ///////////////////////////////////////////////
            var variableFactory = new TestVariableFactory();
            VariableSet vSet = new VariableSet(variableFactory);

            vSet.RegisterVariable(OperandType.String, "text", "GO!-> ");
            vSet.RegisterVariable(OperandType.VSet, "child", new VariableSet(variableFactory));         // VariableSet - same factory
            vSet.RegisterVariable(OperandType.String, "child.textPhrase", "Who seeks HammerCat?");      // Nested Variable

            ///////////////////////////////////////////////
            // Evaluate ...
            ///////////////////////////////////////////////
            var resultStack = compiledExpression.Evaluate(vSet);

            ///////////////////////////////////////////////
            // Get both of the results ...
            ///////////////////////////////////////////////
            // Result of "'HammerCat found: ' + StringContains(text, 'HammerCat', true) >= 0"
            var second = (string)resultStack.Pop().GetValue();

            // Result of "text = text + child.textPhrase"
            var first = (string)resultStack.Pop().GetValue();

            // Ensure the result matches the variable ...
            string text = (string)vSet.GetVariable("text").Value;
            Assert.AreEqual(first, text);

            // Show the results ...
            Debug.WriteLine($"First result is: {first}");
            Debug.WriteLine($"Second result is: {second}");
            ShowCustomVariables("", vSet);
        }

        private void ShowCustomVariables(string prefix, VariableSet vSet)
        {
            foreach (MyVariable item in vSet)
            {
                Debug.WriteLine($"The variable '{prefix}{item.VariableName}' created at {item.Timestamp} was written to {item.WriteCount} time{(item.WriteCount == 1 ? "" : "s")}");
                if (item.VariableType == OperandType.VSet)
                    ShowCustomVariables(prefix + item.VariableName + ".", (VariableSet)item.Value);
            }
        }

        /// <summary>
        /// Called by the Evaluator when it encounters 'StringContains'
        /// long StringContains(string source, string subString, bool isCaseSensitive)
        /// Returns index of subString, or -1
        /// </summary>
        private void DoStringContains(Stack<IOperand> operands, IVariableStore variables, long parserPosition)
        {
            // Pop the correct number of parameters from the operands stack, ** in reverse order **
            // If an operand is a variable, it is resolved from the variables provided
            IOperand third = OperatorActions.PopAndResolve(operands, variables);
            IOperand second = OperatorActions.PopAndResolve(operands, variables);
            IOperand first = OperatorActions.PopAndResolve(operands, variables);

            string strSource = (string)first.GetValue();
            string strSubstring = (string)second.GetValue();
            StringComparison comparison = (bool)third.GetValue() == true ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            long result = strSource.IndexOf(strSubstring, comparison);

            operands.Push(new Operand(-1, OperandType.Long, result));
        }

        /// <summary>
        /// A basic IVariableFactory
        /// </summary>
        public class TestVariableFactory : IVariableFactory
        {
            public Variable CreateVariable(string name, OperandType type, object defaultValue, object state)
            {
                return new MyVariable(name, type, defaultValue, DateTime.UtcNow.ToString());
            }
        }

        /// <summary>
        /// A specialisation of a Variable for the TestVariableFactory to manufacture
        /// </summary>
        internal class MyVariable : Variable
        {
            // 
            public long WriteCount { get; private set; } = 1;
            public string Timestamp { get; }

            public MyVariable(string variableName, OperandType variableType, object startingValue, string timestamp) : base(variableName, variableType, startingValue, "Hello!")
            {
                this.VariableChanged += MyVariable_VariableChanged;
                Timestamp = timestamp;
            }

            private void MyVariable_VariableChanged(object sender, VariableChangedEventArgs e)
            {
                WriteCount++;
            }
        }
    }
}
