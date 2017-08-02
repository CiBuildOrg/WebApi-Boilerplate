using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Utils
{
    public static class TypeHelper
    {
        public static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(Type openGenericType, Assembly assembly)
        {
            return from x in assembly.GetTypes()
                from z in x.GetInterfaces()
                let y = x.BaseType
                where
                (y != null && y.IsGenericType &&
                 openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                (z.IsGenericType &&
                 openGenericType.IsAssignableFrom(z.GetGenericTypeDefinition()))
                select x;
        }

        public static IEnumerable<Type> GetBaseClassesAndInterfaces(Type type)
        {
            return type.BaseType == typeof(object)
                ? type.GetInterfaces()
                : Enumerable
                    .Repeat(type.BaseType, 1)
                    .Concat(type.GetInterfaces())
                    .Concat(GetBaseClassesAndInterfaces(type.BaseType))
                    .Distinct();
        }
    }
}
