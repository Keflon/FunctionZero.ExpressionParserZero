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
using System;
using System.Collections.Generic;
using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Evaluator;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Tokens;

namespace FunctionZero.ExpressionParserZero
{
    public static class ExpressionEvaluator
    {
#if false
		public static OperandStack Evaluate(IEnumerable<IToken> rpnTokens, IBackingStore backingStore)
		{
			var operandStack = new OperandStack();

			foreach (IToken token in rpnTokens)
			{
				if (token.TokenType == TokenType.Operand)
				{
					operandStack.Push((IOperand)token);
				}
				else
				{
					((OperatorWrapper)token).WrappedOperator.DoOperation(operandStack, backingStore, token.ParserPosition);
				}
			}
			return operandStack;
		}
#else
        public static OperandStack Evaluate(IEnumerable<IToken> rpnTokens, IBackingStore backingStore)
        {
            var tree = new ExpressionTree(rpnTokens);
            var operandStack = new OperandStack();

            foreach (var node in tree.RootNodeList)
                Evaluate(node, operandStack, backingStore);

            var resultStack = new OperandStack();

            while (operandStack.Count != 0)
                resultStack.Push(operandStack.Pop());

            return resultStack;
        }
        public static OperandStack Evaluate(ExpressionTree tree, IBackingStore backingStore)
        {
            var operandStack = new OperandStack();

            for (int c = tree.RootNodeList.Count - 1; c >= 0; c--)
                Evaluate(tree.RootNodeList[c], operandStack, backingStore);

            return operandStack;
        }

        private static void Evaluate(ExpressionTreeNode node, OperandStack operandStack, IBackingStore backingStore)
        {
            var token = node.Token;

            if (token.TokenType == TokenType.Operand)
            {
                operandStack.Push((IOperand)token);
            }
            else
            {
                var op = ((OperatorWrapper)token).WrappedOperator;
                if(op.ShortCircuit == ShortCircuitMode.LogicalAnd)
                {
                    Evaluate(node.Children[1], operandStack, backingStore);

                    var thing = OperatorActions.PeekAndResolve(operandStack, backingStore);

                    if ((bool)thing.GetValue() == false)
                    {
                        // The operator has done its work, discard it.
                        operandStack.Pop();
                        operandStack.Push(new Operand(OperandType.Bool, false));
                        return;
                    }
                    else
                        Evaluate(node.Children[0], operandStack, backingStore);
                }
                else if(op.ShortCircuit == ShortCircuitMode.LogicalOr)
                {
                    Evaluate(node.Children[1], operandStack, backingStore);

                    var thing = OperatorActions.PeekAndResolve(operandStack, backingStore);

                    if ((bool)thing.GetValue() == true)
                    {
                        // The operator has done its work, discard it.
                        operandStack.Pop();
                        operandStack.Push(new Operand(OperandType.Bool, true));
                        return;
                    }
                    else
                        Evaluate(node.Children[0], operandStack, backingStore);
                }
                else
                {
                    // TODO: Build it backwards so we can just foreach here.
                    for (int c = node.Children.Count - 1; c >= 0; c--)
                    {
                        Evaluate(node.Children[c], operandStack, backingStore);
                    }
                }
               op.DoOperation(operandStack, backingStore, token.ParserPosition);
            }
        }
#endif
    }
}
