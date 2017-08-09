namespace App.Infrastructure.Tracing.Dashboard.Controls
{
    /// <summary>
    /// Implementation to support bootstrap-table.
    /// </summary>
    public interface IColumnBuilder : ITableBuilder, IColumnApply<IColumnBuilder>
    {
        /// <summary>
        /// Apply an option to a column.
        /// </summary>
        /// <param name="name">Name of the property to apply</param>
        /// <param name="value">Property value</param>
        IColumnBuilder ApplyToColumn(string name, object value);
    }
}