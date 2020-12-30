using FunctionZero.ExpressionParserZero.Operands;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.BackingStore
{
    public interface IBackingStore
    {
        (OperandType type, object value) GetValue(string qualifiedName);
        void SetValue(string qualifiedName, object value);
    }
}
