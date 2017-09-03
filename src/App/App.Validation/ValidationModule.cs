using System.Diagnostics.CodeAnalysis;
using System.Web.Http.Validation;
using App.Validation.Infrastructure;
using Autofac;
using FluentValidation;
using FluentValidation.WebApi;

namespace App.Validation
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Validator"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<FluentValidationModelValidatorProvider>().As<ModelValidatorProvider>();

            builder.RegisterType<AutofacValidatorFactory>().As<IValidatorFactory>().SingleInstance();

            base.Load(builder);
        }
    }
}
