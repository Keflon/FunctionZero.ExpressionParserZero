#region License
// Author: Keith Pickford
// 
// MIT License
// 
// Copyright (c) 2016 FunctionZero Ltd
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
using System.Collections.Generic;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Tokens;
using FunctionZero.ExpressionParserZero.Variables;

namespace FunctionZero.ExpressionParserZero
{
	public static class ExpressionEvaluator
	{
		public static Stack<IOperand> Evaluate(IEnumerable<IToken> rpnTokens, IVariableStore variables)
		{
			Stack<IOperand> operandStack = new Stack<IOperand>();
			foreach(IToken token in rpnTokens)
			{
				if(token.TokenType == TokenType.Operand)
				{
					operandStack.Push((IOperand)token);
				}
				else
				{
					((OperatorWrapper)token).WrappedOperator.DoOperation(operandStack, variables, token.ParserPosition);
				}
			}
			return operandStack;
		}
	}
}
