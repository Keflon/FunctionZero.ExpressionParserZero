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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Tokens;

namespace FunctionZero.ExpressionParserZero.Parser
{
    public class Tokenizer
    {
        private readonly Dictionary<string, IOperator> _operators;

        private readonly Dictionary<string, IOperator> _functions;
        private readonly StreamReader _inputReader;
        public int ParserPosition { get; private set; }
        public int Anchor { get; private set; }

        public delegate bool TryGetOperator(string strToken, out IOperator op);

        public Tokenizer(Stream inputStream, Dictionary<string, IOperator> operators, Dictionary<string, IOperator> functions)
        {
            _operators = operators;
            _functions = functions;
            _inputReader = new StreamReader(inputStream);
            ParserPosition = 0;
        }

        public IToken GetNextToken()
        {
            SkipWhiteSpace();
            Anchor = ParserPosition;

            char nextChar = PeekNextChar();
            if (nextChar == 0)
                return null;

            IOperator op;

            if ((char.IsLetter(nextChar)) || nextChar == '_')
            {
                // It looks like a symbol.
                string strToken = ParseSymbolToken();

                if (strToken == "True" || strToken == "true" || strToken == "False" || strToken == "false")
                {
                    return new Operand(Anchor, OperandType.Bool, bool.Parse(strToken));
                }
                else if (strToken == "Null" || strToken == "null")
                {
                    return new Operand(Anchor, OperandType.Null, null);
                }
                else if (_operators.TryGetValue(strToken, out op))
                {
                    return op;
                }

                else if (_functions.TryGetValue(strToken, out var functionToken))
                {
                    return functionToken;

                }
                else
                {
                    return new Operand(Anchor, OperandType.Variable, strToken);
                }
            }
            else if (char.IsDigit(nextChar) || nextChar == '.')
            {
                // It looks like a number.
                return ParseNumberToken(Anchor);
            }
            else if (nextChar == '\"' || nextChar == '\'')
            {
                // It looks like a string.
                return ParseStringToken(Anchor);
            }
            else
            {
                string nextToken = nextChar.ToString();
                string strToken = string.Empty;
                // See if it's an operator.
                while (AnyOperatorStartsWith(nextToken))
                {
                    strToken = nextToken;
                    GetNextChar();
                    if (PeekNextChar() == 0)
                        break;
                    nextToken += PeekNextChar().ToString();
                }

                if (_operators.TryGetValue(strToken, out op))
                {
                    return op;
                }
            }
            throw new ExpressionParserException(Anchor, ExpressionParserException.ExceptionCause.BadToken);
        }

        /// <summary>
        ///     Reads a character from the input stream.
        /// </summary>
        /// <returns>Returns the next non-whitespace character or 0 if EOF reached.</returns>
        private char GetNextChar()
        {
            char ch = (char)_inputReader.Read();
            if (ch == 65535)
                return (char)0;
            ParserPosition++;
            return ch;
        }

        private int SkipWhiteSpace()
        {
            int retval = 0;
            while (char.IsWhiteSpace((char)_inputReader.Peek()))
            {
                _inputReader.Read();
                ParserPosition++;
                retval++;
            }
            return retval;
        }

        /// <summary>
        ///     Reads a character from the input stream.
        /// </summary>
        /// <returns>Returns the next non-whitespace character or 0 if EOF reached.</returns>
        private char PeekNextChar()
        {
            if ((char)_inputReader.Peek() == 65535)
                return (char)0;

            return (char)_inputReader.Peek();
        }

        private IOperand ParseNumberToken(long anchor)
        {
            Debug.Assert(anchor == ParserPosition);

            string number = string.Empty;
            bool hasDecimal = false;
            char ch = PeekNextChar();
            while (char.IsDigit(ch) || ch == '.')
            {
                if (ch == '.')
                {
                    if (hasDecimal)
                        //throw new ArgumentException("Multiple decimal points at parser position " + ParserPosition);
                        throw new ExpressionParserException(ParserPosition, ExpressionParserException.ExceptionCause.MultipleDecimalPoint);
                    hasDecimal = true;
                }
                number += GetNextChar();
                ch = PeekNextChar();
            }
            if (number == ".")
                throw new ExpressionParserException(ParserPosition, ExpressionParserException.ExceptionCause.InvalidOperand);

            if ((char.IsLetter(ch)) || ch == '_')
                throw new ExpressionParserException(ParserPosition, ExpressionParserException.ExceptionCause.MalformedSymbol);

            //return number;
            if (hasDecimal)
                return new Operand(anchor, OperandType.Double, double.Parse(number));
            else
            {
                // This matches csharp. var <someNumber> will be an int or long if it's too big.
                if (int.TryParse(number, out var intNumber))
                    return new Operand(anchor, OperandType.Int, intNumber);
                else
                    return new Operand(anchor, OperandType.Long, long.Parse(number));
            }
        }

        private IOperand ParseStringToken(long anchor)
        {
            Debug.Assert(anchor == ParserPosition);
            string result = string.Empty;
            char delimiter = GetNextChar();
            char ch = PeekNextChar();
            while ((ch != delimiter) && (ch != 0))
            {
                result += GetNextChar();
                ch = PeekNextChar();
            }
            if (ch == 0)
                throw new ExpressionParserException(ParserPosition, ExpressionParserException.ExceptionCause.UnterminatedString);

            GetNextChar(); // Discard the closing delimiter.
            return new Operand(anchor, OperandType.String, result);
        }

        private string ParseSymbolToken()
        {
            string strToken = string.Empty;
            char ch = PeekNextChar();
            // DOTTY: Allow dots in symbols.
            while (char.IsLetterOrDigit(ch) || ch == '_' || ch == '.')
            {
                strToken += GetNextChar();
                ch = PeekNextChar();
            }
            return strToken;
        }

        private bool AnyOperatorStartsWith(string nextToken)
        {
            return _operators.Keys.Any(name => name.StartsWith(nextToken));
        }
    }
}
