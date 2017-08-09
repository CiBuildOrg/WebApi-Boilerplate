using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Autofac;

namespace App.Infrastructure.Di
{
    public class AutofacDependencyScope : IDependencyScope
    {
        private ILifetimeScope _scope;

        public AutofacDependencyScope(ILifetimeScope scope)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public void Dispose()
        {
            _scope.Dispose();
            _scope = null;
        }

        public object GetService(Type serviceType)
        {
            return _scope == null
                ? throw new ObjectDisposedException("this", "This scope has already been disposed.")
                : (!_scope.IsRegistered(serviceType) ? null : _scope.Resolve(serviceType));
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _scope == null
                ? throw new ObjectDisposedException("this", "This scope has already been disposed.")
                : (!_scope.IsRegistered(serviceType)
                    ? Enumerable.Empty<object>()
                    : (IEnumerable<object>) _scope.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType)));
        }
    }
}
