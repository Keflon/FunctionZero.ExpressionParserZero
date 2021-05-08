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
using System.Diagnostics;
using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Tokens;

namespace FunctionZero.ExpressionParserZero.Operands
{
    public class Operand : IOperand, IToken
    {
        private const long Bogey = -1;

        public Operand(OperandType operandType, object operandValue) : this(Bogey, operandType, operandValue)
        {

        }

        internal Operand(OperandType operandType)
        {
            Type = operandType;
            OperandValue = null;
            ParserPosition = -1;
        }

        public Operand(long parserPosition, OperandType operandType, object operandValue)
        {
            Debug.Assert(CheckValueType(operandType, operandValue) == true);

            Type = operandType;
            OperandValue = operandValue;
            ParserPosition = parserPosition;
        }

        public long ParserPosition { get; }
        public OperandType Type { get; }
        private object OperandValue { get; }

        public bool IsNumber => (Type == OperandType.Double) || (Type == OperandType.Long);

        public TokenType TokenType => TokenType.Operand;

        public object GetValue()
        {
            Debug.Assert(CheckValueType(Type, OperandValue) == true);

            return OperandValue;
        }

        private bool OldCheckValueType(OperandType tokenType, object tokenValue)
        {
            switch (tokenType)
            {
                case OperandType.Long:
                    return tokenValue is long;

                case OperandType.NullableLong:
                    return tokenValue == null || tokenValue is long;

                case OperandType.Double:
                    return tokenValue is double;

                case OperandType.NullableDouble:
                    return tokenValue == null || tokenValue is double;

                case OperandType.String:
                    return tokenValue is string;

                case OperandType.Variable:                // TODO: Make this a reference to a variable? Possible? It might not resolve.
                    return tokenValue is string;

                case OperandType.Bool:
                    return tokenValue is bool;

                case OperandType.NullableBool:
                    return tokenValue == null || tokenValue is bool;

                case OperandType.VSet:
                    return tokenValue is IBackingStore;

                case OperandType.Object:
                    return true;

                case OperandType.Null:
                    return tokenValue == null;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CheckValueType(OperandType tokenType, object tokenValue)
        {
            switch (tokenType)
            {
                case OperandType.Sbyte:
                    return tokenValue is sbyte;
                case OperandType.Byte:
                    return tokenValue is byte;
                case OperandType.Short:
                    return tokenValue is short;
                case OperandType.Ushort:
                    return tokenValue is ushort;
                case OperandType.Int:
                    return tokenValue is int;
                case OperandType.Uint:
                    return tokenValue is uint;
                case OperandType.Long:
                    return tokenValue is long;
                case OperandType.Ulong:
                    return tokenValue is ulong;
                case OperandType.Char:
                    return tokenValue is char;
                case OperandType.Float:
                    return tokenValue is float;
                case OperandType.Double:
                    return tokenValue is double;
                case OperandType.Bool:
                    return tokenValue is bool;
                case OperandType.Decimal:
                    return tokenValue is decimal;
                case OperandType.NullableSbyte:
                    return tokenValue == null || tokenValue is sbyte;
                case OperandType.NullableByte:
                    return tokenValue == null || tokenValue is byte;
                case OperandType.NullableShort:
                    return tokenValue == null || tokenValue is short;
                case OperandType.NullableUshort:
                    return tokenValue == null || tokenValue is ushort;
                case OperandType.NullableInt:
                    return tokenValue == null || tokenValue is int;
                case OperandType.NullableUint:
                    return tokenValue == null || tokenValue is uint;
                case OperandType.NullableLong:
                    return tokenValue == null || tokenValue is long;
                case OperandType.NullableUlong:
                    return tokenValue == null || tokenValue is ulong;
                case OperandType.NullableChar:
                    return tokenValue == null || tokenValue is char;
                case OperandType.NullableFloat:
                    return tokenValue == null || tokenValue is float;
                case OperandType.NullableDouble:
                    return tokenValue == null || tokenValue is double;
                case OperandType.NullableBool:
                    return tokenValue == null || tokenValue is bool;
                case OperandType.NullableDecimal:
                    return tokenValue == null || tokenValue is decimal;
                case OperandType.String:
                    return tokenValue == null || tokenValue is string;
                case OperandType.Variable:                // TODO: Make this a reference to a variable? Possible? It might not resolve.
                    return tokenValue is string;
                case OperandType.Object:
                    return true;
                case OperandType.Null:
                    return tokenValue == null;

                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public override string ToString()
        {
            return OperandValue.ToString();
        }
    }
}
