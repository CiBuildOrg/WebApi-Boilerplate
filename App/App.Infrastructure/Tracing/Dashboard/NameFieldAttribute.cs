using System;

namespace App.Infrastructure.Tracing.Dashboard
{
    /// <exclude/>
    [AttributeUsage(AttributeTargets.Field)]
    internal class NameFieldAttribute : Attribute
    {
        /// <exclude/>
        public string Name { get; set; }
    }
}