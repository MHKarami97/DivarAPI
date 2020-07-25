using Data;
using Common;
using Autofac;
using Data.Contracts;
using Entities.Common;
using Services.CronJob;
using Data.Repositories;
using Services.Services;
using Services.Security;
using AspNetCoreRateLimit;
using Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebFramework.Configuration
{
    public class AutofacConfigurationExtensions : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            containerBuilder.RegisterGeneric(typeof(ScheduleConfig<>))
                .As(typeof(IScheduleConfig<>))
                .SingleInstance();

            containerBuilder.RegisterType<MemoryCacheIpPolicyStore>()
                .As<IIpPolicyStore>()
                .SingleInstance();

            containerBuilder.RegisterType<MemoryCacheRateLimitCounterStore>()
                .As<IRateLimitCounterStore>()
                .SingleInstance();

            containerBuilder.RegisterType<MemoryCacheClientPolicyStore>()
                .As<IClientPolicyStore>()
                .SingleInstance();

            containerBuilder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .SingleInstance();

            containerBuilder.RegisterType<RateLimitConfiguration>()
                .As<IRateLimitConfiguration>()
                .SingleInstance();

            containerBuilder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>()
                .SingleInstance();

            containerBuilder.RegisterType<Security>().As<ISecurity>();

            containerBuilder.RegisterType<Date>().As<IDate>();

            var commonAssembly = typeof(SiteSettings).Assembly;
            var entitiesAssembly = typeof(IEntity).Assembly;
            var dataAssembly = typeof(ApplicationDbContext).Assembly;
            var servicesAssembly = typeof(JwtService).Assembly;
            var repositoriesAssembly = typeof(IBaseRepository).Assembly;

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly, repositoriesAssembly)
                .AssignableTo<IScopedDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly, repositoriesAssembly)
                .AssignableTo<ITransientDependency>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly, repositoriesAssembly)
                .AssignableTo<ISingletonDependency>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
