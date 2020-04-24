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