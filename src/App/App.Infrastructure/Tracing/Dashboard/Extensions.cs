using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace App.Infrastructure.Tracing.Dashboard
{
    /// <exclude/>
    public static class Extensions
    {
        /// <exclude/>
        public static string ToStringLower(this object value) => value?.ToString().ToLower();

        /// <exclude/>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var t in enumerable)
            {
                action(t);
            }
        }

        /// <exclude/>
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        /// <exclude/>
        public static IDictionary<string, object> HtmlAttributesToDictionary(this object htmlAttributes)
        {
            return htmlAttributes as IDictionary<string, object> ?? HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        }

        /// <exclude/>
        public static bool IsAnonymousType(this Type type)
        {
            var hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any();
            var nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            return hasCompilerGeneratedAttribute && nameContainsAnonymousType;
        }

        /// <exclude/>
        public static IHtmlString ToSerializedString(this object data)
        {
            var serializer = new JavaScriptSerializer();
            return new HtmlString((serializer.Serialize(data)));
        }

        /// <exclude/>
        public static string GetName<TModel, TProperty>(this Expression<Func<TModel, TProperty>> field)
        {
            var member = field.Body as MemberExpression;
            return member?.Member.Name;
        }

        /// <exclude/>
        public static string GetDisplayName<TModel, TProperty>(this Expression<Func<TModel, TProperty>> field)
        {
            var member = field.Body as MemberExpression;
            if (member == null) throw new NullReferenceException("member is not a MemberExpression");
            var display = member.Member.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            return !string.IsNullOrEmpty(display?.GetName())
                ? display.GetName()
                : member.Member.Name.SplitCamelCase();
        }

        /// <exclude/>
        public static IEnumerable<PropertyInfo> GetSortedProperties(this Type type)
        {
            var order = int.MaxValue - type.GetProperties().Count();
            return type.GetProperties()
                .Select(p => new
                {
                    property = p,
                    order = p.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() != null ?
                        (((DisplayAttribute)p.GetCustomAttributes(typeof(DisplayAttribute), true).First()).GetOrder() ?? ++order) : ++order
                })
                .OrderBy(o => o.order)
                .Select(o => o.property);
        }
    }
}