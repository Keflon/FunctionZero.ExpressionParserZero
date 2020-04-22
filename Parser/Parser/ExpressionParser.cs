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
using System.Diagnostics;
using System.IO;
using System.Text;
using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Parser.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Parser.FunctionVectors;
using FunctionZero.ExpressionParserZero.Tokens;
using FunctionZero.ExpressionParserZero.Variables;

//using Windows.Storage.Streams;

namespace FunctionZero.ExpressionParserZero.Parser
{
    // TODO: 
    // Functions - count parameters. Validate that count.
    // Function Matrices / Vectors. Flesh them out.

    // DONE:
    // Add type string, tokenise a quoted string.
    // Add support to operators for strings and bools where appropriate.
    // Trap error location if operator fails, e.g. if 'Add' is given a bool and a string.
    // - Items in the resultant IToken list need to know their parser location, though the list can repeat Token objects, therefore we need a List of TokenWrappers where the wrapper is unique and holds a parser position.
    // Finish writing operators.

    public class ExpressionParser
    {
        enum State
        {
            None = 0,
            Operand,
            Operator,
            UnaryOperator,
            FunctionOperator,
            OpenParenthesis,
            CloseParenthesis
        }

        public const int FunctionPrecedence = 13;

        public ExpressionParser()
        {
            Operators = new Dictionary<string, IOperator>();
            OperatorVectors = new Dictionary<IOperator, SingleOperandFunctionVector>();
            OperatorMatrices = new Dictionary<IOperator, DoubleOperandFunctionMatrix>();
            // Got operator precedence from here: http://www.tutorialspoint.com/csharp/csharp_operators_precedence.htm

            // This is better as it includes functions: http://en.cppreference.com/w/c/language/operator_precedence

            // Register UnaryMinus ...
            UnaryMinus = RegisterUnaryOperator("UnaryMinus", 12, UnaryMinusVector.Create());
            UnaryPlus = RegisterUnaryOperator("UnaryPlus", 12, null);
            RegisterUnaryOperator("!", 12, UnaryNotVector.Create());
            RegisterUnaryOperator("~", 12, UnaryComplementVector.Create());
            RegisterOperator("*", 11, MultiplyMatrix.Create());
            RegisterOperator("/", 11, DivideMatrix.Create());
            RegisterOperator("%", 11, ModuloMatrix.Create());
            PlusOperator = RegisterOperator("+", 10, AddMatrix.Create());
            MinusOperator = RegisterOperator("-", 10, SubtractMatrix.Create());
            RegisterOperator("<", 9, LessThanMatrix.Create());
            RegisterOperator(">", 9, GreaterThanMatrix.Create());
            RegisterOperator(">=", 9, GreaterThanOrEqualMatrix.Create());
            RegisterOperator("<=", 9, LessThanOrEqualsMatrix.Create());
            RegisterOperator("!=", 8, NotEqualMatrix.Create());
            RegisterOperator("==", 8, EqualityMatrix.Create());
            RegisterOperator("&", 7, BitwiseAndMatrix.Create());
            RegisterOperator("^", 6, BitwiseXorMatrix.Create());
            RegisterOperator("|", 5, BitwiseOrMatrix.Create());
            RegisterOperator("&&", 4, LogicalAndMatrix.Create());
            RegisterOperator("||", 3, LogicalOrMatrix.Create());

            // Register operators ...
            RegisterSetEqualsOperator("=", 2, SetEqualsMatrix.Create()); // Can do assignment to a variable.
            CommaOperator = RegisterOperator(",", 1, null); // Do nothing. Correct???
            OpenParenthesisOperator = RegisterOperator("(", 0, null, OperatorType.OpenParenthesis);
            CloseParenthesisOperator = RegisterOperator(")", 13, null, OperatorType.CloseParenthesis);

            // Register functions ...
            Functions = new Dictionary<string, IOperator>();
            RegisterFunction("_debug_mul", OperatorActions.DoMultiply, 2);
            // TODO: What should the precedence be? I don't think it matters because it is never read.
        }

        private Dictionary<string, IOperator> Operators { get; }
        private Dictionary<IOperator, DoubleOperandFunctionMatrix> OperatorMatrices { get; }
        private Dictionary<IOperator, SingleOperandFunctionVector> OperatorVectors { get; }
        private Dictionary<string, IOperator> Functions { get; }

        private IOperator UnaryMinus { get; }
        private IOperator UnaryPlus { get; }
        private IOperator PlusOperator { get; }
        private IOperator MinusOperator { get; }
        private IOperator CommaOperator { get; }
        private IOperator OpenParenthesisOperator { get; }
        private IOperator CloseParenthesisOperator { get; }


