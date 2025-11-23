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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Tokens;

namespace FunctionZero.ExpressionParserZero
{
    public static class OperatorActions
    {
        private const long Bogey = -1;

        /// <summary>
        /// Pops an operand from the stack. If it's a variable it returns an operand holding the variable value, otherwise it returns the operand.
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IOperand PopAndResolve(Stack<IOperand> stack, IBackingStore backingStore)
        {
            IOperand operand = stack.Pop();

            if (operand.Type == OperandType.Variable)
            {
                //var variable = ResolveVariable(variables, retVal);

                try
                {
                    var valueAndType = backingStore.GetValue((string)operand.GetValue());
                    operand = new Operand(operand.ParserPosition, valueAndType.type, valueAndType.value);
                }
                catch// (KeyNotFoundException)
                {
                    throw new ExpressionEvaluatorException(operand.ParserPosition, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable, $"'{operand.GetValue().ToString()}'");
                }
            }
            return operand;
        }

        /// <summary>
        /// Peeks an operand from the stack. If it's a variable it returns an operand holding the variable value, otherwise it returns the operand.
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IOperand PeekAndResolve(Stack<IOperand> stack, IBackingStore backingStore)
        {
            IOperand operand = stack.Peek();

            if (operand.Type == OperandType.Variable)
            {
                //var variable = ResolveVariable(variables, retVal);

                try
                {
                    var valueAndType = backingStore.GetValue((string)operand.GetValue());
                    operand = new Operand(operand.ParserPosition, valueAndType.type, valueAndType.value);
                }
                catch// (KeyNotFoundException)
                {
                    throw new ExpressionEvaluatorException(operand.ParserPosition, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable, $"'{operand.GetValue().ToString()}'");
                }
            }
            return operand;
        }

        //private static Variable ResolveVariable(IVariableStore variables, IOperand variable)
        //{
        //	try
        //	{
        //		// Supports dotted notation
        //		Variable v = variables.GetVariable((string)variable.GetValue());
        //		// TODO: What if 'v.Value' is a Variable'? 
        //		return v;
        //	}
        //	catch (KeyNotFoundException)
        //	{
        //		throw new ExpressionEvaluatorException(variable.ParserPosition, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable, variable.GetValue().ToString());
        //	}
        //}

        [Obsolete("Used by a test-case call to RegisterFunction.")]
        internal static void DoMultiply(Stack<IOperand> stack, IBackingStore backingStore, long parserPosition)
        {
            IOperand second = PopAndResolve(stack, backingStore);
            IOperand first = PopAndResolve(stack, backingStore);

            // Don't do this in production code.
            var matrix = Parser.FunctionMatrices.MultiplyMatrix.Create();

            stack.Push(matrix.PerformDelegate(first, second));

        }

        internal static Tuple<OperandType> DoUnaryOperation(SingleOperandFunctionVector vector, Stack<IOperand> stack, IBackingStore backingStore)
        {
            IOperand first = PopAndResolve(stack, backingStore);

            IOperand result = vector.PerformDelegate(first);

            if (result != null)
            {
                stack.Push(result);
                return null;
            }
            else
            {
                // Signal an error ...
                return new Tuple<OperandType>(first.Type);
            }
        }

        internal static Tuple<OperandType, OperandType> DoOperation(DoubleOperandFunctionMatrix matrix, Stack<IOperand> stack, IBackingStore backingStore)
        {
            IOperand second = PopAndResolve(stack, backingStore);
            IOperand first = PopAndResolve(stack, backingStore);

            IOperand result = matrix.PerformDelegate(first, second);

            if (result != null)
            {
                stack.Push(result);
                return null;
            }
            else
            {
                // Signal an error ...
                return new Tuple<OperandType, OperandType>(first.Type, second.Type);
            }
        }

        #region =

        /// <summary>
        /// This ought to be implemented with a FunctionMartix but it would be huge.
        /// This way is easier!
        /// </summary>
        internal static Tuple<OperandType, OperandType> DoSetEquals(DoubleOperandFunctionMatrix matrix, Stack<IOperand> stack, IBackingStore backingStore, long parserPosition)
        {
            IOperand second = PopAndResolve(stack, backingStore);
            IOperand first = stack.Pop();       // Not PopAndResolve. LHS must be a variable.

            if (first.Type != OperandType.Variable)
            {
                throw new ExpressionEvaluatorException(parserPosition, ExpressionEvaluatorException.ExceptionCause.BadOperand, "LHS of '=' is a '" + first.Type + "' when it must be a variable");
            }
            else
            {
                string varName = (string)first.GetValue();

                (OperandType type, object value) valueAndType;
                try
                {
                    // We need the type, don't need the value because it's going to be overwritten.
                    valueAndType = backingStore.GetValue(varName);
                }
                catch(Exception ex)
                {
                    throw new ExpressionEvaluatorException(-1, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable, $"'{varName}'");
                }

                // type is needed so we can pick out the correct martrix operation. Value is irrelevant as it is overwritten.
                var firstOperand = new Operand(first.ParserPosition, valueAndType.type, valueAndType.value);

                IOperand result = matrix.PerformDelegate(firstOperand, second);

                if (result != null)
                {
                    backingStore.SetValue(varName, result.GetValue());
                    stack.Push(result);
                    return null;
                }
                else
                {
                    // Signal an error ...
                    return new Tuple<OperandType, OperandType>(first.Type, second.Type);
                }
            }
        }

        internal static Tuple<OperandType> DoUnaryCastOperation(DoubleOperandFunctionMatrix matrix, Stack<IOperand> operandStack, IBackingStore backingStore, Operand castTo)
        {
            IOperand operand = PopAndResolve(operandStack, backingStore);

            IOperand result = matrix.PerformDelegate(operand, castTo);

            if (result != null)
            {
                operandStack.Push(result);
                return null;
            }
            else
            {
                // Signal an error ...
                return new Tuple<OperandType>(operand.Type);
            }
        }

        #endregion
    }
}
