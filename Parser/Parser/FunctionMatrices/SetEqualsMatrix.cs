using FunctionZero.ExpressionParserZero.FunctionMatrices;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Variables;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionZero.ExpressionParserZero.Parser.FunctionMatrices
{
    public static class SetEqualsMatrix
    {
        /*
		Long,
		NullableLong,
        Double,
		NullableDouble,
		String,
		Variable,
        Bool,
        NullableBool,
        VSet,
        Object, 
        Null,
		*/

        internal static DoubleOperandFunctionMatrix Create()
        {
            var matrix = new DoubleOperandFunctionMatrix();

            //new Operand(OperandType.XXX, (XXX)rightOperand.GetValue()));

            double a;
            long b = 5;
            a = b;

            matrix.RegisterDelegate(OperandType.Long, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Long, (long)rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Long, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.Long, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Long, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Long, (long) leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Long, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.Long, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Long, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.Long, (long) leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Long, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.Long, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Long, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.Long, (long) leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Long, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.Long, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Long, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.Long, (long) leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Long, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.Long, (long) leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Long, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.Long, (long) leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));

            matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.NullableLong, (long?)(long)rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.NullableLong, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.NullableLong, (long?)rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableLong, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableLong, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableLong, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableLong, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.NullableLong, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.NullableLong, rightOperand.GetValue()));

            matrix.RegisterDelegate(OperandType.Double, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Double, (double)(long)rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Double, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Double, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Double, (double)rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Double, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Double, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Double, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Double, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Double, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Double, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Double, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Double, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));

            matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (double?)(long)rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (double?)(long?)rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (double?)(double)rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, (double?)rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.NullableDouble, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.NullableDouble, rightOperand.GetValue()));

            //matrix.RegisterDelegate(OperandType.String, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.String, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.String, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.String, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.String, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.String, (string)rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.String, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.String, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.String, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.String, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.String, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.String, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.String, rightOperand.GetValue()));


// HERE


            //matrix.RegisterDelegate(OperandType.Variable, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Variable, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Variable, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Variable, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Variable, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Variable, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Variable, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Variable, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Variable, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Variable, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Variable, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));

            //matrix.RegisterDelegate(OperandType.Bool, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Bool, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Bool, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Bool, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Bool, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Bool, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Bool, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.Bool, (bool)rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Bool, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Bool, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Bool, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Bool, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));

            //matrix.RegisterDelegate(OperandType.NullableBool, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableBool, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableBool, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableBool, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableBool, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableBool, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.NullableBool, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.NullableBool, (bool)rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.NullableBool, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.NullableBool, (bool?)rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableBool, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableBool, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.NullableBool, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));

            //matrix.RegisterDelegate(OperandType.VSet, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.VSet, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.VSet, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.VSet, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.VSet, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.VSet, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.VSet, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.VSet, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.VSet, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.VSet, (VariableSet)rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.VSet, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.VSet, (VariableSet)rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.VSet, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));

            matrix.RegisterDelegate(OperandType.Object, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Object, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Object, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Object, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Object, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Object, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Object, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Object, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Object, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Object, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));
            matrix.RegisterDelegate(OperandType.Object, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.Object, rightOperand.GetValue()));

            //matrix.RegisterDelegate(OperandType.Null, OperandType.Long, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Null, OperandType.NullableLong, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Null, OperandType.Double, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Null, OperandType.NullableDouble, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Null, OperandType.String, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Null, OperandType.Variable, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Null, OperandType.Bool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Null, OperandType.NullableBool, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Null, OperandType.VSet, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Null, OperandType.Object, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));
            //matrix.RegisterDelegate(OperandType.Null, OperandType.Null, (leftOperand, rightOperand) => new Operand(OperandType.REPLACE_RESULT_TYPE, () leftOperand.GetValue() REPLACE_OPERATOR() rightOperand.GetValue()));


            return matrix;

        }
    }
}