        public IOperator RegisterOperator(string text, int precedence, DoubleOperandFunctionMatrix matrix,
            OperatorType operatorType = OperatorType.Operator)
        {
            var op = new Operator(operatorType,
                precedence,
                (operandStack, vSet, parserPosition) =>
                {
                    var result = OperatorActions.DoOperation(matrix, operandStack, vSet);
                    if (result != null)
                        throw new ExpressionEvaluatorException(
                            parserPosition,
                            ExpressionEvaluatorException.ExceptionCause.BadOperand,
                            "Operator '" + text + "' cannot be applied to operands of type " + result.Item1 + " and " + result.Item2);
                }, text
            );
            Operators.Add(text, op);
            OperatorMatrices.Add(op, matrix);
            return op;
        }

        public IOperator GetNamedOperator(string strName)
        {
            return Operators[strName];
        }

        public IOperator RegisterSetEqualsOperator(string text, int precedence, DoubleOperandFunctionMatrix matrix)
        {
            var op = new Operator(OperatorType.Operator, precedence,

                        (operandStack, vSet, parserPosition) =>
                        {
                            var result = OperatorActions.DoSetEquals(matrix, operandStack, vSet, parserPosition);
                            // DoSetEquals throws its own exception.
                        }
				
				
				, text);
            Operators.Add(text, op);
            OperatorMatrices.Add(op, matrix);
            return op;
        }

        public IOperator RegisterUnaryOperator(string text, int precedence, SingleOperandFunctionVector vector)
        {
            var op = new Operator(
                OperatorType.UnaryOperator,
                precedence,
                (operandStack, vSet, parserPosition) =>
                {

                    var result = OperatorActions.DoUnaryOperation(vector, operandStack, vSet);
                    if (result != null)
                    {
                        throw new ExpressionEvaluatorException(parserPosition,
                            ExpressionEvaluatorException.ExceptionCause.BadUnaryOperand,
                            "Unary operator '" + text + "' cannot be applied to operand of type " + result.Item1);

                    }
                },
                text
            );
            Operators.Add(text, op);
            OperatorVectors.Add(op, vector);
            return op;
        }

        public IFunctionOperator RegisterFunction(string text, Action<Stack<IOperand>, IVariableStore, long> doOperation,
            int parameterCount, int maxParameterCount = 0)
        {
            var op = new FunctionOperator(OperatorType.Function, doOperation, text, parameterCount, maxParameterCount);
            Functions.Add(text, op);
            return op;
        }

        public IList<IToken> Parse(string expression)
        {
            return Parse(new MemoryStream(Encoding.UTF8.GetBytes(expression ?? "")));
        }

        private State _state;
        private int _parenthesisDepth;

        //public IList<IToken> TestTokenValidator(string expression)
        //{
        //	_parenthesisDepth = 0;

        //	Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(expression ?? ""));
        //	Input = new StreamReader(inputStream);

        //	IList<IToken> tokenList = new List<IToken>();
        //	IToken token = null;

        //	while((token = GetAndValidateNextToken()) != null)
        //	{
        //		tokenList.Add(token);

        //	}
        //	return tokenList;
        //}


