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
	public static class BitwiseAndMatrix
	{
/*
Long			Long
Long			NullableLong
NullableLong	Long
NullableLong	NullableLong
Bool			Bool
Bool			NullableBool
NullableBool	Bool
NullableBool	NullableBool
*/
		public static DoubleOperandFunctionMatrix Create()
		{
			var matrix = new DoubleOperandFunctionMatrix();
			
			matrix.RegisterDelegate(OperandType.Long, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Long, (long)leftOperand.GetValue() & (long)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Long, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.NullableLong, (long)leftOperand.GetValue() & (long?)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.NullableLong, (long?)leftOperand.GetValue() & (long)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.NullableLong, (long?)leftOperand.GetValue() & (long?)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.Bool, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool)leftOperand.GetValue() & (bool)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Bool, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.NullableBool, (bool)leftOperand.GetValue() & (bool?)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.NullableBool, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.NullableBool, (bool?)leftOperand.GetValue() & (bool)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableBool, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.NullableBool, (bool?)leftOperand.GetValue() & (bool?)rightOperand.GetValue()));

			return matrix;
		}
	}
}
