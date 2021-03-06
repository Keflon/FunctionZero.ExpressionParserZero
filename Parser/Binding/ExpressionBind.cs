﻿using FunctionZero.ExpressionParserZero.Evaluator;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Tokens;
using System;
using System.Collections.Generic;

namespace FunctionZero.ExpressionParserZero.Binding
{
    public class ExpressionBind
    {
        private readonly IList<string> _bindingLookup;
        private readonly IList<PathBind> _bindingCollection;
        private readonly VariableEvaluator _evaluator;
        private readonly ExpressionTree _compiledExpression;
        private object _result;
        private bool _isStale;

        public bool IsStale
        {
            get => _isStale;
            set
            {
                if (value != _isStale)
                {
                    _isStale = value;
                    if (value == true)
                        ValueIsStale?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public object Result
        {
            // Caution. Not standard pattern.
            get { return Evaluate(); }
            set
            {
                IsStale = false;
                if (value != _result)
                {
                    _result = value;
                    ResultChanged?.Invoke(this, new ValueChangedEventArgs(value));
                }
            }
        }
        /// <summary>
        /// Result is re-evaluated lazily, i.e. when Result is stale AND Evaluate is called.
        /// Result is stale if the binding has not been evaluated or if Invalidate has been called.
        /// Invalidate() is called if a property referenced by the expression raises an INotifyPropertyChanged event.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ResultChanged;
        /// <summary>
        /// This event is raised if Invalidate() is called.
        /// Invalidate() is called if a property referenced by the expression raises an INotifyPropertyChanged event.
        /// </summary>
        public event EventHandler<EventArgs> ValueIsStale;

        public ExpressionBind(object host, string expression, ExpressionParser parser = null)
        {
            _bindingLookup = new List<string>();
            _bindingCollection = new List<PathBind>();

            _compiledExpression = (parser ?? ExpressionParserFactory.GetExpressionParser()).Parse(expression);

            foreach (IToken item in _compiledExpression.RpnTokens)
            {
                if (item is Operand op)
                {
                    if (op.Type == OperandType.Variable)
                    {
                        if (_bindingLookup.Contains(op.ToString()) == false)
                        {
                            var binding = new PathBind(host, op.ToString(), SomethingChanged);
                            _bindingLookup.Add(op.ToString());
                            _bindingCollection.Add(binding);
                        }
                    }
                }
            }
            _evaluator = new VariableEvaluator(_bindingLookup, _bindingCollection);
            IsStale = true;
        }

        public object Evaluate()
        {
            if (IsStale)
            {
                try
                {
                    IsStale = false;
                    var stack = _compiledExpression.Evaluate(_evaluator);
                    var operand = stack.Pop();

                    if (operand.Type == OperandType.Variable)
                    {
                        var valueAndType = _evaluator.GetValue((string)operand.GetValue());
                        Result = valueAndType.value;
                    }
                    else
                        Result = operand.GetValue();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            // Return the backing variable rather than the property, to prevent a recursive call.
            // The getter calls Evaluate!
            return _result;
        }

        private void SomethingChanged(object newValue) => Invalidate();

        public void Invalidate() => IsStale = true;
    }
}
