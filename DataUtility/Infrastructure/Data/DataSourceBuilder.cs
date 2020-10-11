using DataUtility.CustomException;
using DataUtility.Infrastructure.Data.Model;
using DataUtility.Infrastructure.Model;
using DataUtility.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace DataUtility.Infrastructure.Data
{
    public class DataSourceBuilder<TEntity> : IDataSourceBuilder<TEntity> where TEntity : Entity
    {
        private object _context { get; set; }

        private IServiceProvider _serviceProvider { get; set; }

        public Source _dataSource { get; private set; }

        public string _path { get; private set; }

        public DbContext _dbContext { get; private set; }

        public DataSourceBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Build repository instance based on provided <see cref="Source">.
        /// </summary>
        /// <returns>Returns implementation of <see cref="IRepository{TEntity}"></returns>
        /// <exceptions cref="InvalidDataSourceException">If datasource file has exception.</exceptions>
        /// <exceptions cref="ArgumentException">If DbContext is not set in case of <see cref="Source.EntityFramewrk"/>.</exceptions>
        /// <exceptions cref="Exception">Any unhandelled exception while initiating repository instance.</exceptions>
        public IRepository<TEntity> Build()
        {
            IRepository<TEntity> repository = null;
            try
            {
                var repositoryResolver = _serviceProvider.GetService<Func<DataSource, IRepository<TEntity>>>();
                switch (_dataSource)
                {
                    case Source.XML:
                        var xmlFilePath = string.IsNullOrWhiteSpace(_path) ? $"{typeof(TEntity).Name}{FileExtension.FileTypeXML}" : Path.Combine(_path, $"{typeof(TEntity).Name}{FileExtension.FileTypeXML}");
                        repository = repositoryResolver(new DataSource() { Source = _dataSource, Context = null, FilePath = xmlFilePath });
                        _context = repository.Context;
                        break;
                    case Source.Json:
                        var jsonFilePath = string.IsNullOrWhiteSpace(_path) ? $"{typeof(TEntity).Name}{FileExtension.FileTypeJSON}" : Path.Combine(_path, $"{typeof(TEntity).Name}{FileExtension.FileTypeJSON}");
                        repository = repositoryResolver(new DataSource() { Source = _dataSource, Context = null, FilePath = jsonFilePath });
                        _context = repository.Context;
                        break;
                    case Source.EntityFramework:
                        if (_dbContext == null)
                        {
                            throw new ArgumentException("Kindly set database context as datasource, prior building using EntityFramework as datasource.");
                        }
                        repository = repositoryResolver(new DataSource() { Source = _dataSource, Context = _dbContext, FilePath = null });
                        break;
                    default:
                        throw new ArgumentException("Invalid data source.");
                }
            }
            catch (InvalidDataSourceException invalidDataSourceException)
            {
                // Log and throw exception further or Handle the exception.
            }
            catch (ArgumentException argumentException)
            {
                // Log and throw exception further or Handle the exception.
            }
            catch (Exception exception)
            {
                // Log and throw exception further or Handle the exception.
            }

            return repository;
        }

        /// <summary>
        /// Get data context of provided <see cref="Source">.
        /// </summary>
        /// <returns>Context. Example - <see cref="DbContext"> is returned as object in case of EntityFramework. ></returns>
        public object GetDataContext()
        {
            return _context;
        }

        /// <summary>
        /// Set file path for DataSource dealing with file based repository implementaion. 
        /// If not provided it will look for file name in executing directory with TEntity name 
        /// and if  file does not exist it will create a new file in executing directory with TEntity name.
        /// </summary>
        /// <param name="path">File path.</param>
        public void SetFilePath(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Set Dbcontext when DataCources set as EntityFramework
        /// </summary>
        /// <param name="dbContext">DBContext</param>
        public void SetDbContext(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Set DataSource for which <see cref="IRepository{TEntity}"/> implementation is expected. 
        /// </summary>
        /// <param name="dataSource">DataSource</param>
        public void SetDataSource(Source dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
