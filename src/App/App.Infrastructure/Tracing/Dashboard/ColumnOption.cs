namespace App.Infrastructure.Tracing.Dashboard
{
    /// <summary>
    /// An enumeration representing all of the options that can be set against a column.
    /// </summary>
    public enum ColumnOption
    {
        /// <summary>
        /// True to show a radio. The radio column has fixed width (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-radio")]
        Radio,

        /// <summary>
        /// True to show a checkbox. The checkbox column has fixed width (defualt = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-checkbox")]
        Checkbox,

        /// <summary>
        /// The column field name.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-field")]
        Field,

        /// <summary>
        /// The column title text.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-title")]
        Title,

        /// <summary>
        /// The column class name.
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-class")]
        Class,

        /// <summary>
        /// Indicate how to align the column data.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-align", Value = "left")]
        AlignLeft,

        /// <summary>
        /// Indicate how to align the column data.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-align", Value = "right")]
        AlignRight,

        /// <summary>
        /// Indicate how to align the column data.
        /// </summary>
        /// <value>bool</value>        
        [ValueField(Name = "data-align", Value = "center")]
        AlignCenter,

        /// <summary>
        /// Indicate how to align the table header.
        /// </summary>
        /// <value>bool</value>
        /// <remarks>1.4.0</remarks>
        [ValueField(Name = "data-halign", Value = "left")]
        HalignLeft,

        /// <summary>
        /// Indicate how to align the table header.
        /// </summary>
        /// <value>bool</value>
        /// <remarks>1.4.0</remarks>
        [ValueField(Name = "data-halign", Value = "right")]
        HalignRight,

        /// <summary>
        /// Indicate how to align the table header.
        /// </summary>
        /// <value>bool</value>
        /// <remarks>1.4.0</remarks>
        [ValueField(Name = "data-halign", Value = "center")]
        HalignCenter,

        /// <summary>
        /// Indicate how to align the cell data.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-valign", Value = "top")]
        ValignTop,

        /// <summary>
        /// Indicate how to align the cell data.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-valign", Value = "middle")]
        ValignMiddle,

        /// <summary>
        /// Indicate how to align the cell data.
        /// </summary>
        /// <value>bool</value>
        [ValueField(Name = "data-valign", Value = "bottom")]
        ValignBottom,

        /// <summary>
        /// The width of column. If not defined, the width will auto expand to fit its contents.
        /// </summary>
        /// <value>int</value>
        [NameField(Name = "data-width")]
        Width,

        /// <summary>
        /// True to allow the column can be sorted (default = false).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-sortable")]
        Sortable,

        /// <summary>
        /// The default sort order (default = asc).
        /// </summary>
        /// <value>string "asc" or "desc"</value>
        [NameField(Name = "data-order")]
        Order,

        /// <summary>
        /// False to hide the columns item (default = true)
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-visible")]
        Visible,

        /// <summary>
        /// False to disable the switchable of columns item (default = true).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-switchable")]
        Switchable,

        /// <summary>
        /// True to select checkbox or radiobox when the column is clicked (default = true).
        /// </summary>
        /// <value>bool</value>
        [NameField(Name = "data-click-to-select")]
        ClickToSelect,

        /// <summary>
        /// The cell formatter function, take three parameters: value: the field value. row: 
        /// the row record data. index: the row index (function).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-formatter")]
        Formatter,

        /// <summary>
        /// The cell events listener when you use formatter function, take three parameters: 
        /// event: the jQuery event. value: the field value. row: the row record data. index:
        /// the row index (function).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-events")]
        Events,

        /// <summary>
        /// The custom field sort function that used to do local sorting, take two parameters:
        /// a: the first field value. b: the second field value (function).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-sorter")]
        Sorter,

        /// <summary>
        /// The cell style formatter function, take three parameters: value: the field value.
        /// row: the row record data. index: the row index. Support classes or css (function).
        /// </summary>
        /// <value>string</value>
        [NameField(Name = "data-cell-style")]
        CellStyle,

        /// <summary>
        /// True to search data for this column.
        /// </summary>
        /// <value>bool</value>
        /// <remarks>1.5.0</remarks>
        [NameField(Name = "data-searchable")]
        Searchable,
    }
}