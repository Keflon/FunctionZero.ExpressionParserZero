using System.Collections.Generic;

namespace FunctionZero.ExpressionParserZero.Variables
{
	public interface IVariableStore
	{
		IReadOnlyDictionary<string, Variable> AllVariables { get; }
		Variable GetVariable(string qualifiedVariableName);

		void RegisterVariable(Variable variable);
		void UnregisterVariable(Variable variable);
	}
}