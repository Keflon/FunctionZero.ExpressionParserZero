using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Tokens;
using FunctionZero.ExpressionParserZero.Variables;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Parser
{
    public class TokenList : List<IToken>
    {
        public OperandStack Evaluate(IVariableStore variables)
        {
            return ExpressionEvaluator.Evaluate(this, variables);
        }

        public override string ToString()
        {
            return TokenService.TokensAsString(this);
        }
    }
}
