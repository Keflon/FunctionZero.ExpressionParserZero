using FunctionZero.ExpressionParserZero.Operands;
using System.Collections.Generic;

namespace FunctionZero.ExpressionParserZero.Parser
{
    public class OperandStack : Stack<IOperand>
    {
        public override string ToString()
        {
            return TokenService.TokensAsString(this);
        }

        public string ToShortString()
        {
            return TokenService.TokensAsString(this);
        }
    }
}