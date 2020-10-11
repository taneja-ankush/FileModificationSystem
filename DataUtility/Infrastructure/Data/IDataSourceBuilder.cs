using DataUtility.CustomException;
using DataUtility.Infrastructure.Data.Model;
using DataUtility.Infrastructure.Model;
using DataUtility.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataUtility.Infrastructure.Data
{
    public interface IDataSourceBuilder<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Set DataSource for which <see cref="IRepository{TEntity}"/> implementation is expected. 
        /// </summary>
        /// <param name="dataSource">DataSource</param>
        void SetDataSource(Source dataSource);

        /// <summary>
        /// Set file path for DataSource dealing with file based repository implementaion. 
        /// If not provided it will look for file name in executing directory with TEntity name 
        /// and if  file does not exist it will create a new file in executing directory.
        /// </summary>
        /// <param name="path">File path.</param>
        void SetFilePath(string path);

        /// <summary>
        /// Set Dbcontext when DataCources set as EntityFramework
        /// </summary>
        /// <param name="dbContext">DBContext</param>
        void SetDbContext(DbContext dbContext);

        /// Build repository instance based on provided <see cref="Source">.
        /// </summary>
        /// <returns>Returns implementation of <see cref="IRepository{TEntity}"></returns>
        /// <exceptions cref="InvalidDataSourceException">If datasource file has exception.</exceptions>
        /// <exceptions cref="ArgumentException">If DbContext is not set in case of <see cref="Source.EntityFramewrk"/>.</exceptions>
        /// <exceptions cref="Exception">Any unhandelled exception while initiating repository instance.</exceptions> 
        IRepository<TEntity> Build();

        /// <summary>
        /// Get data context of provided <see cref="Source">.
        /// </summary>
        /// <returns>Context <see cref="object"></returns>
        object GetDataContext();
    }
}
