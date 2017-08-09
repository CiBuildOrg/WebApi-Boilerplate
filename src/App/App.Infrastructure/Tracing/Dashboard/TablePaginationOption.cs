namespace App.Infrastructure.Tracing.Dashboard
{
    ///<summary>
    ///Represents the options available for client or server side pagination.
    ///</summary>
    public enum TablePaginationOption
    {
        /// <summary></summary>
        [ValueField(Name = "data-side-pagination", Value = "")]
        None,
        /// <summary></summary>
        [ValueField(Name = "data-side-pagination", Value = "client")]
        Client,
        /// <summary></summary>
        [ValueField(Name = "data-side-pagination", Value = "server")]
        Server,
    }
}