using System;

namespace FunctionZero.ExpressionParserZero.Binding
{
    public class ValueChangedEventArgs : EventArgs
    {
        public ValueChangedEventArgs(object newValue)
        {
            NewValue = newValue;
        }

        public object NewValue { get; }
    }
}