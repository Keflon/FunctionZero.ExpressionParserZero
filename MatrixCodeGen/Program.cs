using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace MatrixCodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() != 2)
                Console.WriteLine("Incorrect parameters");
            else
            {
                Debug.WriteLine(args[0]);
                Debug.WriteLine(args[1]);

                List<string> sourceCode;

                if (args[0].ToLower().EndsWith("matrix"))
                {
                    sourceCode = DoubleOperandFunctionMatrix(args[0], args[1]);
                }
                else
                {
                    sourceCode = SingleOperandFunctionVector(args[0], args[1]);

                }
                StringBuilder sb = new StringBuilder();

                foreach (var item in sourceCode)
                    sb.AppendLine(item);


                Console.WriteLine(sb.ToString());
            }
        }

        private static List<string> SingleOperandFunctionVector(string className, string theOperator)
        {
            var lookup = GetTypeLookup();

            var retval = new List<string>();

            retval.Add("using FunctionZero.ExpressionParserZero.FunctionMatrices;");
            retval.Add("using FunctionZero.ExpressionParserZero.Operands;");
            retval.Add("");
            retval.Add("namespace FunctionZero.ExpressionParserZero.Parser.FunctionVectors");
            retval.Add("{");
            retval.Add($"    public static class {className}");
            retval.Add(" {");
            retval.Add("     public static SingleOperandFunctionVector Create()");
            retval.Add("     {");
            retval.Add("         var vector = new SingleOperandFunctionVector();");
            retval.Add("");
            retval.Add("");
            retval.Add("");

            foreach (var theType in lookup)
            {
                string resultType = GetVectorResultType(theType.Value, theOperator);
                 if (resultType != null)
                {
                    string enumType = lookup.FirstOrDefault(pair => pair.Value == resultType).Key;

                    string line = $"      vector.RegisterDelegate(OperandType.{theType.Key}, operand => new Operand(OperandType.{enumType}, {theOperator} ({theType.Value})operand.GetValue()));";

                    retval.Add(line);
                }
            }

            retval.Add("");
            retval.Add("         return vector;");
            retval.Add("     }");
            retval.Add(" }");
            retval.Add("}");

            return retval;
        }


        private static List<string> DoubleOperandFunctionMatrix(string className, string theOperator)
        {
            var lookup = GetTypeLookup();

            var retval = new List<string>();

            retval.Add("using FunctionZero.ExpressionParserZero.FunctionMatrices;");
            retval.Add("using FunctionZero.ExpressionParserZero.Operands;");
            retval.Add("");
            retval.Add("namespace FunctionZero.ExpressionParserZero.Parser.FunctionMatrices");
            retval.Add("{");
            retval.Add($"    public static class {className}");
            retval.Add(" {");
            retval.Add("     public static DoubleOperandFunctionMatrix Create()");
            retval.Add("     {");
            retval.Add("         var matrix = new DoubleOperandFunctionMatrix();");
            retval.Add("");
            retval.Add("");
            retval.Add("");

            foreach (var left in lookup)
            {
                foreach (var right in lookup)
                {
                    string resultType = GetResultType(left.Value, right.Value, theOperator);
                    if (resultType != null)
                    {
                        string enumType = lookup.FirstOrDefault(pair => pair.Value == resultType).Key;

                        string line = $"      matrix.RegisterDelegate(OperandType.{left.Key}, OperandType.{right.Key}, (leftOperand, rightOperand) => new Operand(OperandType.{enumType}, ({left.Value})leftOperand.GetValue() {theOperator} ({right.Value})rightOperand.GetValue()));";

                        retval.Add(line);
                    }
                }
            }

            retval.Add("");
            retval.Add("         return matrix;");
            retval.Add("     }");
            retval.Add(" }");
            retval.Add("}");

            return retval;
        }

        private static string GetResultType(string leftType, string rightType, string theOperator)
        {
            var sb = new StringBuilder();


            sb.AppendLine("using System;");
            sb.AppendLine("");
            sb.AppendLine(" namespace Borg");
            sb.AppendLine(" {");
            sb.AppendLine($"        public static class Test");
            sb.AppendLine("     {");
            sb.AppendLine("static int myInt;");

            sb.AppendLine($"        public static string GetResultType(string leftType, string rightType, string theOperator)");
            sb.AppendLine("     {");
            sb.AppendLine($"            {leftType} left = default;");
            sb.AppendLine($"            {rightType} right = default;");
            sb.AppendLine($"            var result = left {theOperator} right;");
            sb.AppendLine("             return result.GetType().ToString();");
            sb.AppendLine("     }");
            sb.AppendLine(" }");
            sb.AppendLine(" }");
            sb.AppendLine("");

            if (CanCompile(sb.ToString(), out var compilation))
            {
                var source = sb.ToString();

                SemanticModel semanticModel = compilation.GetSemanticModel(compilation.SyntaxTrees[0], true);

                var variableDeclarations = compilation.SyntaxTrees[0].GetRoot()
                    .DescendantNodes()
                    .OfType<LocalDeclarationStatementSyntax>();


                foreach (LocalDeclarationStatementSyntax variableDeclaration in variableDeclarations)
                {
                    SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(variableDeclaration.Declaration.Type);
                    var name = variableDeclaration.Declaration.Variables.First().Identifier.Value;
                    ISymbol typeSymbol = symbolInfo.Symbol; // the type symbol for the variable..

                    if (name.ToString() == "result")
                    {
                        var retval = typeSymbol?.ToDisplayString() ?? null;

                        if (retval != null)
                        {
                            //Compile(source);
                        }

                        return retval;
                    }
                }

                throw new InvalidOperationException();
            }
            return null;
        }


        private static string GetVectorResultType(string theType, string theOperator)
        {
            var sb = new StringBuilder();


            sb.AppendLine("using System;");
            sb.AppendLine("");
            sb.AppendLine(" namespace Borg");
            sb.AppendLine(" {");
            sb.AppendLine($"        public static class Test");
            sb.AppendLine("     {");
            sb.AppendLine("static int myInt;");

            sb.AppendLine($"        public static string GetVectorResultType(string theType, string theOperator)");
            sb.AppendLine("     {");
            sb.AppendLine($"            {theType} theValue = default;");
            sb.AppendLine($"            var result = {theOperator} theValue;");
            sb.AppendLine("             return result.GetType().ToString();");
            sb.AppendLine("     }");
            sb.AppendLine(" }");
            sb.AppendLine(" }");
            sb.AppendLine("");

            if (CanCompile(sb.ToString(), out var compilation))
            {
                var source = sb.ToString();

                SemanticModel semanticModel = compilation.GetSemanticModel(compilation.SyntaxTrees[0], true);

                var variableDeclarations = compilation.SyntaxTrees[0].GetRoot()
                    .DescendantNodes()
                    .OfType<LocalDeclarationStatementSyntax>();


                foreach (LocalDeclarationStatementSyntax variableDeclaration in variableDeclarations)
                {
                    SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(variableDeclaration.Declaration.Type);
                    var name = variableDeclaration.Declaration.Variables.First().Identifier.Value;
                    ISymbol typeSymbol = symbolInfo.Symbol; // the type symbol for the variable..

                    if (name.ToString() == "result")
                    {
                        var retval = typeSymbol?.ToDisplayString() ?? null;

                        if (retval != null)
                        {
                            //Compile(source);
                        }

                        return retval;
                    }
                }

                throw new InvalidOperationException();
            }
            return null;
        }

        private static Dictionary<string, string> GetTypeLookup()
        {
            var retval = new Dictionary<string, string>();


            retval.Add("Sbyte", "sbyte");
            retval.Add("Byte", "byte");
            retval.Add("Short", "short");
            retval.Add("Ushort", "ushort");
            retval.Add("Int", "int");
            retval.Add("Uint", "uint");
            retval.Add("Long", "long");
            retval.Add("Ulong", "ulong");
            retval.Add("Char", "char");
            retval.Add("Float", "float");
            retval.Add("Double", "double");
            retval.Add("Bool", "bool");
            retval.Add("Decimal", "decimal");

            retval.Add("NullableSbyte", "sbyte?");
            retval.Add("NullableByte", "byte?");
            retval.Add("NullableShort", "short?");
            retval.Add("NullableUshort", "ushort?");
            retval.Add("NullableInt", "int?");
            retval.Add("NullableUint", "uint?");
            retval.Add("NullableLong", "long?");
            retval.Add("NullableUlong", "ulong?");
            retval.Add("NullableChar", "char?");
            retval.Add("NullableFloat", "float?");
            retval.Add("NullableDouble", "double?");
            retval.Add("NullableBool", "bool?");
            retval.Add("NullableDecimal", "decimal?");
            retval.Add("String", "string");
            retval.Add("Variable", "IVariable");
            retval.Add("VSet", "IVSet");

            retval.Add("Object", "object");
            retval.Add("Null", "null");

            return retval;
        }

        public static bool CanCompile(string sourceCode, out CSharpCompilation compilation)
        {
            using (var compiledStream = new MemoryStream())
            {
                compilation = GenerateCode(sourceCode);

                Microsoft.CodeAnalysis.Emit.EmitResult result = compilation.Emit(compiledStream);

                if (!result.Success)
                {
                    //Console.WriteLine("You have a problem.");

                    var problems = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var problem in problems)
                    {
                        //Console.Error.WriteLine("{0}: {1}: {2}", problem.Id, problem.GetMessage(), problem.Location.GetLineSpan().StartLinePosition);
                    }
                    return false;
                }
                //Console.WriteLine("Compilation done without any error.");

                compiledStream.Seek(0, SeekOrigin.Begin);

                return true;
            }
        }

        private static CSharpCompilation GenerateCode(string sourceCode)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp7_3);

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);


            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
                //MetadataReference.CreateFromFile(typeof(FunctionZero.ExpressionParserZero.ExpressionEvaluator).Assembly.Location),
            };

            return CSharpCompilation.Create("Hello.dll",
                new[] { parsedSyntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Debug,
                    assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
        }
    }
}
