using FunctionZero.ExpressionParserZero.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Operands;

namespace FunctionZero.ExpressionParserZero.Parser.FunctionVectors
{
    public static class UnaryNotVector
    {
        public static SingleOperandFunctionVector Create()
        {
            var vector = new SingleOperandFunctionVector();

            vector.RegisterDelegate(OperandType.Bool, operand => new Operand(OperandType.Bool, ! (bool)operand.GetValue()));
            vector.RegisterDelegate(OperandType.NullableBool, operand => new Operand(OperandType.NullableBool, ! (bool?)operand.GetValue()));

            return vector;
        }
    }
}
