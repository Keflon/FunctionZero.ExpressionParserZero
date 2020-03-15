using System;

namespace FunctionZero.ExpressionParserZero.Variables
{
    public interface IObservableVariableStore : IVariableStore
	{
		event EventHandler<VariableAddedEventArgs> VariableAdded;
		event EventHandler<VariableRemovedEventArgs> VariableRemoved;
		event EventHandler<VariableChangingEventArgs> VariableChanging;
		event EventHandler<VariableChangedEventArgs> VariableChanged;
		
		bool NotifyChanges { get; set; }
	}
}