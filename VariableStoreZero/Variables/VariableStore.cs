#region License
// Author: Keith Pickford
// 
// MIT License
// 
// Copyright (c) 2016 -2020 FunctionZero Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion
using FunctionZero.ExpressionParserZero.Operands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FunctionZero.ExpressionParserZero.Variables
{
    public abstract class VariableStore : IVariableStore
    {
        private char[] _dot = new[] { '.' };
        protected IDictionary<string, Variable> _allVariables;

        public VariableStore(IEnumerable<Variable> seedVariables, IVariableFactory variableFactory)
        {
            _allVariables = new Dictionary<string, Variable>();

            // Seed the dictionary
            if (seedVariables != null)
                foreach (var item in seedVariables)
                    _allVariables.Add(item.VariableName, item);

            VariableFactory = variableFactory ?? new DefaultVariableFactory();
        }

        public Variable this[string unqualifiedVariableName]
        {
            get => _allVariables[unqualifiedVariableName];
        }



        public Variable GetVariable(string qualifiedVariableName)
        {
            ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);
            return targetVariableSet[unqualifiedVariableName];
        }

        protected void ProcessVariableName(string qualifiedVariableName, out IVariableStore vSet, out string variableName)
        {
            IVariableStore currentVSet = this;
            var bits = qualifiedVariableName.Split(_dot);

            for (int c = 0; c < bits.Length - 1; c++)
                currentVSet = (IVariableStore)currentVSet[bits[c]].Value;

            vSet = currentVSet;
            variableName = bits[bits.Length - 1];
        }

        public virtual Variable RegisterVariable(Variable variable)
        {
            this._allVariables.Add(variable.VariableName, variable);
            return variable;
        }

        public virtual Variable UnregisterVariable(Variable variable)
        {
            var item = _allVariables[variable.VariableName];

            if (item != variable)
                throw new ValueNotFoundException("The key was found but the value does not match the variable you provided. It looks like you have provided a variable with the same name as a different variable in the collection. There is a seperate overload for removing variables by name.");

            this._allVariables.Remove(variable.VariableName);
            return variable;
        }
        public IVariableFactory VariableFactory { get; }

        public Variable RegisterVariable(OperandType type, string qualifiedVariableName, object initialValue, object state = null)
        {
            object scrubbedValue = null;

            switch (type)
            {
                case OperandType.Long:
                    var longResult = CastToLong(initialValue);
                    if (longResult.success)
                        scrubbedValue = longResult.result;
                    else
                        // Force an exception!
                        scrubbedValue = (long)initialValue;
                    break;
                case OperandType.NullableLong:
                    if (initialValue != null)
                        goto case OperandType.Long;
                    break;
                case OperandType.Double:
                    var doubleResult = CastToDouble(initialValue);
                    if (doubleResult.success)
                        scrubbedValue = doubleResult.result;
                    else
                        // Force an exception!
                        scrubbedValue = (double)initialValue;
                    break;
                case OperandType.NullableDouble:
                    if (initialValue != null)
                        goto case OperandType.Double;
                    break;
                case OperandType.String:
                    scrubbedValue = (string)initialValue;
                    break;
                case OperandType.Variable:
                    scrubbedValue = (Variable)initialValue;
                    break;
                case OperandType.Bool:
                    scrubbedValue = (bool)initialValue;
                    break;
                case OperandType.NullableBool:
                    scrubbedValue = (bool?)initialValue;
                    break;
                case OperandType.VSet:
                    scrubbedValue = (VariableSet)initialValue;
                    break;
                case OperandType.Object:
                    scrubbedValue = initialValue;
                    break;
                case OperandType.Null:
                    break;
            }

            ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);
            var variable = VariableFactory.CreateVariable(unqualifiedVariableName, type, scrubbedValue, state);
            return targetVariableSet.RegisterVariable(variable);
        }



        (long result, bool success) CastToLong(object initialValue)
        {
            long scrubbedValue;

            if (initialValue is long scrubbedLong)
                scrubbedValue = scrubbedLong;
            else if (initialValue is int scrubbedInt)
                scrubbedValue = (long)scrubbedInt;
            else if (initialValue is short scrubbedShort)
                scrubbedValue = (long)scrubbedShort;
            else if (initialValue is char scrubbedChar)
                scrubbedValue = (long)scrubbedChar;
            else if (initialValue is byte scrubbedByte)
                scrubbedValue = (long)scrubbedByte;
            else
                return (0, false);

            return (scrubbedValue, true);
        }

        (double result, bool success) CastToDouble(object initialValue)
        {
            double scrubbedValue;

            if (initialValue is double scrubbedDouble)
                scrubbedValue = scrubbedDouble;
            else if (initialValue is float scrubbedFloat)
                scrubbedValue = (double)scrubbedFloat;
            else
            {
                var result = CastToLong(initialValue);

                if (result.success)
                    scrubbedValue = result.result;
                else
                    return (0, false);
            }
            return (scrubbedValue, true);
        }




        public Variable UnregisterVariable(string qualifiedVariableName)
        {
            ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);
            Variable targetVariable = targetVariableSet[unqualifiedVariableName];
            targetVariableSet.UnregisterVariable(targetVariable);
            return targetVariable;
        }

        public void SetVariableValue(string qualifiedVariableName, object newValue)
        {
            try
            {
                Variable variable = GetVariable(qualifiedVariableName);
                variable.Value = newValue;
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("Variable named " + qualifiedVariableName + "' not found.");
            }
        }

        public IEnumerator<Variable> GetEnumerator()
        {
            return _allVariables.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public (OperandType type, object value) GetValue(string qualifiedName)
        {
            var variable = GetVariable(qualifiedName);
            return (variable.VariableType, variable.Value);
        }

        public void SetValue(string qualifiedName, object value)
        {
            var variable = GetVariable(qualifiedName);
            variable.Value = value;
        }
    }
}
