using FunctionZero.ExpressionParserZero.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Operands;

namespace FunctionZero.ExpressionParserZero.Parser.FunctionVectors
{
    public static class UnaryMinusVector
    {
        public static SingleOperandFunctionVector Create()
        {
            var vector = new SingleOperandFunctionVector();

            vector.RegisterDelegate(OperandType.Sbyte, operand => new Operand(OperandType.Int, - (sbyte)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Byte, operand => new Operand(OperandType.Int, - (byte)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Short, operand => new Operand(OperandType.Int, - (short)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Ushort, operand => new Operand(OperandType.Int, - (ushort)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Int, operand => new Operand(OperandType.Int, - (int)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Uint, operand => new Operand(OperandType.Long, - (uint)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Long, operand => new Operand(OperandType.Long, - (long)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Char, operand => new Operand(OperandType.Int, - (char)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Float, operand => new Operand(OperandType.Float, - (float)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Double, operand => new Operand(OperandType.Double, - (double)operand.GetValue()));
            vector.RegisterDelegate(OperandType.Decimal, operand => new Operand(OperandType.Decimal, - (decimal)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableSbyte, operand => new Operand(OperandType.NullableInt, - (sbyte?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableByte, operand => new Operand(OperandType.NullableInt, - (byte?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableShort, operand => new Operand(OperandType.NullableInt, - (short?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableUshort, operand => new Operand(OperandType.NullableInt, - (ushort?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableInt, operand => new Operand(OperandType.NullableInt, - (int?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableUint, operand => new Operand(OperandType.NullableLong, - (uint?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableLong, operand => new Operand(OperandType.NullableLong, - (long?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableChar, operand => new Operand(OperandType.NullableInt, - (char?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableFloat, operand => new Operand(OperandType.NullableFloat, - (float?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableDouble, operand => new Operand(OperandType.NullableDouble, - (double?)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableDecimal, operand => new Operand(OperandType.NullableDecimal, - (decimal?)operand.GetValue()));

            return vector;
        }
    }
}
