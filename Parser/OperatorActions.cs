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
                catch (KeyNotFoundException)
                {
                    throw new ExpressionEvaluatorException(operand.ParserPosition, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable, operand.GetValue().ToString());
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

            if (first.IsNumber && second.IsNumber)
            {
                if (first.Type == OperandType.Double)
                    if (second.Type == OperandType.Double)
                        stack.Push(new Operand(Bogey, OperandType.Double, (double)first.GetValue() * (double)second.GetValue()));        // double double
                    else
                        stack.Push(new Operand(Bogey, OperandType.Double, (double)first.GetValue() * (long)second.GetValue()));          // double long
                else if (second.Type == OperandType.Double)
                    stack.Push(new Operand(Bogey, OperandType.Double, (long)first.GetValue() * (double)second.GetValue()));              // long double
                else
                    stack.Push(new Operand(Bogey, OperandType.Long, (long)first.GetValue() * (long)second.GetValue()));                  // long long
            }
            else
            {
                throw new ExpressionEvaluatorException(parserPosition, ExpressionEvaluatorException.ExceptionCause.BadOperand, "Operator '*' cannot be applied to operands of type " + first.TokenType + " and " + second.TokenType);

            }
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

                //var targetVariable = ResolveVariable(variables, first);
                var valueAndType = backingStore.GetValue(varName);

                var firstOperand = new Operand(first.ParserPosition, valueAndType.type, valueAndType.value);

                IOperand result = matrix.PerformDelegate(firstOperand, second);

                if (result != null)
                {
                    //Debug.Assert(targetVariable.VariableType == result.Type);
                    //targetVariable.Value = result.GetValue();
                    backingStore.SetValue(varName, result.GetValue());
                    stack.Push(result);
                    return null;
                }
                else
                {
                    // Signal an error ...
                    return new Tuple<OperandType, OperandType>(first.Type, second.Type);
                }
#if false
				// Supports dotted notation.
				Variable v = variables.GetVariable(varName);
				v.Value = second.GetValue();
				// Put the result on the stack. Is that right? Should the result be the variable or it's value?
				stack.Push(new Operand(Bogey, v.VariableType, v.Value));
#endif
            }
        }

        internal static Tuple<OperandType> DoUnaryCastOperation(SingleOperandFunctionVector vector, Stack<IOperand> operandStack, IBackingStore backingStore, OperandType castTo)
        {
            IOperand first = PopAndResolve(operandStack, backingStore);

            IOperand result = vector.PerformCastDelegate(castTo, first);

            if (result != null)
            {
                operandStack.Push(result);
                return null;
            }
            else
            {
                // Signal an error ...
                return new Tuple<OperandType>(first.Type);
            }
        }

        #endregion
    }
}
