using FunctionZero.ExpressionParserZero.Operands;
using System.Collections.Generic;

namespace FunctionZero.ExpressionParserZero.Variables
{
	public interface IVariableStore
	{
		IReadOnlyDictionary<string, Variable> AllVariables { get; }
		Variable GetVariable(string qualifiedVariableName);

		void RegisterVariable(Variable variable);
		void UnregisterVariable(Variable variable);

		void RegisterVariable(OperandType type, string qualifiedVariableName, object initialValue, object state);
		void UnregisterVariable(string qualifiedVariableName);
	}
}