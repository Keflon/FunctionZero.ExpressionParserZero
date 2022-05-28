using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Parser.FunctionMatrices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Parser
{
    internal static class DefaultRegistrations
    {
        public static void RegisterDefaultAliases(ExpressionParser parser)
        {
            parser.RegisterOperator("AND", 4, LogicalAndMatrix.Create(), ShortCircuitMode.LogicalAnd);
            parser.RegisterOperator("OR", 4, LogicalOrMatrix.Create(), ShortCircuitMode.LogicalOr);
            parser.RegisterOperator("LT", 4, LessThanMatrix.Create());
            parser.RegisterOperator("LTE", 4, LessThanOrEqualMatrix.Create());
            parser.RegisterOperator("GT", 4, GreaterThanMatrix.Create());
            parser.RegisterOperator("GTE", 4, GreaterThanOrEqualMatrix.Create());
            parser.RegisterOperator("BAND", 4, BitwiseAndMatrix.Create());
            parser.RegisterOperator("BOR", 4, BitwiseOrMatrix.Create());
        }
        public static void RegisterDefaultFunctions(ExpressionParser parser)
        {
            parser.RegisterFunction("Acos", DoAcos, 1, 1);
            parser.RegisterFunction("ASin", DoAsin, 1, 1);
            parser.RegisterFunction("Atan", DoAtan, 1, 1);
            parser.RegisterFunction("Cos", DoCos, 1, 1);
            parser.RegisterFunction("Cosh", DoCosh, 1, 1);
            parser.RegisterFunction("Exp", DoExp, 1, 1);
            //parser.RegisterFunction("Log", DoLog, 1, 2);
            parser.RegisterFunction("Log10", DoLog10, 1, 1);
            parser.RegisterFunction("Pow", DoPow, 2, 2);
            parser.RegisterFunction("Sin", DoSin, 1, 1);
            parser.RegisterFunction("Sinh", DoSinh, 1, 1);
            parser.RegisterFunction("Sqrt", DoSqrt, 1, 1);
            parser.RegisterFunction("Tan", DoTan, 1, 1);
            parser.RegisterFunction("Tanh", DoTanh, 1, 1);

            parser.RegisterFunction("Lerp", DoLerp, 3, 3);
        }

        private static void DoAcos(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            //double val = (double)first.GetValue();
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Acos(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        private static void DoAsin(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Asin(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        private static void DoAtan(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Atan(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        private static void DoCos(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Cos(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        private static void DoCosh(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Cosh(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        private static void DoExp(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Exp(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        //private static void DoLog(Stack<IOperand> stack, IBackingStore store, long paramCount)
        //{
        //    IOperand first = OperatorActions.PopAndResolve(stack, store);
        //    double val = (double)first.GetValue();
        //    var result = Math.Exp(val);
        //    stack.Push(new Operand(-1, OperandType.Double, result));
        //}
        private static void DoLog10(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Log10(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        private static void DoPow(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand second = OperatorActions.PopAndResolve(stack, store);
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = (double)first.GetValue();
            var result = Math.Pow(Convert.ToDouble(first.GetValue()), Convert.ToDouble(second.GetValue()));
            stack.Push(new Operand(-1, OperandType.Double, result));
        }

        private static void DoSin(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Sin(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        private static void DoSinh(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Sinh(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        private static void DoSqrt(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Sqrt(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        private static void DoTan(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Tan(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }
        private static void DoTanh(Stack<IOperand> stack, IBackingStore store, long paramCount)
        {
            IOperand first = OperatorActions.PopAndResolve(stack, store);
            double val = Convert.ToDouble(first.GetValue());
            var result = Math.Tanh(val);
            stack.Push(new Operand(-1, OperandType.Double, result));
        }

        private static void DoLerp(Stack<IOperand> operandStack, IBackingStore backingStore, long paramCount)
        {
            // Pop the correct number of parameters from the operands stack, ** in reverse order **
            // If an operand is a variable, it is resolved from the backing store provided
            IOperand third = OperatorActions.PopAndResolve(operandStack, backingStore);
            IOperand second = OperatorActions.PopAndResolve(operandStack, backingStore);
            IOperand first = OperatorActions.PopAndResolve(operandStack, backingStore);

            double a = Convert.ToDouble(first.GetValue());
            double b = Convert.ToDouble(second.GetValue());
            double t = Convert.ToDouble(third.GetValue());

            // The result is of type double
            double result = a + t * (b - a);

            // Push the result back onto the operand stack
            operandStack.Push(new Operand(-1, OperandType.Double, result));
        }
    }
}
