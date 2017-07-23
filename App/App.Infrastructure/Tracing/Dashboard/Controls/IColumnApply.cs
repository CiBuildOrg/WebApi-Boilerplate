namespace App.Infrastructure.Tracing.Dashboard.Controls
{
    /// <exclude/>
    public interface IColumnApply<out TResult>
    {
        /// <summary>
        /// Apply an option to a column
        /// </summary>
        /// <param name="option">Column option to apply</param>
        /// <param name="value">Object value</param>
        TResult Apply(ColumnOption option, object[] value);

        /// <summary>
        /// Apply an option to a column.
        /// </summary>
        /// <param name="option">Column option to apply.</param>
        /// <param name="value">Property value.</param>
        TResult Apply(ColumnOption option, object value);

        /// <summary>
        /// Apply a boolean true value to one or more column options.
        /// </summary>
        /// <param name="options">Column option(s) to apply.</param>
        TResult Apply(params ColumnOption[] options);

        /// <summary>
        /// Apply a boolean false value to one or more column options.
        /// </summary>
        /// <param name="options">Column option(s) to apply.</param>
        TResult Cease(params ColumnOption[] options);
    }
}