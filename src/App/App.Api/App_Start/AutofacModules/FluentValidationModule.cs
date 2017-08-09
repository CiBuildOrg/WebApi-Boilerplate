using App.Core.Utils;
using Autofac;
using FluentValidation;

namespace App.Api.App_Start.AutofacModules
{
    public class FluentValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // assemblies
            var assemblies = AutofacAssemblyStore.GetAssemblies();

            // validators: types derived from AbstractValidator<>
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
                .Keyed(type => type.BaseType?.GenericTypeArguments[0], typeof(IValidator))
                .SingleInstance();
        }
    }
}