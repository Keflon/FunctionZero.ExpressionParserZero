using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Variables
{
    public class ObservableVariableStore : VariableStore, IObservableVariableStore
    {
        public ObservableVariableStore(IDictionary<string, Variable> allVariables = null) : base(allVariables)
        {
        }

        public bool NotifyChanges { get; set; }

        public event EventHandler<VariableAddedEventArgs> VariableAdded;
        public event EventHandler<VariableRemovedEventArgs> VariableRemoved;
        public event EventHandler<VariableChangingEventArgs> VariableChanging;
        public event EventHandler<VariableChangedEventArgs> VariableChanged;

        public override void RegisterVariable(Variable variable)
        {
            variable.VariableChanging += OnVariableChanging;
            variable.VariableChanged += OnVariableChanged;
            base._allVariables.Add(variable.VariableName, variable);
            VariableAdded?.Invoke(this, new VariableAddedEventArgs(variable));
        }

        public override void UnregisterVariable(Variable variable)
        {
            variable.VariableChanged -= OnVariableChanged;
            variable.VariableChanging -= OnVariableChanging;
            base._allVariables.Remove(variable.VariableName);
            this.VariableRemoved?.Invoke(this, new VariableRemovedEventArgs(variable));
        }

        protected void OnVariableChanging(object sender, VariableChangingEventArgs e)
        {
            if (NotifyChanges)
                this.VariableChanging?.Invoke(this, e);
        }

        protected void OnVariableChanged(object sender, VariableChangedEventArgs e)
        {
            if (NotifyChanges)
                this.VariableChanged?.Invoke(this, e);
        }
    }
}
