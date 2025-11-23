#region License
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
using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Tokens;

namespace FunctionZero.ExpressionParserZero.Operators
{
    public interface IOperator : IToken
    {
        string AsString { get; }

        /// <summary>
        /// Operator precedence.
        /// </summary>
        int Precedence { get; }

        /// <summary>
        /// Performs the operator action on the stack.
        /// </summary>
        Action<Stack<IOperand>, IBackingStore, long> DoOperation { get; }

        OperatorType Type { get; }
        ShortCircuitMode ShortCircuit { get; }
    }

    public enum ShortCircuitMode
    {
        None = 0,
        LogicalAnd,
        LogicalOr
    }

    public enum OperatorType
    {
        Operator,
        UnaryOperator,
        Function,
        OpenParenthesis,
        CloseParenthesis,


        UnaryCastOperator,
    }
}