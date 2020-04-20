using FunctionZero.ExpressionParserZero.Operands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FunctionZero.ExpressionParserZero.Variables
{
    public abstract class VariableStore : IVariableStore
    {
        private char[] _dot = new[] { '.' };
        protected IDictionary<string, Variable> _allVariables;
        public IReadOnlyDictionary<string, Variable> AllVariables { get; }

        public VariableStore(IDictionary<string, Variable> allVariables, IVariableFactory variableFactory)
        {
            _allVariables = allVariables ?? new Dictionary<string, Variable>();
            AllVariables = new ReadOnlyDictionary<string, Variable>(_allVariables);
            VariableFactory = variableFactory ?? new DefaultVariableFactory();
        }

        public Variable GetVariable(string qualifiedVariableName)
        {
            ProcessVariableName(qualifiedVariableName, out var targetVariableSet, out var unqualifiedVariableName);
            return targetVariableSet.AllVariables[unqualifiedVariableName];
        }

        protected void ProcessVariableName(string qualifiedVariableName, out IVariableStore vSet, out string variableName)
        {
            IVariableStore currentVSet = this;
            var bits = qualifiedVariableName.Split(_dot);

            for (int c = 0; c < bits.Length - 1; c++)
                currentVSet = (IVariableStore)currentVSet.AllVariables[bits[c]].Value;
            
            vSet = currentVSet;
            variableName = bits[bits.Length - 1];
        }

        public virtual void RegisterVariable(Variable variable)
        {
            this._allVariables.Add(variable.VariableName, variable);
        }

        public virtual void UnregisterVariable(Variable variable)
        {
            var item = _allVariables[variable.VariableName];

            if (item != variable)
                throw new ValueNotFoundException("The key was found but the value does not match the variable you provided. It looks like you have provided a variable with the same name as a different variable in the collection. There is a seperate overload for removing variables by name.");

            this._allVariables.Remove(variable.VariableName);
        }
        public IVariableFactory VariableFactory { get; }

        public void RegisterVariable(OperandType type, string qualifiedVariableName, object initialValue, object state = null)
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
