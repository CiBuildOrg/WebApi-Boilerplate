using System;
using App.Core.Contracts;
using App.Core.Utils;
using App.Exceptions;
using Autofac;

namespace App.Core.Implementations
{
    /// <summary>
    /// Resolves any dependency registered in autofac
    /// </summary>
    public class Resolver : IResolver
    {
        private readonly ILifetimeScope _scope;
        private const string ResolverFailure = "Failed to resolve type {0}. More details in the underlying exception.";

        public Resolver(ILifetimeScope scope)
        {
            _scope = scope;
        }
        /// <summary>
        /// Resolves simple dependency based on the autofac di container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            try
            {
                return _scope.Resolve<T>();
            }
            catch (Exception exception)
            {
                throw new ResolverException(string.Format(ResolverFailure, typeof(T).FullName), exception);
            }
        }
    }
}