using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Parser
{
    public class TokenService
    {
        public static string TokensAsString(IEnumerable<IToken> tokens)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var token in tokens)
            {
                if(token is IOperand operand)
                {
                    sb.Append($"({operand.Type}:{operand.GetValue()}) ");
                }
                else // Operator
                {
                    sb.Append($"[{token.ToString()}] ");
                }
            }


            return sb.ToString();
        }
    }
}