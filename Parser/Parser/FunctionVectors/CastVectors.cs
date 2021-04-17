
#if false

using FunctionZero.ExpressionParserZero.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Operands;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Parser.FunctionVectors
{
    public static class CastVector
    {
        internal static SingleOperandFunctionVector Create()
        {


            var vector = new SingleOperandFunctionVector();

            vector.RegisterDelegate(OperandType.Sbyte, operand => new Operand(OperandType.Sbyte, (sbyte)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Byte, operand => new Operand(OperandType.Byte, (byte)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Short, operand => new Operand(OperandType.Short, (short)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Ushort, operand => new Operand(OperandType.Ushort, (ushort)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Int, operand => new Operand(OperandType.Int, (int)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Uint, operand => new Operand(OperandType.Uint, (uint)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Long, operand => new Operand(OperandType.Long, (long)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Ulong, operand => new Operand(OperandType.Ulong, (ulong)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Char, operand => new Operand(OperandType.Char, (char)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Float, operand => new Operand(OperandType.Float, (float)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Double, operand => new Operand(OperandType.Double, (double)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Bool, operand => new Operand(OperandType.Bool, (bool)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Decimal, operand => new Operand(OperandType.Decimal, (decimal)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableSbyte, operand => new Operand(OperandType.NullableSbyte, (sbyte?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableByte, operand => new Operand(OperandType.NullableByte, (byte?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableShort, operand => new Operand(OperandType.NullableShort, (short?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableUshort, operand => new Operand(OperandType.NullableUshort, (ushort?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableInt, operand => new Operand(OperandType.NullableInt, (int?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableUint, operand => new Operand(OperandType.NullableUint, (uint?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableLong, operand => new Operand(OperandType.NullableLong, (long?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableUlong, operand => new Operand(OperandType.NullableUlong, (ulong?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableChar, operand => new Operand(OperandType.NullableChar, (char?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableFloat, operand => new Operand(OperandType.NullableFloat, (float?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableDouble, operand => new Operand(OperandType.NullableDouble, (double?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableBool, operand => new Operand(OperandType.NullableBool, (bool?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableDecimal, operand => new Operand(OperandType.NullableDecimal, (decimal?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.String, operand => new Operand(OperandType.String, (string)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Variable, operand => new Operand(OperandType.Variable, (Variable)operand.GetValue()));
            vector.RegisterDelegate(OperandType.VSet, operand => new Operand(OperandType.VSet, (IVariableSet)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Object, operand => new Operand(OperandType.Object, (object)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Null, operand => new Operand(OperandType.Null, (null)operand.GetValue()));

            return vector;
        }
    }
}

#endif
