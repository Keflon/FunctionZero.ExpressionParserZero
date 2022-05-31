//using FunctionZero.ExpressionParserZero.Operands;
//using System;
//using System.Collections.Generic;
//using System.Reflection;

//namespace FunctionZero.ExpressionParserZero.BackingStore
//{
///// <summary>
///// Spoiler: It's not faster than PocoBackingStore! (well, it can be about 0.7% faster in a particular test)
///// </summary>
//    public class FastPocoBackingStore : IBackingStore
//    {
//        private static char[] _dot = new[] { '.' };

//        private readonly Dictionary<Type, Dictionary<string, (MethodInfo getter, MethodInfo setter)>> _pocoLookup;


//        //private readonly (object host, FastPocoBackingStore backingStore) _hostInfo;

//        public FastPocoBackingStore(object host)
//        {
//            Host = host;

//            _pocoLookup = new Dictionary<Type, Dictionary<string, (MethodInfo getter, MethodInfo setter)>>();

//        }

//        public object Host { get; }

//        public (OperandType type, object value) GetValue(string qualifiedName)
//        {
//            var hostInfo = GetPropertyInfo(Host, qualifiedName);
//            var value = hostInfo.info.getter.Invoke(hostInfo.host, new object[] { });

//            if (BackingStoreHelpers.OperandTypeLookup.TryGetValue(hostInfo.info.getter.ReturnType, out var theOperandType))
//                return (theOperandType, value);

//            if (value == null)
//                return (OperandType.Null, null);

//            return (OperandType.Object, value);
//        }

//        public void SetValue(string qualifiedName, object value)
//        {
//            var hostInfo = GetPropertyInfo(Host, qualifiedName);

//            //if(hostInfo.info.CanWrite)
//            {
//                hostInfo.info.setter.Invoke(hostInfo.host, new object[] { value });
//            }
//        }

//        //protected (object host, PropertyInfo info) GetPropertyInfo(object host, string qualifiedName)
//        //{
//        //    var bits = qualifiedName.Split(_dot);

//        //    PropertyInfo pi = null;
//        //    object nextHost = host;
//        //    foreach (var bit in bits)
//        //    {
//        //        host = nextHost;

//        //        // Get info for the property
//        //        pi = host.GetType().GetProperty(bit, BindingFlags.Public | BindingFlags.Instance);
//        //        if (pi == null || pi.CanRead == false)
//        //            return (null, null);

//        //        nextHost = pi.GetValue(host);
//        //    }
//        //    return (host, pi);
//        //}

//        protected (object host, (MethodInfo getter, MethodInfo setter) info) GetPropertyInfo(object host, string qualifiedName)
//        {
//            var bits = qualifiedName.Split(_dot);

//            (MethodInfo getter, MethodInfo setter) propertyInfo = (null, null);
//            object nextHost = host;
//            foreach (var bit in bits)
//            {
//                host = nextHost;

//                // Get info for the property


//                // is 'host' type in the cache?
//                Dictionary<string, (MethodInfo propertyGetter, MethodInfo propertySetter)> propertyLookup;

//                if (_pocoLookup.TryGetValue(host.GetType(), out propertyLookup) == false)
//                {
//                    // If not, add it in ...
//                    propertyLookup = new Dictionary<string, (MethodInfo propertyGetter, MethodInfo propertySetter)>();
//                    _pocoLookup.Add(host.GetType(), propertyLookup);
//                }

//                // Does the property lookup contain our property?
//                if (propertyLookup.TryGetValue(bit, out propertyInfo) == false)
//                {
//                    // If not, add it in ...
//                    MethodInfo propertyInfoGet = host.GetType().GetProperty(bit, BindingFlags.Public | BindingFlags.Instance).GetGetMethod();
//                    MethodInfo propertyInfoSet = host.GetType().GetProperty(bit, BindingFlags.Public | BindingFlags.Instance).GetSetMethod();

//                    propertyInfo = (propertyInfoGet, propertyInfoSet);
//                    propertyLookup.Add(bit, propertyInfo);
//                }


//                //pi = host.GetType().GetProperty(bit, BindingFlags.Public | BindingFlags.Instance);





//                if (propertyInfo.getter == null)
//                    return (null, propertyInfo);

//                nextHost = propertyInfo.getter.Invoke(host, new object[] { });
//            }
//            return (host, propertyInfo);
//        }

//    }
//}
