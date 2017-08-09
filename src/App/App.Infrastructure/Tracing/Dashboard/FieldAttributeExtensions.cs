using System;
using System.Reflection;

namespace App.Infrastructure.Tracing.Dashboard
{
    /// <exclude/>
    public static class FieldAttributeExtensions
    {
        /// <exclude/>
        public static string FieldName(this Enum value)
        {
            var attr = value.GetType().GetField(value.ToString()).GetCustomAttribute<NameFieldAttribute>();
            return attr?.Name;
        }

        /// <exclude/>
        public static string FieldValue(this Enum value)
        {
            var attr = value.GetType().GetField(value.ToString()).GetCustomAttribute<ValueFieldAttribute>();
            return attr?.Value;
        }
    }
}