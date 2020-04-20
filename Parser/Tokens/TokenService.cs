using FunctionZero.ExpressionParserZero.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Parser
{
    public class TokenService
    {
        public static string TokensAsString(IEnumerable<IToken> tokens, bool terse = false)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var token in tokens)
                if (terse)
                    sb.Append($"[{token.ToString()}]");
                else
                    sb.Append($"[{token.TokenType}:{token.ToString()}]");

            return sb.ToString();
        }
    }
}