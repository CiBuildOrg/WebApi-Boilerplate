using System;
using System.Collections.Generic;
using System.Linq;
using App.Core.Utils;
using Autofac;
using AutoMapper;
using Module = Autofac.Module;

namespace App.Services
{
    public class ServiceModule : Module
    {
        private const string ServicesEnding = "Service";

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.IsClass && t.Name.EndsWith(ServicesEnding))
                .As(t => t.GetInterfaces().Single(i => i.Name.EndsWith(t.Name))).InstancePerLifetimeScope();

            RegisterMemberValueResolvers(builder);
            RegisterProfiles(builder);
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

        private static void RegisterProfiles(ContainerBuilder containerBuilder)
        {
            var profileType = typeof(Profile);

            var allProfileTypes = typeof(ServiceModule).Assembly.GetTypes().Where(x => x.BaseType == profileType)
                .ToList();

            foreach (var profile in allProfileTypes)
                containerBuilder.RegisterType(profile).As(profileType);
        }

        private static void RegisterMemberValueResolvers(ContainerBuilder builder)
        {
            var interfaceType = typeof(IMemberValueResolver<,,,>);
            var allTs = TypeHelper.GetAllTypesImplementingOpenGenericType(interfaceType, typeof(ServiceModule).Assembly).ToList();
            foreach (var tp in allTs)
            {
                var allBaseClassses = TypeHelper.GetBaseClassesAndInterfaces(tp).ToList();
                if (allBaseClassses.Count != 1) continue;
                var baseInterface = allBaseClassses[0];
                builder.RegisterType(tp).As(baseInterface).SingleInstance();
            }
        }
    }
}
