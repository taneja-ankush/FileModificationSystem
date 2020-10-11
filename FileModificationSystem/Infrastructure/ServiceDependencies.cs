using DataUtility.Extension;
using DataUtility.Infrastructure.Data.Model;
using DataUtility.Infrastructure.Repository;
using FileModificationSystem.Model;
using FileModificationSystem.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FileModificationSystem.Infrastructure
{
    public static class ServiceDependencies
    {
        public static IServiceCollection AddSevices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDataUtilityDependencies<Employee>();
            services.AddTransient<Func<Source, DbContext, string, IEmployeeRepository>>(
                serviceProvider => (dataSource, dbcontext, filepath) =>
                {
                    return dataSource switch
                    {
                        var source when
                        source == Source.XML ||
                        source ==  Source.Json => new EmployeeRepository(serviceProvider, dataSource, filepath),
                        Source.EntityFramework => new EmployeeRepository(serviceProvider, dbcontext)
                    };
                });

            return services;
        }
    }
}
