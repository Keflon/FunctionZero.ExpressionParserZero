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
using FunctionZero.ExpressionParserZero.Tokens;

namespace FunctionZero.ExpressionParserZero.Operands
{
    public interface IOperand : IToken
    {
        object GetValue();
        bool IsNumber { get; }
        OperandType Type { get; }
    }

    public enum OperandType
    {
        //      Long = 0,
        //NullableLong,
        //      Double,
        //NullableDouble,
        //String,
        //Variable,
        //      Bool,
        //      NullableBool,
        //      VSet,
        //      Object, 

        Sbyte,
        Byte,
        Short,
        Ushort,
        Int,
        Uint,
        Long,
        Ulong,
        Char,
        Float,
        Double,
        Bool,
        Decimal,
        NullableSbyte,
        NullableByte,
        NullableShort,
        NullableUshort,
        NullableInt,
        NullableUint,
        NullableLong,
        NullableUlong,
        NullableChar,
        NullableFloat,
        NullableDouble,
        NullableBool,
        NullableDecimal,
        String,
        Variable,           // TODO: Derive a VariableOperand?
        VSet,               // Wierd old uncle.
        Object, 
        Null
    }
}

/*

Sbyte	
Byte	
Short	
Ushort	
Int	    
Uint	
Long	
Ulong	
Char	
Float	
Double	
Bool	
Decimal	

NullableSbyte	
NullableByte	
NullableShort	
NullableUshort	
NullableInt	    
NullableUint	
NullableLong	
NullableUlong	
NullableChar	
NullableFloat	
NullableDouble	
NullableBool	
NullableDecimal	

*/