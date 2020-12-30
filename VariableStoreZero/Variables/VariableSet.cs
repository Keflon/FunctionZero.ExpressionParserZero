﻿#region License
// Author: Keith Pickford
// 
// MIT License
// 
// Copyright (c) 2016 -2020 FunctionZero Ltd
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

namespace FunctionZero.ExpressionParserZero.Variables
{
    public class VariableSet : VariableStore, IObservableVariableStore
    {
        public VariableSet(IVariableFactory variableFactory = null) : base(null, variableFactory)
        {
            NotifyChanges = true;
        }

        public VariableSet(IEnumerable<Variable> allVariables, IVariableFactory variableFactory = null) : base(allVariables, variableFactory)
        {
            NotifyChanges = true;
        }

        public bool NotifyChanges { get; set; }

        public event EventHandler<VariableAddedEventArgs> VariableAdded;
        public event EventHandler<VariableRemovedEventArgs> VariableRemoved;
        public event EventHandler<VariableChangingEventArgs> VariableChanging;
        public event EventHandler<VariableChangedEventArgs> VariableChanged;

        public override Variable RegisterVariable(Variable variable)
        {
            variable.VariableChanging += OnVariableChanging;
            variable.VariableChanged += OnVariableChanged;
            base.RegisterVariable(variable);
            VariableAdded?.Invoke(this, new VariableAddedEventArgs(variable));
            return variable;
        }

        public override Variable UnregisterVariable(Variable variable)
        {
            variable.VariableChanged -= OnVariableChanged;
            variable.VariableChanging -= OnVariableChanging;
            base.UnregisterVariable(variable);
            this.VariableRemoved?.Invoke(this, new VariableRemovedEventArgs(variable));
            return variable;
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
