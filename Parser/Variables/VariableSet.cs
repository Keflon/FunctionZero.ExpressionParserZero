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
using System.Collections.Generic;
using FunctionZero.ExpressionParserZero.Operands;

namespace FunctionZero.ExpressionParserZero.Variables
{
    public class VariableSet : ObservableVariableStore
    {
        public IVariableFactory VariableFactory { get; }

        public VariableSet(IVariableFactory variableFactory = null) : base(new Dictionary<string, Variable>())
        {
            VariableFactory = variableFactory ?? new DefaultVariableFactory();
            NotifyChanges = true;
        }

        private void RegisterVariable(string qualifiedVariableName, OperandType type, object initialValue, object state)
        {
            ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);
            var variable = VariableFactory.CreateVariable(unqualifiedVariableName, type, initialValue, state);
            targetVariableSet.RegisterVariable(variable);
        }

        public void UnregisterVariable(string qualifiedVariableName)
        {
            ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);
            Variable targetVariable = targetVariableSet.GetVariable(unqualifiedVariableName);
            targetVariableSet.UnregisterVariable(targetVariable);
        }

        #region Register

        public void RegisterLong(string variableName, long initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.Long, initialValue, state);
        }

        public void RegisterNullableLong(string variableName, long? initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.NullableLong, initialValue, state);
        }

        public void RegisterDouble(string variableName, double initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.Double, initialValue, state);
        }

        public void RegisterNullableDouble(string variableName, double initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.NullableDouble, initialValue, state);
        }

        public void RegisterString(string variableName, string initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.String, initialValue, state);
        }

        public void RegisterBool(string variableName, bool initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.Bool, initialValue, state);
        }

        public void RegisterNullableBool(string variableName, bool? initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.NullableBool, initialValue, state);
        }

        public void RegisterVSet(string variableName, IVariableStore initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.VSet, initialValue, state);
        }

        public void RegisterObject(string variableName, object initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.Object, initialValue, state);
        }

        #endregion

        //   #region UpdateOrCreate

        //   public void CreateOrUpdateLong(string variableName, long newValue, object state = null)
        //   {
        //       if (!AllVariables.ContainsKey(variableName))
        //           RegisterVariable(variableName, OperandType.Long, newValue, state);
        //       else
        //           AllVariables[variableName].Value = newValue;
        //   }

        //public void CreateOrUpdateNullableLong(string variableName, long? newValue, object state = null)
        //   {
        //       if (!AllVariables.ContainsKey(variableName))
        //           RegisterVariable(variableName, OperandType.NullableLong, newValue, state);
        //       else
        //           AllVariables[variableName].Value = newValue;
        //   }

        //   public void CreateOrUpdateDouble(string variableName, double newValue, object state = null)
        //   {
        //       if (!AllVariables.ContainsKey(variableName))
        //           RegisterVariable(variableName, OperandType.Double, newValue, state);
        //       else
        //           AllVariables[variableName].Value = newValue;
        //   }

        //   public void CreateOrUpdateNullableDouble(string variableName, double? newValue, object state = null)
        //   {
        //       if (!AllVariables.ContainsKey(variableName))
        //           RegisterVariable(variableName, OperandType.NullableDouble, newValue, state);
        //       else
        //           AllVariables[variableName].Value = newValue;
        //   }

        //   public void CreateOrUpdateString(string variableName, string newValue, object state = null)
        //   {
        //       if (!AllVariables.ContainsKey(variableName))
        //           RegisterVariable(variableName, OperandType.String, newValue, state);
        //       else
        //           AllVariables[variableName].Value = newValue;
        //   }

        //   public void CreateOrUpdateBool(string variableName, bool newValue, object state = null)
        //   {
        //       if (!AllVariables.ContainsKey(variableName))
        //           RegisterVariable(variableName, OperandType.Bool, newValue, state);
        //       else
        //           AllVariables[variableName].Value = newValue;
        //   }

        //   public void CreateOrUpdateNullableBool(string variableName, bool? newValue, object state = null)
        //   {
        //       if (!AllVariables.ContainsKey(variableName))
        //           RegisterVariable(variableName, OperandType.NullableBool, newValue, state);
        //       else
        //           AllVariables[variableName].Value = newValue;
        //   }

        //   public void CreateOrUpdateVSet(string variableName, IVariableSet newValue, object state = null)
        //   {
        //       if (!AllVariables.ContainsKey(variableName))
        //           RegisterVariable(variableName, OperandType.VSet, newValue, state);
        //       else
        //           AllVariables[variableName].Value = newValue;
        //   }

        //   public void CreateOrUpdateObject(string variableName, object newValue, object state = null)
        //   {
        //       if (!AllVariables.ContainsKey(variableName))
        //           RegisterVariable(variableName, OperandType.Object, newValue, state);
        //       else
        //           AllVariables[variableName].Value = newValue;
        //   }

        //   #endregion


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
    }
}
