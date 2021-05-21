using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.Operands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace FunctionZero.ExpressionParserZero.Binding
{
    internal class VariableEvaluator : IBackingStore
    {
        //private object[] _values;
        private readonly IList<string> _keys;
        private readonly IList<PathBind> _bindingCollection;

        public VariableEvaluator(IList<string> keys, IList<PathBind> bindingCollection)
        {
            _keys = keys;
            _bindingCollection = bindingCollection;
        }

        public (OperandType type, object value) GetValue(string qualifiedName)
        {
            int index = _keys.IndexOf(qualifiedName);
            object value = _bindingCollection[index].Value;
            Type theType = _bindingCollection[index].PropertyType;

            if (theType == null)
                throw new ExpressionEvaluatorException(-1, ExpressionEvaluatorException.ExceptionCause.UndefinedVariable, $"'{qualifiedName}'");

            if(BackingStoreHelpers.OperandTypeLookup.TryGetValue(theType, out var theOperandType))
                return (theOperandType, value);

            if (value == null)
                return (OperandType.Null, null);

            return (OperandType.Object, value);
        }

        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types
        bool IsOfNullableType<T>(T o)
        {
            var type = typeof(T);
            return Nullable.GetUnderlyingType(type) != null;
        }

        private static char[] _dot = new[] { '.' };

        public void SetValue(string qualifiedName, object value)
        {
            throw new NotImplementedException();
            //var host = _bindingExtension.Source ?? _bindingExtension.BindableTarget.BindingContext;
            //if (host != null)
            //{
            //    var bits = qualifiedName.Split(_dot);

            //    for (int c = 0; c < bits.Length - 1; c++)
            //    {
            //        PropertyInfo prop = host.GetType().GetProperty(bits[c], BindingFlags.Public | BindingFlags.Instance);
            //        if (null != prop && prop.CanRead)
            //        {
            //            host = prop.GetValue(host);
            //        }
            //        else
            //            return;
            //    }
            //    var variableName = bits[bits.Length - 1];

            //    PropertyInfo prop2 = host.GetType().GetProperty(variableName, BindingFlags.Public | BindingFlags.Instance);
            //    if (null != prop2 && prop2.CanWrite)
            //    {
            //        prop2.SetValue(host, value, null);
            //    }
            //}
        }
    }
}