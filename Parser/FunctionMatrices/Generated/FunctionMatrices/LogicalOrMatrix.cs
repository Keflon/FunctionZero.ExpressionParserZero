using FunctionZero.ExpressionParserZero.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Operands;

namespace FunctionZero.ExpressionParserZero.Parser.FunctionMatrices
{
    public static class LogicalOrMatrix
 {
     public static DoubleOperandFunctionMatrix Create()
     {
         var matrix = new DoubleOperandFunctionMatrix();

            matrix.RegisterDelegate(OperandType.Bool, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool)leftOperand.GetValue() || (bool)rightOperand.GetValue()));

         return matrix;
     }
 }
}
