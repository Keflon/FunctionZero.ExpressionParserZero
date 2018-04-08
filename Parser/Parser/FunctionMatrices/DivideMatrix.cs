#region License
// Author: Keith Pickford
// 
// MIT License
// 
// Copyright (c) 2016 FunctionZero Ltd
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

namespace FunctionZero.ExpressionParserZero.Parser.FunctionMatrices
{
    internal static class DivideMatrix
    {
	    /*
Long			Long
Long			NullableLong
Long			Double
Long			NullableDouble
NullableLong	Long
NullableLong	NullableLong
NullableLong	Double
NullableLong	NullableDouble
Double			Long
Double			NullableLong
Double			Double
Double			NullableDouble
NullableDouble	Long
NullableDouble	NullableLong
NullableDouble	Double
NullableDouble	NullableDouble
*/
		internal static DoubleOperandFunctionMatrix Create()
        {
            var matrix = new DoubleOperandFunctionMatrix();
			//matrix.RegisterDelegate(OperandType.Long, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Long, (long)leftOperand.GetValue() / (long)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.Long, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Double, (long)leftOperand.GetValue() / (double)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.Double, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Double, (double)leftOperand.GetValue() / (long)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.Double, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Double, (double)leftOperand.GetValue() / (double)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.Long, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Long, (long)leftOperand.GetValue() / (long)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Long, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.NullableLong, (long)leftOperand.GetValue() / (long?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Long, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Double, (long)leftOperand.GetValue() / (double)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Long, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (long)leftOperand.GetValue() / (double?)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.NullableLong, (long?)leftOperand.GetValue() / (long)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.NullableLong, (long?)leftOperand.GetValue() / (long?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (long?)leftOperand.GetValue() / (double)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (long?)leftOperand.GetValue() / (double?)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.Double, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Double, (double)leftOperand.GetValue() / (long)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Double, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (double)leftOperand.GetValue() / (long?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Double, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Double, (double)leftOperand.GetValue() / (double)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Double, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (double)leftOperand.GetValue() / (double?)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (double?)leftOperand.GetValue() / (long)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (double?)leftOperand.GetValue() / (long?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (double?)leftOperand.GetValue() / (double)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (double?)leftOperand.GetValue() / (double?)rightOperand.GetValue()));

			return matrix;
        }
    }
}
