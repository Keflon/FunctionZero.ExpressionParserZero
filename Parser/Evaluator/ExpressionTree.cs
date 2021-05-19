using FunctionZero.ExpressionParserZero.Operators;
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
            var stack = new Stack<ExpressionTreeNode>();

            foreach (var item in rpnTokens)
            {
                var node = new ExpressionTreeNode(item);
                int count = GetOperandCount(item);

                while (count-- != 0)
                    node.AddChild(stack.Pop());
                
                stack.Push(node);
            }
            RootNode = stack.Pop();
        }

        public ExpressionTreeNode RootNode { get; }

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
    }

    public class ExpressionTreeNode
    {
        public ExpressionTreeNode(IToken token)
        {
            Token = token;
            Children = new List<ExpressionTreeNode>();
        }

        public IToken Token { get; }
        public List<ExpressionTreeNode> Children { get; }

        public void AddChild(ExpressionTreeNode node)
        {
            this.Children.Add(node);
        }
    }
}
