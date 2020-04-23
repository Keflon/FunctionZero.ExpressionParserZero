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
        public Stack<IOperand> Evaluate(IVariableStore variables)
        {
            return ExpressionEvaluator.Evaluate(this, variables);
        }
    }
}
