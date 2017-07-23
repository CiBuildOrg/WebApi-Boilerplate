using System;
using System.Reflection;

namespace App.Infrastructure.Tracing.Dashboard
{
    /// <exclude/>
    internal static class FieldAttributeExtensions
    {
        /// <exclude/>
        public static string FieldName(this Enum value)
        {
            var attr = value.GetType().GetField(value.ToString()).GetCustomAttribute<NameFieldAttribute>();
            return attr != null ? attr.Name : null;
        }

        /// <exclude/>
        public static string FieldValue(this Enum value)
        {
            var attr = value.GetType().GetField(value.ToString()).GetCustomAttribute<ValueFieldAttribute>();
            return attr != null ? attr.Value : null;
        }
    }
}