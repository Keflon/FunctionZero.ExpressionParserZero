using FunctionZero.ExpressionParserZero.Operands;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.BackingStore
{
    public static class BackingStoreHelpers
    {
        public static IDictionary<Type, OperandType> OperandTypeLookup = new Dictionary<Type, OperandType>()
        {
            { typeof(sbyte), OperandType.Sbyte},
            { typeof(byte), OperandType.Byte},
            { typeof(short), OperandType.Short},
            { typeof(ushort), OperandType.Ushort},
            { typeof(int), OperandType.Int},
            { typeof(uint), OperandType.Uint},
            { typeof(long), OperandType.Long},
            { typeof(ulong), OperandType.Ulong},
            { typeof(char), OperandType.Char},
            { typeof(float), OperandType.Float},
            { typeof(double), OperandType.Double},
            { typeof(bool), OperandType.Bool},
            { typeof(decimal), OperandType.Decimal},
            { typeof(sbyte?), OperandType.NullableSbyte},
            { typeof(byte?), OperandType.NullableByte},
            { typeof(short?), OperandType.NullableShort},
            { typeof(ushort?), OperandType.NullableUshort},
            { typeof(int?), OperandType.NullableInt},
            { typeof(uint?), OperandType.NullableUint},
            { typeof(long?), OperandType.NullableLong},
            { typeof(ulong?), OperandType.NullableUlong},
            { typeof(char?), OperandType.NullableChar},
            { typeof(float?), OperandType.NullableFloat},
            { typeof(double?), OperandType.NullableDouble},
            { typeof(bool?), OperandType.NullableBool},
            { typeof(decimal?), OperandType.NullableDecimal},
            { typeof(string), OperandType.String},
            //{ typeof(), OperandType.Variable,       },
            //{ typeof(), OperandType.VSet,           },
            //{ typeof(), OperandType.Object},
            //{ typeof(), OperandType.Null }
        };
    }
}
