using DataUtility.Infrastructure.Data;
using DataUtility.Infrastructure.Data.Model;
using DataUtility.Infrastructure.Model;
using DataUtility.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DataUtility.Extension
{
    public static class DataUtilityDependencies
    {
        public static void AddDataUtilityDependencies<TEntity>(this IServiceCollection services) where TEntity : Entity
        {
            services.AddTransient<IDataSourceBuilder<TEntity>, DataSourceBuilder<TEntity>>();
            services.AddTransient<Func<DataSource, IRepository<TEntity>>>(
                serviceProvider => (dataSource) =>
                {
                    return dataSource.Source switch
                    {
                        Source.XML => new XMLRepository<TEntity>(dataSource.FilePath),
                        Source.EntityFramework => new EFRepository<TEntity>(dataSource.Context),
                        Source.Json => new JsonRepository<TEntity>(dataSource.FilePath),
                        _ => throw new NotImplementedException()
                    };
                });
        }
    }
}
