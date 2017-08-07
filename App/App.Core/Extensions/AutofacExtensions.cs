using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Core.Utils;
using Autofac;
using AutoMapper;

namespace App.Core.Extensions
{
    public static class AutofacExtensions
    {
        /// <summary>
        /// Autowires automapper (mapping profiles and member value resolvers)
        /// </summary>
        /// <param name="builder"><see cref="ContainerBuilder">Autofac container builder</see></param>
        /// <param name="assembly">The assembly in which the profiles/member value resolvers are located</param>
        public static void AutowireAutomapper(this ContainerBuilder builder, Assembly assembly)
        {
            RegisterMemberValueResolvers(builder, assembly);
            RegisterProfiles(builder, assembly);
            builder.Register(context => new MapperConfiguration(configuration =>
                {
                    foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                    {
                        configuration.AddProfile(profile);
                    }
                }))
                .AsSelf()
                .SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();
        }

        private static void RegisterProfiles(ContainerBuilder containerBuilder, Assembly assembly)
        {
            var profileType = typeof(Profile);

            var allProfileTypes = assembly.GetTypes().Where(x => x.BaseType == profileType)
                .ToList();

            foreach (var profile in allProfileTypes)
                containerBuilder.RegisterType(profile).As(profileType);
        }

        private static void RegisterMemberValueResolvers(ContainerBuilder builder, Assembly assembly)
        {
            var interfaceType = typeof(IMemberValueResolver<,,,>);
            var memberValueResolvers = TypeHelper.GetAllTypesImplementingOpenGenericType(interfaceType, assembly).ToList();
            foreach (var memberValueResolver in memberValueResolvers)
            {
                var allBaseClassses = TypeHelper.GetBaseClassesAndInterfaces(memberValueResolver).ToList();

                if (allBaseClassses.Count != 1)
                {
                    continue;
                }

                var baseInterface = allBaseClassses[0];
                builder.RegisterType(memberValueResolver).As(baseInterface).SingleInstance();
            }
        }
    }
}
