using System.Web.Mvc;
using App.Infrastructure.Tracing.Dashboard.Controls;

namespace App.Infrastructure.Tracing.Dashboard
{
    /// <summary>
    /// Represents a collection of extensions that can be applied to the table builder.
    /// </summary>
    public static class BootstrapTableHelpers
    {
        /// <summary>
        /// Returns a BootstrapTable control.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="url">The url that will be used to obtain the data.</param>
        /// <param name="pagination">Is pagination required.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>Html representation of the control.</returns>
        public static ITableBuilder BootstrapTable(this HtmlHelper helper, string url = null, TablePaginationOption pagination = TablePaginationOption.None, object htmlAttributes = null)
        {
            return new TableBuilder(null, url, pagination, htmlAttributes);
        }

        /// <summary>
        /// Returns a BootstrapTable control.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="id">Identity of the control.</param>
        /// <param name="url">The url that will be used to obtain the data.</param>
        /// <param name="pagination">Is pagination required.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>Html representation of the control.</returns>
        public static ITableBuilder BootstrapTable(this HtmlHelper helper, string id, string url, TablePaginationOption pagination = TablePaginationOption.None, object htmlAttributes = null)
        {
            return new TableBuilder(id, url, pagination, htmlAttributes);
        }

        /// <summary>
        /// Returns a BootstrapTable control.
        /// </summary>
        /// <typeparam name="TModel">Model</typeparam>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="url">The url that will be used to obtain the data.</param>
        /// <param name="pagination">Is pagination required</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>Html representation of the control.</returns>
        public static ITableBuilderT<TModel> BootstrapTable<TModel>(this HtmlHelper helper, string url = null, TablePaginationOption pagination = TablePaginationOption.None, object htmlAttributes = null)
        {
            return new TableBuilderT<TModel>(null, url, pagination, htmlAttributes);
        }

        /// <summary>
        /// Returns a BootstrapTable control.
        /// </summary>
        /// <typeparam name="TModel">Model</typeparam>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="id">Identity of the control.</param>
        /// <param name="url">The url that will be used to obtain the data.</param>
        /// <param name="pagination">Is pagination required.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>Html representation of the control.</returns>
        public static ITableBuilderT<TModel> BootstrapTable<TModel>(this HtmlHelper helper, string id, string url, TablePaginationOption pagination = TablePaginationOption.None, object htmlAttributes = null)
        {
            return new TableBuilderT<TModel>(id, url, pagination, htmlAttributes);
        }
    }
}