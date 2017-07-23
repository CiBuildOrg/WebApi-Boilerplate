using System.Web.Http.Dependencies;
using Autofac;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

namespace App.Infrastructure.Di
{
    public class AutofacResolver : AutofacDependencyScope, IDependencyResolver
    {
        private readonly IContainer _container;

        public AutofacResolver(IContainer container)
            : base(container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new AutofacDependencyScope(_container.BeginLifetimeScope());
        }
    }
}