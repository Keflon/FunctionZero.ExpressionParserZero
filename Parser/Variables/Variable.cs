#region License
// Author: Keith Pickford
// 
// MIT License
// 
// Copyright (c) 2016 FunctionZero Ltd
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
using System;
using FunctionZero.ExpressionParserZero.Operands;

namespace FunctionZero.ExpressionParserZero.Variables
{
    // TODO: Use an operand of type 'Variable'?
    public class Variable
    {
        private object _value;

        /// <summary>
        /// A variable has a name, type and value. Get or set the value here.
        /// </summary>
		public object Value
        {
            get { return _value; }
            set
            {
                if (Equals(value, _value)) return;
                OnVariableChanging();
                switch (VariableType)
                {
                    case OperandType.Long:
                        _value = (long)value;
                        break;
                    case OperandType.NullableLong:
                        _value = (long?)value;
                        break;
                    case OperandType.Double:
                        _value = (double)value;
                        break;
                    case OperandType.NullableDouble:
                        _value = (double?)value;
                        break;
                    case OperandType.String:
                        _value = (string)value;
                        break;
                    case OperandType.Bool:
                        _value = (bool)value;
                        break;
                    case OperandType.NullableBool:
                        _value = (bool?)value;
                        break;
                    case OperandType.VSet:
                        _value = (IVariableSet)value;
                        break;
                    case OperandType.Object:
                        _value = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Variable.Value setter OperandType unhandled.");
                }
                _value = value;
                OnVariableChanged();
            }
        }

        /// <summary>
        /// A variable has a name, type and value. Get the name here.
        /// </summary>
		public string VariableName { get; }

        /// <summary>
        /// A variable has a name, type and value. Get the type here.
        /// </summary>
		public OperandType VariableType { get; }

        /// <summary>
        /// Returns a new variable with the provided name, type and initial value.
        /// </summary>
        /// <param name="variableName">The name of the variable</param>
        /// <param name="variableType">The type of the variable</param>
        /// <param name="startingValue">The initial value of the variable</param>
		public Variable(string variableName, OperandType variableType, object startingValue)
        {
            if (variableType == OperandType.Null)
                throw new Exception("Attempt to create a variable of type Null.");

            this.VariableName = variableName;
            this.VariableType = variableType;
            this.Value = startingValue;
        }

        /// <summary>
        /// This event is raised when the value of this variable is about to change.
        /// </summary>
        public event EventHandler<VariableChangingEventArgs> VariableChanging;

        /// <summary>
        /// This event is raised when the value of this variable changes.
        /// </summary>
        public event EventHandler<VariableChangedEventArgs> VariableChanged;

        /// <summary>
        /// Can be overidden. Simply raises the VariableChanging event.
        /// </summary>
        protected virtual void OnVariableChanging()
        {
            VariableChanging?.Invoke(this, new VariableChangingEventArgs(this));
        }
        /// <summary>
        /// Can be overidden. Simply raises the VariableChanged event.
        /// </summary>
		protected virtual void OnVariableChanged()
        {
            VariableChanged?.Invoke(this, new VariableChangedEventArgs(this));
        }
    }
}
