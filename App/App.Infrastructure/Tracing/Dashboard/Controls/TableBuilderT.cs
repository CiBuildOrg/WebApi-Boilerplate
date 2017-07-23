using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace App.Infrastructure.Tracing.Dashboard.Controls
{
    /// <summary>
    /// Build a BootstrapTable control.
    /// </summary>
    internal class TableBuilderT<TModel> : TableBuilder, IColumnBuilderT<TModel>
    {
        /// <exclude/>
        public TableBuilderT(string id = null, string url = null, TablePaginationOption sidePagination = TablePaginationOption.None, 
            object htmlAttributes = null)
            : base(id, url, sidePagination, htmlAttributes)
        {
            typeof(TModel).GetSortedProperties().ToDictionary(x => x.Name, x => x).ForEach(pair =>
            {
                var display = pair.Value.GetCustomAttribute<DisplayAttribute>();
                var hidden = pair.Value.GetCustomAttribute<HiddenInputAttribute>();

                if (hidden != null && hidden.DisplayValue != true) return;
                if (display?.GetAutoGenerateField() != null && !display.AutoGenerateField) return;

                Column(!string.IsNullOrEmpty(display?.GetName()) ? display.GetName() : pair.Key.SplitCamelCase(), pair.Key);
            });
        }

        #region ITableApply
        /// <inheritDoc/>
        public new ITableBuilderT<TModel> ApplyToTable(string name, object value)
        {
            base.ApplyToTable(name, value);
            return this;
        }

        /// <inheritDoc/>
        public new ITableBuilderT<TModel> ApplyToColumns(ColumnOption option)
        {
            base.ApplyToColumns(option);
            return this;
        }

        /// <inheritDoc/>
        public new ITableBuilderT<TModel> Apply(params TableOption[] options)
        {
            base.Apply(options);
            return this;
        }

        /// <inheritDoc/>
        public new ITableBuilderT<TModel> Apply(TableOption option, object value)
        {
            base.Apply(option, value);
            return this;
        }

        /// <inheritDoc/>
        public new ITableBuilderT<TModel> Apply(TableOption option, object[] value)
        {
            base.Apply(option, value);
            return this;
        }

        /// <inheritDoc/>
        public new ITableBuilderT<TModel> Cease(params TableOption[] options)
        {
            base.Cease(options);
            return this;
        }

        /// <inheritDoc/>
        public new ITableBuilderT<TModel> Columns(params string[] columns)
        {
            base.Columns(columns);
            return this;
        }
        #endregion //ITableApply

        #region IColumnApply
        /// <inheritDoc/>
        public new IColumnBuilderT<TModel> Apply(ColumnOption option, object[] value)
        {
            base.Apply(option, value);
            return this;
        }

        /// <inheritDoc/>
        IColumnBuilderT<TModel> IColumnApply<IColumnBuilderT<TModel>>.Apply(ColumnOption option, object value)
        {
            base.Apply(option, value);
            return this;
        }

        /// <inheritDoc/>
        IColumnBuilderT<TModel> IColumnApply<IColumnBuilderT<TModel>>.Apply(params ColumnOption[] options)
        {
            base.Apply(options);
            return this;
        }

        /// <inheritDoc/>
        public new IColumnBuilderT<TModel> Cease(params ColumnOption[] options)
        {
            base.Cease(options);
            return this;
        }
        #endregion //IColumnApply

        #region ITableBuilderT
        /// <inheritDoc/>
        public IColumnBuilderT<TModel> Apply<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnOption option, object[] value)
        {
            ApplyToColumnT(expression, option.FieldName(), string.Format("[{0}]", string.Join(",", value)));
            return this;
        }

        /// <inheritDoc/>
        public IColumnBuilderT<TModel> Apply<TProperty>(Expression<Func<TModel, TProperty>> expression, ColumnOption option, object value)
        {
            ApplyToColumnT(expression, option.FieldName(), value);
            return this;
        }

        /// <inheritDoc/>
        public IColumnBuilderT<TModel> Apply<TProperty>(Expression<Func<TModel, TProperty>> expression, params ColumnOption[] options)
        {
            options.ForEach(option =>
            {
                ApplyToColumnT(expression, option.FieldName(), option.FieldValue() ?? true.ToStringLower());
            });
            return this;
        }

        /// <inheritDoc/>
        public IColumnBuilderT<TModel> Cease<TProperty>(Expression<Func<TModel, TProperty>> expression, params ColumnOption[] options)
        {
            options.ForEach(option =>
            {
                ApplyToColumnT(expression, option.FieldName(), option.FieldValue() ?? false.ToStringLower());
            });
            return this;
        }

        /// <inheritDoc/>
        public IColumnBuilderT<TModel> Column<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            SetColumnByTitle(expression.GetDisplayName());
            return this;
        }
        #endregion //ITableBuilderT

        /// <exclude/>
        protected IColumnBuilderT<TModel> ApplyToColumnT<TProperty>(Expression<Func<TModel, TProperty>> expression, string name, object value)
        {
            if (!SetColumnByTitle(expression.GetDisplayName()))
                throw new ArgumentException("Column not found!");
            ApplyToColumn(name, value);
            return this;
        }
    }
}
