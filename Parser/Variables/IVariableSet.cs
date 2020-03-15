﻿using System;
using System.Collections.Generic;

namespace FunctionZero.ExpressionParserZero.Variables
{
	public interface IVariableSet2 : IVariableStore
	{
		//Dictionary<string, Variable> AllVariables { get; }
		//IVariableFactory VariableFactory { get; }
		//bool NotifyChanges { get; set; }
		//event EventHandler<VariableAddedEventArgs> VariableAdded;
		//event EventHandler<VariableRemovedEventArgs> VariableRemoved;

		//void RegisterVariable(Variable variable);
		//void UnregisterVariable(Variable variable);
		//bool UnregisterVariable(string qualifiedVariableName);

		//void RegisterLong(string variableName, long initialValue, object state = null);
		//void RegisterNullableLong(string variableName, long? initialValue, object state = null);
		//void RegisterDouble(string variableName, double initialValue, object state = null);
		//void RegisterNullableDouble(string variableName, double initialValue, object state = null);
		//void RegisterString(string variableName, string initialValue, object state = null);
		//void RegisterBool(string variableName, bool initialValue, object state = null);
		//void RegisterNullableBool(string variableName, bool? initialValue, object state = null);
		//void RegisterVSet(string variableName, IVariableSet initialValue, object state = null);
		//void RegisterObject(string variableName, object initialValue, object state = null);
		//void CreateOrUpdateLong(string variableName, long newValue, object state = null);
		//void CreateOrUpdateNullableLong(string variableName, long? newValue, object state = null);
		//void CreateOrUpdateDouble(string variableName, double newValue, object state = null);
		//void CreateOrUpdateNullableDouble(string variableName, double? newValue, object state = null);
		//void CreateOrUpdateString(string variableName, string newValue, object state = null);
		//void CreateOrUpdateBool(string variableName, bool newValue, object state = null);
		//void CreateOrUpdateNullableBool(string variableName, bool? newValue, object state = null);
		//void CreateOrUpdateVSet(string variableName, IVariableSet newValue, object state = null);
		//void CreateOrUpdateObject(string variableName, object newValue, object state = null);

		//Variable GetVariable(string qualifiedVariableName);
		//void SetVariable(string qualifiedVariableName, object newValue);
		//void SetVariable(Variable variable, object newValue);
	}

	public interface IVariableStore
	{
		IReadOnlyDictionary<string, Variable> AllVariables { get; }

		void RegisterVariable(Variable variable);
		void UnregisterVariable(Variable variable);

		//event EventHandler<VariableAddedEventArgs> VariableAdded;
		//event EventHandler<VariableRemovedEventArgs> VariableRemoved;

		Variable GetVariable(string qualifiedVariableName);
		void SetVariable(string qualifiedVariableName, object newValue);
	}
}