        public IList<IToken> Parse(Stream inputStream)
        {
            _parenthesisDepth = 0;
            var tokenizer = new Tokenizer(inputStream, Operators, Functions);

            var operatorStack = new Stack<OperatorWrapper>();
            IList<IToken> tokenList = new List<IToken>();
            _state = State.None;

            var parserPosition = tokenizer.ParserPosition;
            IToken token;
            while ((token = tokenizer.GetNextToken()) != null)
            {
                if (token is IOperator)
                    token = new OperatorWrapper(TranslateOperator((IOperator)token), tokenizer.Anchor);

                ValidateNextToken(token);

                _state = GetState(token);

                // TokenWrapper is Operand or OperatorWrapper. Nothing else.
                Debug.Assert((token is Operand) || (token is OperatorWrapper));

                OperatorWrapper operatorWrapper = token as OperatorWrapper;

                switch (token.TokenType)
                {
                    case TokenType.Operator:
                        switch (((IOperator)token).Type)
                        {
                            case OperatorType.Operator:
                                {
                                    // Pop operators with precedence >= current operator.
                                    Debug.Assert(operatorWrapper != null);
                                    PopByPrecedence(operatorStack, tokenList, operatorWrapper.WrappedOperator.Precedence);
                                    if (operatorWrapper.WrappedOperator != CommaOperator)
                                        operatorStack.Push(operatorWrapper);
                                }
                                _state = State.Operator;

                                break;
                            case OperatorType.UnaryOperator:
                                operatorStack.Push(operatorWrapper);
                                break;
                            case OperatorType.Function:
                                operatorStack.Push(operatorWrapper);
                                break;
                            case OperatorType.OpenParenthesis:
                                _parenthesisDepth++;
                                operatorStack.Push(operatorWrapper);
                                //if(lastTokenWrapper?.WrappedToken.TokenType == TokenType.Function)
                                //	operatorWrapper.Tag = lastTokenWrapper.WrappedToken;			// Set the OpenParenthesis wrapper Tag to the function that precedes it.
                                break;
                            case OperatorType.CloseParenthesis:
                                _parenthesisDepth--;
                                // Pop operators until an open-parenthesis is encountered.
                                PopByPrecedence(operatorStack, tokenList, 1);
                                operatorStack.Pop(); // Pop the open parenthesis.
                                break;
                        }

                        break;
                    case TokenType.Operand:
                        switch (((IOperand)token).Type)
                        {

                            case OperandType.Long:
                            case OperandType.Double:
                            case OperandType.String:
                            case OperandType.Bool:
                            case OperandType.Variable:
                            case OperandType.Null:
                                tokenList.Add(token);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                }
            }

            // Cannot end with an operator.
            if (_state == State.Operator || _state == State.UnaryOperator)
                throw new ExpressionParserException(tokenizer.ParserPosition,
                    ExpressionParserException.ExceptionCause.OperandExpected);
            if (_state == State.FunctionOperator)
                throw new ExpressionParserException(tokenizer.ParserPosition,
                    ExpressionParserException.ExceptionCause.OpenParenthesisExpected);
            // Check parenthesis
            if (_parenthesisDepth != 0)
                throw new ExpressionParserException(tokenizer.ParserPosition,
                    ExpressionParserException.ExceptionCause.ClosingBraceExpected);

            PopByPrecedence(operatorStack, tokenList, 0);

            return tokenList;
        }

        /// <summary>
        /// Depending on the current parser state, a + or - operator might need to be translated to a unary + or -
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        private IOperator TranslateOperator(IOperator op)
        {
            if ((op == MinusOperator) &&
                ((_state == State.Operator) || (_state == State.UnaryOperator) || (_state == State.None) ||
                 (_state == State.OpenParenthesis)))
            {
                return UnaryMinus;
            }
            else if ((op == PlusOperator) &&
                     ((_state == State.Operator) || (_state == State.UnaryOperator) || (_state == State.None) ||
                      (_state == State.OpenParenthesis)))
            {
                return UnaryPlus;
            }
            else
            {
                return op;
            }
        }

        private void ValidateNextToken(IToken token)
        {
            if (token == null)
                throw new ExpressionParserException(token.ParserPosition,
                    ExpressionParserException.ExceptionCause.UnexpectedEndOfStream);
            //return null;

            TokenType tokenType = token.TokenType;

            switch (tokenType)
            {
                case TokenType.Operator:
                    switch (((IOperator)token).Type)
                    {
                        case OperatorType.Operator: // + - * /
                                                    // Can follow Operand, CloseParenthesis
                            switch (_state)
                            {
                                case State.None:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedOperator);
                                case State.Operand:
                                    break;
                                case State.Operator:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedOperator);
                                case State.UnaryOperator:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedOperator);
                                case State.FunctionOperator:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.OpenParenthesisExpected);
                                case State.OpenParenthesis:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedOperator);
                                case State.CloseParenthesis:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            break;
                        case OperatorType.UnaryOperator: // UnaryMinus UnaryPlus ! ^
                                                         // Can follow None, Operator, UnaryOperator, OpenParenthesis
                            switch (_state)
                            {
                                case State.None:
                                    break;
                                case State.Operand:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedUnaryOperator);
                                case State.Operator:
                                    break;
                                case State.UnaryOperator:
                                    break;
                                case State.FunctionOperator:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedUnaryOperator);
                                case State.OpenParenthesis:
                                    break;
                                case State.CloseParenthesis:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedUnaryOperator);
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            break;
                        case OperatorType.Function: // mul StringContains
                                                    // Can follow None, Operator, UnaryOperator, OpenParenthesis
                            switch (_state)
                            {
                                case State.None:
                                    break;
                                case State.Operand:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedFunctionCall);
                                case State.Operator:
                                    break;
                                case State.UnaryOperator:
                                    break;
                                case State.FunctionOperator:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.OpenParenthesisExpected);
                                case State.OpenParenthesis:
                                    break;
                                case State.CloseParenthesis:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedFunctionCall);
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            break;
                        case OperatorType.OpenParenthesis: // (
                                                           // Can follow None, Operator, UnaryOperator, FunctionOperator, OpenParenthesis
                                                           //_parenthesisDepth++;

                            switch (_state)
                            {
                                case State.None:
                                    break;
                                case State.Operand:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedOpenParenthesis);
                                case State.Operator:
                                    break;
                                case State.UnaryOperator:
                                    break;
                                case State.FunctionOperator:
                                    break;
                                case State.OpenParenthesis:
                                    break;
                                case State.CloseParenthesis:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedOpenParenthesis);
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            break;
                        case OperatorType.CloseParenthesis: // )
                                                            // Can follow Operand, OpenParenthesis, CloseParenthesis

                            if (_parenthesisDepth == 0)
                                throw new ExpressionParserException(token.ParserPosition,
                                    ExpressionParserException.ExceptionCause.UnmatchedClosingBrace);
                            //_parenthesisDepth--;

                            switch (_state)
                            {
                                case State.None:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedCloseParenthesis);
                                case State.Operand:
                                    break;
                                case State.Operator:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedCloseParenthesis);
                                case State.UnaryOperator:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedCloseParenthesis);
                                case State.FunctionOperator:
                                    throw new ExpressionParserException(token.ParserPosition,
                                        ExpressionParserException.ExceptionCause.UnexpectedCloseParenthesis);
                                case State.OpenParenthesis:
                                    break;
                                case State.CloseParenthesis:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            break;
                    }

                    break;
                case TokenType.Operand:
                    {
                        switch (((IOperand)token).Type)
                        {
                            // Operands ...
                            case OperandType.Long:
                            case OperandType.Double:
                            case OperandType.String:
                            case OperandType.Bool:
                            case OperandType.Variable: // 5, 5.5, "5", True, Eggs
                                                       // Can follow None, Operator, UnaryOperator, OpenParenthesis
                            case OperandType.Null:
                                switch (_state)
                                {
                                    case State.None:
                                        break;
                                    case State.Operand:
                                        throw new ExpressionParserException(token.ParserPosition,
                                            ExpressionParserException.ExceptionCause.UnexpectedOperand);
                                    case State.Operator:
                                        break;
                                    case State.UnaryOperator:
                                        break;
                                    case State.FunctionOperator:
                                        throw new ExpressionParserException(token.ParserPosition,
                                            ExpressionParserException.ExceptionCause.OpenParenthesisExpected);
                                    case State.OpenParenthesis:
                                        break;
                                    case State.CloseParenthesis:
                                        throw new ExpressionParserException(token.ParserPosition,
                                            ExpressionParserException.ExceptionCause.UnexpectedOperand);
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    }
            }
        }

        private State GetState(IToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Operator:
                    switch (((IOperator)token).Type)
                    {
                        //case TokenType.Undefined:
                        //	break;
                        case OperatorType.Operator:
                            return State.Operator;
                        case OperatorType.UnaryOperator:
                            return State.UnaryOperator;
                        case OperatorType.Function:
                            return State.FunctionOperator;
                        case OperatorType.OpenParenthesis:
                            return State.OpenParenthesis;
                        case OperatorType.CloseParenthesis:
                            return State.CloseParenthesis;
                    }

                    break;
                case TokenType.Operand:
                    switch (((IOperand)token).Type)
                    {
                        case OperandType.Long:
                        case OperandType.Double:
                        case OperandType.String:
                        case OperandType.Bool:
                        case OperandType.Variable:
                        case OperandType.Null:
                            return State.Operand;
                    }

                    break;
            }

            throw new ArgumentOutOfRangeException();
        }


        private void PopByPrecedence(Stack<OperatorWrapper> operatorStack, IList<IToken> tokenList,
            int currentPrecedence)
        {
            while (operatorStack.Count > 0 && operatorStack.Peek().WrappedOperator.Precedence >= currentPrecedence)
            {
                tokenList.Add(operatorStack.Pop());
            }
        }

        public void RegisterOverload(string operatorName, OperandType left, OperandType right, DoubleOperandDelegate func)
        {
            if (Operators.TryGetValue(operatorName, out IOperator op))
            {
                if (op.Type == OperatorType.Operator)
                    OperatorMatrices[op].RegisterDelegate(left, right, func);
                else
                    throw new ExpressionParserException(-1, ExpressionParserException.ExceptionCause.DoubleOperandOperatorNotFound,
                        "'" + operatorName + "' is a unary operator");
            }
            else
                throw new ExpressionParserException(-1, ExpressionParserException.ExceptionCause.DoubleOperandOperatorNotFound,
                    "'" + operatorName + "' is not a registered operator");
        }

        public void RegisterOverload(string operatorName, OperandType type, SingleOperandDelegate func)
        {
            if (Operators.TryGetValue(operatorName, out IOperator op))
            {
                if (op.Type == OperatorType.UnaryOperator)
                    OperatorVectors[op].RegisterDelegate(type, func);
                else
                    throw new ExpressionParserException(-1, ExpressionParserException.ExceptionCause.UnaryOperatorNotFound,
                        "'" + operatorName + "' is not a unary operator");
            }
            else
                throw new ExpressionParserException(-1, ExpressionParserException.ExceptionCause.UnaryOperatorNotFound,
                    "'" + operatorName + "' is not a registered operator");
        }
    }
}