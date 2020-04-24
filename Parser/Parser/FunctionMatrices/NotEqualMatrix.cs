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

namespace FunctionZero.ExpressionParserZero.Parser.FunctionMatrices
{
    /*


    */


    public static class NotEqualMatrix
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
NullableLong	Null

Double			Long
Double			NullableLong
Double			Double
Double			NullableDouble

NullableDouble	Long
NullableDouble	NullableLong
NullableDouble	Double
NullableDouble	NullableDouble
NullableDouble	Null

Bool			Bool
Bool			NullableBool

NullableBool	Bool
NullableBool	NullableBool

Null			NullableLong
Null			NullableDouble
Null			NullableBool

============

NullableLong	Null
NullableDouble	Null
NullableBool	Null

Null			NullableLong
Null			NullableDouble
Null			NullableBool

String			String
String			Null
Null			String
Null			Null

*/
		internal static DoubleOperandFunctionMatrix Create()
        {
            var matrix = new DoubleOperandFunctionMatrix();
			//matrix.RegisterDelegate(OperandType.Long, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long)leftOperand.GetValue() != (long)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.Long, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long)leftOperand.GetValue() != (double)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.Double, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double)leftOperand.GetValue() != (long)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.Double, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double)leftOperand.GetValue() != (double)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.Bool, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool)leftOperand.GetValue() != (bool)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.NullableBool, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool?)leftOperand.GetValue() != (bool)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.Bool, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool?)leftOperand.GetValue() != (bool)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.String, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (string)leftOperand.GetValue() != (string)rightOperand.GetValue()));
			//matrix.RegisterDelegate(OperandType.String, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (string)leftOperand.GetValue() != null));
			//matrix.RegisterDelegate(OperandType.NullableBool, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool?)leftOperand.GetValue() != null));


			matrix.RegisterDelegate(OperandType.Long, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long)leftOperand.GetValue() != (long)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Long, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long)leftOperand.GetValue() != (long?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Long, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long)leftOperand.GetValue() != (double)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Long, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long)leftOperand.GetValue() != (double?)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long?)leftOperand.GetValue() != (long)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long?)leftOperand.GetValue() != (long?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long?)leftOperand.GetValue() != (double)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long?)leftOperand.GetValue() != (double?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (long?)leftOperand.GetValue() != null));

			matrix.RegisterDelegate(OperandType.Double, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double)leftOperand.GetValue() != (long)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Double, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double)leftOperand.GetValue() != (long?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Double, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double)leftOperand.GetValue() != (double)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Double, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double)leftOperand.GetValue() != (double?)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double?)leftOperand.GetValue() != (long)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double?)leftOperand.GetValue() != (long?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double?)leftOperand.GetValue() != (double)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double?)leftOperand.GetValue() != (double?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (double?)leftOperand.GetValue() != null));

			matrix.RegisterDelegate(OperandType.Bool, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool)leftOperand.GetValue() != (bool)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Bool, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool)leftOperand.GetValue() != (bool?)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.NullableBool, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool?)leftOperand.GetValue() != (bool)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.NullableBool, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool?)leftOperand.GetValue() != (bool?)rightOperand.GetValue()));

			matrix.RegisterDelegate(OperandType.Null, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.Bool, null != (long?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Null, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.Bool, null != (double?)rightOperand.GetValue()));
			matrix.RegisterDelegate(OperandType.Null, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, null != (bool?)rightOperand.GetValue()));
			
	        matrix.RegisterDelegate(OperandType.String, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (string)leftOperand.GetValue() != (string)rightOperand.GetValue()));
	        matrix.RegisterDelegate(OperandType.String, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (string)leftOperand.GetValue() != null));
	        matrix.RegisterDelegate(OperandType.Null, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.Bool, null != (string)rightOperand.GetValue()));
	        matrix.RegisterDelegate(OperandType.Null, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.Bool, false));

			return matrix;
        }
    }
}
