using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Parser.FunctionMatrices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Binding
{
    public class ExpressionParserFactory
    {
        private static ExpressionParser _expressionParser;

        public static ExpressionParser GetExpressionParser()
        {
            if (_expressionParser == null)
            {
                var parser = new ExpressionParser();
                DefaultRegistrations.RegisterDefaultAliases(parser);
                DefaultRegistrations.RegisterDefaultFunctions(parser);
                ReplaceDefaultExpressionParser(parser, false);
            }
            return _expressionParser;
        }

        public static void ReplaceDefaultExpressionParser(ExpressionParser replacement, bool allowLateReplacement)
        {
            if (_expressionParser == null || allowLateReplacement)
            {
                _expressionParser = replacement;
            }
            else
            {
                throw new InvalidOperationException("Attempt to replace default ExpressionParser after it has created been created for a consumer");
            }
        }
    }
}
