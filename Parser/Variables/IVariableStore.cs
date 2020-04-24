using FunctionZero.ExpressionParserZero.Operands;
using System.Collections.Generic;

namespace FunctionZero.ExpressionParserZero.Variables
{
	public interface IVariableStore : IEnumerable<Variable>
	{
		//IReadOnlyDictionary<string, Variable> AllVariables { get; }
		Variable GetVariable(string qualifiedVariableName);

		Variable this[string unqualifiedVariableName] { get; }

		Variable RegisterVariable(Variable variable);
		Variable UnregisterVariable(Variable variable);

		Variable RegisterVariable(OperandType type, string qualifiedVariableName, object initialValue, object state);
		Variable UnregisterVariable(string qualifiedVariableName);
	}
}