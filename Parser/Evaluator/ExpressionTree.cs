﻿using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Binding;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Evaluator
{
    public class ExpressionTree
    {
        public ExpressionTree(IEnumerable<IToken> rpnTokens)
        {
            RpnTokens = rpnTokens;
            var stack = new Stack<ExpressionTreeNode>();

            foreach (var item in rpnTokens)
            {
                int count = GetOperandCount(item);
                var node = new ExpressionTreeNode(item, count);

                while (count-- != 0)
                    node.AddChild(stack.Pop());
                
                stack.Push(node);
            }
            RootNodeList = new List<ExpressionTreeNode>(stack);
        }

        public List<ExpressionTreeNode> RootNodeList { get; }
        public IEnumerable<IToken> RpnTokens { get; }

        private int GetOperandCount(IToken item)
        {
            switch (item.TokenType)
            {
                case TokenType.Operator:
                    {
                        var op = (IOperator)item;
                        switch (op.Type)
                        {
                            case OperatorType.Operator:
                                return 2;
                            case OperatorType.UnaryOperator:
                                return 1;
                            case OperatorType.Function:
                                return ((IFunctionOperator)((OperatorWrapper)op).WrappedOperator).ActualParameterCount;
                            case OperatorType.UnaryCastOperator:
                                return 1;
                            case OperatorType.OpenParenthesis:
                            case OperatorType.CloseParenthesis:
                            default:
                                throw new InvalidOperationException("For now");
                        }
                    }
                case TokenType.Operand:
                default:
                    return 0;
            }
        }

        //internal object Evaluate(VariableEvaluator evaluator)
        //{
        //    throw new NotImplementedException();
        //}
        public OperandStack Evaluate(IBackingStore backingStore)
        {
            return ExpressionEvaluator.Evaluate(this, backingStore);
        }
    }

    public class ExpressionTreeNode
    {
        public ExpressionTreeNode(IToken token, int parameterCount)
        {
            Token = token;
            ParameterCount = parameterCount;
            Children = new List<ExpressionTreeNode>();
        }

        public IToken Token { get; }
        public int ParameterCount { get; }
        public List<ExpressionTreeNode> Children { get; }

        public void AddChild(ExpressionTreeNode node)
        {
            this.Children.Add(node);
        }
    }
}
