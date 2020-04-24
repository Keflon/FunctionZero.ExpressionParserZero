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
using System.Text;
using FunctionZero.ExpressionParserZero.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Operands;

namespace FunctionZero.ExpressionParserZero.Parser.FunctionVectors
{
    public static class UnaryComplementVector
    {
        internal static SingleOperandFunctionVector Create()
        {
            var vector = new SingleOperandFunctionVector();
            vector.RegisterDelegate(OperandType.Long, operand => new Operand(OperandType.Long, ~(long)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableLong, operand => new Operand(OperandType.NullableLong, ~(long?)operand.GetValue()));
            return vector;
        }
    }
}
