using System.Web;

namespace App.Infrastructure.Tracing.Dashboard.Controls
{
    /// <summary>
    /// Implementation to support bootstrap-table.
    /// </summary>
    public interface ITableBuilder : ITableApply<ITableBuilder>, IHtmlString
    {
        /// <summary>
        /// Add or retrieve a column.
        /// </summary>
        /// <param name="title">Column title.</param>
        /// <param name="field">Field name.</param>
        /// <param name="sortable">Is sortable.</param>
        /// <param name="sorter">Sorting function.</param>
        IColumnBuilder Column(string title, string field, bool sortable = false, string sorter = null);
    }
}
