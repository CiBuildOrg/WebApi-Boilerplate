namespace App.Infrastructure.Tracing.Dashboard
{
    /// <summary>
    /// An enumeration representing all of the options that can be set against a table.
    /// </summary>
    public enum TableOption
    {
        /// <summary>
        /// Activate bootstrap table without writing JavaScript.
        /// </summary>
        /// <value>string = table</value>
        [ValueField(Name = "data-toggle", Value = "table")]
        Toggle,

        /// <summary>
        /// The class name of table (default = table table-hover).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-classes")]
        Classes,

        /// <summary>
        /// The height of table.
        /// </summary>
        /// <value>int</value>
        [NameField(Name = "data-height")]
        Height,

        /// <summary>
        /// Defines the default undefined text.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-undefined-text")]
        Undefined,

        /// <summary>
        /// True to stripe the rows (default = false)
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-striped")]
        Striped,

        /// <summary>
        /// Defines which column can be sorted.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-sort-name")]
        SortName,

        /// <summary>
        /// Defines the column sort order as assending.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-sort-order", Value = "asc")]
        SortOrderAsc,

        /// <summary>
        /// Defines the column sort order as decending.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-sort-order", Value = "desc")]
        SortOrderDesc,

        /// <summary>
        /// Defines icon set name.
        /// </summary>
        /// <value>string = 'glyphicon' (default) or 'fa' for FontAwesome</value>
        /// <remarks>1.6</remarks>
        [NameField(Name = "data-icons-prefix")]
        IconsPrefix,

        /// <summary>
        /// Defines icons that used for refresh, toggle and columns buttons.
        /// </summary>
        /// <value>object[] { refresh: 'glyphicon-refresh icon-refresh', toggle: 'glyphicon-list-alt icon-list-alt', columns: 'glyphicon-th icon-th' } </value>
        /// <example>
        /// .Apply(TableOption.iconsPrefix, "fa") //font-awesome
        /// .Apply(TableOption.icons, new { refresh = "fa fa-refresh" })
        /// </example>
        /// <remarks>1.6</remarks>
        [NameField(Name = "data-icons")]
        Icons,

        /// <summary>
        /// The table columns config object, see column properties for more details.
        /// </summary>
        /// <value>string[]</value>
        [NameField(Name = "")]
        Columns,

        /// <summary>
        /// The method type to request remote data (default = get).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-method")]
        Method,

        /// <summary>
        /// A URL to request data from remote site.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-url")]
        Url,

        /// <summary>
        /// False to disable caching of AJAX requests (default = true).
        /// </summary>
        /// <value>boolean</value>
        [ValueField(Name = "data-cache")]
        Cache,

        /// <summary>
        /// The contentType of request remote data (default = application/json).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-content-type")]
        ContentType,

        /// <summary>
        /// The type of data that you are expecting back from the server (default = json).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-data-type")]
        DataType,

        /// <summary>
        /// Additional options for submit ajax request. List of values: http://api.jquery.com/jQuery.ajax.
        /// </summary>
        /// <value>object[]</value>
        /// <remarks>1.6</remarks>
        [NameField(Name = "data-ajax-options")]
        AjaxOptions,

        /// <summary>
        /// When request remote data, sending additional parameters by format the queryParams,
        /// the parameters object contains: pageSize, pageNumber, searchText, sortName, sortOrder.
        /// Return false to stop request (default = function(params) { return params; }).
        /// </summary>
        /// <value>string</value>
        [ValueField(Name = "data-query-params")]
        QueryParams,

        /// <summary>
        /// Set "limit" to send query params width RESTFul type (default = limit).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-query-params-type")]
        QueryParamsType,

        /// <summary>
        /// Before load remote data, handler the response data format, the parameters object 
        /// contains: res: the response data (default = function(res) { return res; }
        /// </summary>
        /// <value>string (function)</value>
        [NameField(Name = "data-response-handler")]
        ResponseHandler,

        /// <summary>
        /// True to show a pagination toolbar on table bottom (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-pagination")]
        Pagination,

        /// <summary>
        /// Defines the side pagination of table, can only be "client" or "server" (default = client).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-side-pagination")]
        SidePagination,

        /// <summary>
        /// When set pagination property, initialize the page number (default = 1).
        /// </summary>
        /// <value>int</value>
        [NameField(Name = "data-page-number")]
        PageNumber,

        /// <summary>
        /// When set pagination property, initialize the page size (default = 10).
        /// </summary>
        /// <value>int</value>
        [NameField(Name = "data-page-size")]
        PageSize,

        /// <summary>
        /// When set pagination property, initialize the page size selecting list (default = [10, 25, 50, 100]).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-page-list")]
        PageList,

        /// <summary>
        /// The name of radio or checkbox input (default = btSelectItem).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-select-item-name")]
        SelectItemName,

        /// <summary>
        /// True to display pagination or card view smartly.
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-smart-display")]
        SmartDisplay,

        /// <summary>
        /// Enable the search input (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-search")]
        Search,

        /// <summary>
        /// Set timeout for search fire.
        /// </summary>
        /// <value>int (default = 500)</value>
        /// <remarks>1.6</remarks>
        [NameField(Name = "data-search-time-out")]
        SearchTimeOut,

        /// <summary>
        /// False to hide the table header (default = true)
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-show-header")]
        ShowHeader,

        /// <summary>
        /// True to show the columns drop down list (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-show-columns")]
        ShowColumns,

        /// <summary>
        /// True to show the refresh button (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-show-refresh")]
        ShowRefresh,

        /// <summary>
        /// True to show the toggle button to toggle table / card view (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-show-toggle")]
        ShowToggle,

        /// <summary>
        /// The minimum count columns to hide of the columns drop down list (default = 1).
        /// </summary>
        /// <value>int</value>
        [NameField(Name = "data-minimum-count-columns")]
        MinimumCountColumns,

        /// <summary>
        /// Indicate which field is an identity field.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-id-field")]
        IdField,

        /// <summary>
        /// True to show card view table, for example mobile view (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-card-view")]
        CardView,

        /// <summary>
        /// Indicate how to align the search input (default = right).
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-search-align", Value = "left")]
        SearchAlignLeft,

        /// <summary>
        /// Indicate how to align the search input (default = right).
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-search-align", Value = "right")]
        SearchAlignRight,

        /// <summary>
        /// Indicate how to align the toolbar buttons (default = right).
        /// </summary>
        /// <value>string = 'left', 'right'</value>
        /// <remarks>1.6.0</remarks>
        [NameField(Name = "data-buttons-align")]
        ButtonsAlign,

        /// <summary>
        /// Indicate how to align the toolbar buttons on the left.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-toolbar-align", Value = "left")]
        ToolbarAlignLeft,

        /// <summary>
        /// Indicate how to align the toolbar buttons on the right.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-toolbar-align", Value = "right")]
        ToolbarAlignRight,

        /// <summary>
        /// True to select checkbox or radiobox when click rows (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-click-to-select")]
        ClickToSelect,

        /// <summary>
        /// True to allow checkbox selecting only one row (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-single-select")]
        SingleSelect,

        /// <summary>
        /// jQuery selector that indicate the toolbar, for example: #toolbar, .toolbar.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-toolbar")]
        Toolbar,

        /// <summary>
        /// False to hide check-all checkbox in header row (default = true).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-checkbox-header")]
        CheckboxHeader,

        /// <summary>
        /// True to maintain selected rows on change page and search (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-maintain-selected")]
        MaintainSelected,

        /// <summary>
        /// False to disable sortable of all columns (default = true).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-sortable")]
        Sortable,

        /// <summary>
        /// The row style formatter function, take two parameters: row: the row record data. 
        /// index: the row index. Support classes or css.
        /// </summary>
        /// <value>string (function)</value>
        [NameField(Name = "data-row-style")]
        RowStyle,

        /// <summary>
        /// Support all custom attributes.
        /// </summary>
        /// <value>Function	{} (parameters - row: the row record data, index: the row index)</value>
        /// <remarks>1.4.0</remarks>
        [NameField(Name = "data-row-attributes")]
        RowAttributes,

        /// <summary>
        /// Set the icons size.
        /// </summary>
        /// <value>?</value>
        /// <remarks>1.6.0</remarks>
        [NameField(Name = "data-icon-size")]
        IconSize,

        /// <summary>
        /// Toolbar button to show or hide the pagination.
        /// </summary>
        /// <remarks>1.6.0</remarks>
        [NameField(Name = "data-show-pagination-switch")]
        ShowPaginationSwitch,

        /// <summary>
        /// Show loading message when table is obtaining data.
        /// </summary>
        [NameField(Name = "data-show-loading")]
        ShowLoading,
    }
}