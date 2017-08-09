namespace App.Core.Contracts
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}