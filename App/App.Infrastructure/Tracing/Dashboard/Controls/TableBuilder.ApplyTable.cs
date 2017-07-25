using System.Linq;

namespace App.Infrastructure.Tracing.Dashboard.Controls
{
    public partial class TableBuilder
    {
        /// <inheritDoc/>
        public ITableBuilder ApplyToTable(string name, object value)
        {
            if (value.GetType().IsAnonymousType())
                _builder.Attributes.Add(name, value.ToSerializedString().ToString());
            else if (value is bool)
                _builder.Attributes.Add(name, value.ToStringLower());
            else
                _builder.Attributes.Add(name, value.ToString());
            return this;
        }

		/// <exclude/>
        public ITableBuilder Apply(TableOption option, object value)
        {
            return ApplyToTable(option.FieldName(), value);
        }

        /// <exclude/>
        public ITableBuilder Apply(TableOption option, object[] value)
        {
            if (option.GetType() == typeof(TableOption) && option == TableOption.Columns)
                return Columns(value.Select(o => o.ToString()).ToArray());
            else
                return ApplyToTable(option.FieldName(), $"[{string.Join(",", value)}]");
        }

        /// <exclude/>
        public ITableBuilder Apply(params TableOption[] options)
        {
            options.ForEach(option =>
            {
                // ReSharper disable once MustUseReturnValue
                ApplyToTable(option.FieldName(), option.FieldValue() ?? true.ToStringLower());
            });
            return this;
        }

        /// <exclude/>
        public ITableBuilder Cease(params TableOption[] options)
        {
            options.ForEach(option =>
            {
                // ReSharper disable once MustUseReturnValue
                ApplyToTable(option.FieldName(), option.FieldValue() ?? false.ToStringLower());
            });
            return this;
        }
    }
}
