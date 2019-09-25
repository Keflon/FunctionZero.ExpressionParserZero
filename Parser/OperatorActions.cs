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
using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Tokens;
using FunctionZero.ExpressionParserZero.Variables;

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
		public static IOperand PopAndResolve(Stack<IOperand> stack, IVariableSet variables)
		{
			IOperand retVal = stack.Pop();
			
			if(retVal.Type == OperandType.Variable)
			{
				Variable v = variables.GetVariable((string)retVal.GetValue());
				if(v == null)
					throw new ExpressionEvaluatorException(retVal.ParserPosition, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable, retVal.GetValue().ToString());

				retVal = new Operand(retVal.ParserPosition, v.VariableType, v.Value);
			}
			return retVal;
		}

		[Obsolete("Used by a test-case call to RegisterFunction.")]
		internal static void DoMultiply(Stack<IOperand> stack, IVariableSet variables, long parserPosition)
		{
			IOperand second = PopAndResolve(stack, variables);
			IOperand first = PopAndResolve(stack, variables);

			if(first.IsNumber && second.IsNumber)
			{
				if(first.Type == OperandType.Double)
					if(second.Type == OperandType.Double)
						stack.Push(new Operand(Bogey, OperandType.Double, (double)first.GetValue() * (double)second.GetValue()));        // double double
					else
						stack.Push(new Operand(Bogey, OperandType.Double, (double)first.GetValue() * (long)second.GetValue()));          // double long
				else if(second.Type == OperandType.Double)
					stack.Push(new Operand(Bogey, OperandType.Double, (long)first.GetValue() * (double)second.GetValue()));              // long double
				else
					stack.Push(new Operand(Bogey, OperandType.Long, (long)first.GetValue() * (long)second.GetValue()));                  // long long
			}
			else
			{
				throw new ExpressionEvaluatorException(parserPosition, ExpressionEvaluatorException.ExceptionCause.BadOperand, "Operator '*' cannot be applied to operands of type " + first.TokenType + " and " + second.TokenType);

			}
		}

	    internal static Tuple<OperandType> DoUnaryOperation(SingleOperandFunctionVector matrix, Stack<IOperand> stack, IVariableSet variables)
	    {
	        IOperand first = PopAndResolve(stack, variables);

	        IOperand result = matrix.PerformDelegate(first);

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

	    internal static Tuple<OperandType, OperandType> DoOperation(DoubleOperandFunctionMatrix matrix, Stack<IOperand> stack, IVariableSet variables)
	    {
	        IOperand second = PopAndResolve(stack, variables);
	        IOperand first = PopAndResolve(stack, variables);

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

		internal static void DoSetEquals(Stack<IOperand> stack, IVariableSet variables, long parserPosition)
		{
			IOperand second = PopAndResolve(stack, variables);
			IOperand first = stack.Pop();       // Not PopAndResolve. LHS must be a variable.

			if(first.Type != OperandType.Variable)
			{
				throw new ExpressionEvaluatorException(parserPosition, ExpressionEvaluatorException.ExceptionCause.BadOperand, "LHS of '=' is a '" + first.Type + "' when it must be a variable");
			}
			else
			{
				string varName = (string)first.GetValue();
                // TODO: Support dotted notation.
				Variable v = variables.GetVariable(varName);
				// TODO: Use the setter on v. Can't yet because SetVariable raises an event.
				variables.SetVariable(v, second.GetValue());
				// Put the result on the stack. Is that right?
				stack.Push(new Operand(Bogey, v.VariableType, v.Value));
			}
		}

		#endregion
	}
}
