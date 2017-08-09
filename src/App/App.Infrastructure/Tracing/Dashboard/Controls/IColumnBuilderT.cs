namespace App.Infrastructure.Tracing.Dashboard.Controls
{
    /// <exclude/>
    public interface IColumnBuilderT<TModel> : ITableBuilderT<TModel>, IColumnApply<IColumnBuilderT<TModel>>
    {

    }
}