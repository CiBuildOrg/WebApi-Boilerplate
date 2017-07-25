using System;

namespace App.Infrastructure.Tracing.Dashboard
{
    /// <exclude/>
    [AttributeUsage(AttributeTargets.Field)]
    public class NameFieldAttribute : Attribute
    {
        /// <exclude/>
        public string Name { get; set; }
    }
}