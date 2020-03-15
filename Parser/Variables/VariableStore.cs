using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Variables
{
    public abstract class VariableStore : IVariableStore
    {
        private char[] _dot = new[] { '.' };
        protected IDictionary<string, Variable> _allVariables;
        public IReadOnlyDictionary<string, Variable> AllVariables { get; }

        public VariableStore(IDictionary<string, Variable> allVariables)
        {
            _allVariables = allVariables ?? new Dictionary<string, Variable>();
            AllVariables = new ReadOnlyDictionary<string, Variable>(_allVariables);
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
            this._allVariables.Remove(variable.VariableName);
        }
    }
}
