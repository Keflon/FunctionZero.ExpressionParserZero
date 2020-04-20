using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Variables
{
    public class ObservableVariableStore : VariableStore, IObservableVariableStore
    {
        public ObservableVariableStore(IDictionary<string, Variable> allVariables = null, IVariableFactory variableFactory = null) : base(allVariables, variableFactory)
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
            base.RegisterVariable(variable);
            VariableAdded?.Invoke(this, new VariableAddedEventArgs(variable));
        }

        public override void UnregisterVariable(Variable variable)
        {
            variable.VariableChanged -= OnVariableChanged;
            variable.VariableChanging -= OnVariableChanging;
            base.UnregisterVariable(variable);
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
