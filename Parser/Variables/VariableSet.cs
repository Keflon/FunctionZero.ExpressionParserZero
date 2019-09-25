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
    public class VariableSet : IVariableSet
    {
        public Dictionary<string, Variable> AllVariables { get; }
        public IVariableFactory VariableFactory { get; }

        public bool NotifyChanges { get; set; }

        public event EventHandler<VariableAddedEventArgs> VariableAdded;
        public event EventHandler<VariableRemovedEventArgs> VariableRemoved;

        public VariableSet(IVariableFactory variableFactory = null)
        {
            VariableFactory = variableFactory ?? new DefaultVariableFactory();

            AllVariables = new Dictionary<string, Variable>();

            NotifyChanges = true;
        }

        private void RegisterVariable(string qualifiedVariableName, OperandType type, object initialValue, object state)
        {
            ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);

            var variable = VariableFactory.CreateVariable(unqualifiedVariableName, type, initialValue, state);

	        targetVariableSet.RegisterVariable(variable);
        }

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="variable"></param>
	    public void RegisterVariable(Variable variable)
	    {
		    variable.VariableChanged += OnVariableChanged;
		    this.AllVariables.Add(variable.VariableName, variable);
		    VariableAdded?.Invoke(this, new VariableAddedEventArgs(variable));
	    }

	    public bool UnregisterVariable(string qualifiedVariableName)
	    {
		    ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);

			Variable targetVariable = targetVariableSet.GetVariable(unqualifiedVariableName);

		    if (targetVariable != null)
		    {
			    targetVariableSet.UnregisterVariable(targetVariable);
			    return true;
		    }
		    return false;
	    }

	    public void UnregisterVariable(Variable variable)
	    {
		    variable.VariableChanged -= OnVariableChanged;
		    this.AllVariables.Remove(variable.VariableName);
		    this.VariableRemoved?.Invoke(this, new VariableRemovedEventArgs(variable));
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

        public void RegisterVSet(string variableName, IVariableSet initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.VSet, initialValue, state);
        }

        public void RegisterObject(string variableName, object initialValue, object state = null)
        {
            RegisterVariable(variableName, OperandType.Object, initialValue, state);
        }

        #endregion

        #region UpdateOrCreate

        public void CreateOrUpdateLong(string variableName, long newValue, object state = null)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.Long, newValue, state);
            else
                AllVariables[variableName].Value = newValue;
        }

	    public void CreateOrUpdateNullableLong(string variableName, long? newValue, object state = null)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.NullableLong, newValue, state);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateDouble(string variableName, double newValue, object state = null)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.Double, newValue, state);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateNullableDouble(string variableName, double? newValue, object state = null)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.NullableDouble, newValue, state);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateString(string variableName, string newValue, object state = null)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.String, newValue, state);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateBool(string variableName, bool newValue, object state = null)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.Bool, newValue, state);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateNullableBool(string variableName, bool? newValue, object state = null)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.NullableBool, newValue, state);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateVSet(string variableName, VariableSet newValue, object state = null)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.VSet, newValue, state);
            else
                AllVariables[variableName].Value = newValue;
        }

        public void CreateOrUpdateObject(string variableName, object newValue, object state = null)
        {
            if (!AllVariables.ContainsKey(variableName))
                RegisterVariable(variableName, OperandType.Object, newValue, state);
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

        void ProcessVariableName(string qualifiedVariableName, out IVariableSet vSet, out string variableName)
        {
            IVariableSet currentVSet = this;
            var bits = qualifiedVariableName.Split(_dot);
            for (int c = 0; c < bits.Length - 1; c++)
            {
                currentVSet = (IVariableSet)currentVSet.AllVariables[bits[c]].Value;
            }
            vSet = currentVSet;
            variableName = bits[bits.Length - 1];
        }

        public event EventHandler<VariableChangedEventArgs> VariableChanged;

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
