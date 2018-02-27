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
using System.Diagnostics;
using FunctionZero.ExpressionParserZero.Operands;

namespace FunctionZero.ExpressionParserZero.Variables
{
    public class VariableSet
    {
        public Dictionary<string, Variable> AllVariables { get; }
        public IVariableFactory VariableFactory { get; }

        public bool NotifyChanges { get; set; }

        public EventHandler<VariableAddedEventArgs> VariableAdded;

        public VariableSet(IVariableFactory variableFactory = null)
        {
            VariableFactory = variableFactory ?? new DefaultVariableFactory();

            AllVariables = new Dictionary<string, Variable>();

            NotifyChanges = true;
        }

        private void RegisterVariable(string qualifiedVariableName, OperandType type, object initialValue)
        {
            ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);

            var variable = VariableFactory.CreateVariable(unqualifiedVariableName, type, initialValue);
            VariableAdded?.Invoke(this, new VariableAddedEventArgs(variable));
            variable.VariableChanged += OnVariableChanged;
            targetVariableSet.AllVariables.Add(unqualifiedVariableName, variable);
        }

        #region Register

        public void RegisterLong(string variableName, long initialValue)
        {
            RegisterVariable(variableName, OperandType.Long, initialValue);
        }

        public void RegisterDouble(string variableName, double initialValue)
        {
            RegisterVariable(variableName, OperandType.Double, initialValue);
        }

        public void RegisterString(string variableName, string initialValue)
        {
            RegisterVariable(variableName, OperandType.String, initialValue);
        }

        public void RegisterBool(string variableName, bool initialValue)
        {
            RegisterVariable(variableName, OperandType.Bool, initialValue);
        }

        public void RegisterNullableBool(string variableName, bool? initialValue)
        {
            RegisterVariable(variableName, OperandType.NullableBool, initialValue);
        }

        public void RegisterVSet(string variableName, object initialValue)
        {
            RegisterVariable(variableName, OperandType.VSet, initialValue);
        }

        public void RegisterObject(string variableName, object initialValue)
        {
            RegisterVariable(variableName, OperandType.Object, initialValue);
        }

        #endregion

        #region UpdateOrCreate

        public void CreateOrUpdateLong(string variableName, long newValue)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.Long, newValue);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateDouble(string variableName, double newValue)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.Double, newValue);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateString(string variableName, string newValue)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.String, newValue);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateBool(string variableName, bool newValue)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.Bool, newValue);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateNullableBool(string variableName, bool? newValue)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.NullableBool, newValue);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateVSet(string variableName, VariableSet newValue)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.VSet, newValue);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateObject(string variableName, object newValue)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.Object, newValue);
            else
                AllVariables[variableName].Value = newValue;
        }

        #endregion

        private char[] _dot = new[] { '.' };

        public Variable GetVariable(string qualifiedVariableName)
        {
            ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);

            if (targetVariableSet.AllVariables.ContainsKey(unqualifiedVariableName))
                return targetVariableSet.AllVariables[unqualifiedVariableName];
            return null;
        }

        void ProcessVariableName(string qualifiedVariableName, out VariableSet vSet, out string variableName)
        {
            VariableSet currentVSet = this;
            var bits = qualifiedVariableName.Split(_dot);
            for (int c = 0; c < bits.Length - 1; c++)
            {
                currentVSet = (VariableSet)currentVSet.AllVariables[bits[c]].Value;
            }
            vSet = currentVSet;
            variableName = bits[bits.Length - 1];
        }

        public EventHandler<VariableChangedEventArgs> VariableChanged;

        public void SetVariable(string qualifiedVariableName, object newValue)
        {
            // DOTTY: Parse dotted strings.
            ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);

            Variable varp;
            if (targetVariableSet.AllVariables.TryGetValue(unqualifiedVariableName, out varp))
            {
                SetVariable(varp, newValue);
            }
            else
            {
                throw new ArgumentException("Variable named " + unqualifiedVariableName + "' not found.");
            }
        }

        public void SetVariable(Variable varp, object newValue)
        {
            object oldValue = varp.Value;

            if (!Equals(oldValue, newValue))
            {
                varp.Value = newValue;
            }
        }

        private void OnVariableChanged(object sender, VariableChangedEventArgs e)
        {
            if (NotifyChanges)
                this.VariableChanged?.Invoke(this, e);
        }
    }
}
