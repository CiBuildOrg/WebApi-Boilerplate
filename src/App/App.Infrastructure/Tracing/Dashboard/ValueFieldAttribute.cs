using System;

namespace App.Infrastructure.Tracing.Dashboard
{
    /// <exclude/>
    [AttributeUsage(AttributeTargets.Field)]
    public class ValueFieldAttribute : NameFieldAttribute
    {
        /// <exclude/>
        public string Value { get; set; }
    }
}