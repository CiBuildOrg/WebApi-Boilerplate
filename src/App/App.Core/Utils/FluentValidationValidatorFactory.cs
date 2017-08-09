using System;
using Autofac;
using FluentValidation;

namespace App.Core.Utils
{
    /// <summary>
    /// Validator factory provides the validator instance by resoving from autofac container.
    /// </summary>
    /// <remarks>
    /// Validator should only directly depend on singleton instance. For other lifetime instance should use self-created child scope from ILifetimeScope.
    /// </remarks>
    public class FluentValidationValidatorFactory : IValidatorFactory
    {
        private readonly ILifetimeScope _container;

        public FluentValidationValidatorFactory(ILifetimeScope container)
        {
            _container = container;
        }

        public IValidator<T> GetValidator<T>()
        {
            return (IValidator<T>)GetValidator(typeof(T));
        }

        public IValidator GetValidator(Type type)
        {
            object retVal;
            if (_container.TryResolveKeyed(type, typeof(IValidator), out retVal))
            {
                return (IValidator)retVal;
            }
            return null;
        }
    }
}