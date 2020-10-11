using DataUtility.Infrastructure.Data;
using DataUtility.Infrastructure.Data.Model;
using DataUtility.Infrastructure.Repository;
using FileModificationSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace FileModificationSystem.Repository
{
    class EmployeeRepository : IEmployeeRepository
    {
        private readonly IRepository<Employee> _baseRepository;

        private readonly IServiceProvider _serviceProvider;

        public EmployeeRepository(IServiceProvider serviceProvider, Source dataSource, string filePath)
        {
            _serviceProvider = serviceProvider;
            _baseRepository = BuildRepository(dataSource: dataSource, filePath: filePath, context: null);
        }

        public EmployeeRepository(IServiceProvider serviceProvider, DbContext dbContext)
        {
            _serviceProvider = serviceProvider;
            _baseRepository = BuildRepository(dataSource: Source.EntityFramework, context: dbContext, filePath: null);
        }

        private IRepository<Employee> BuildRepository(Source dataSource, DbContext context, string filePath)
        {
            IDataSourceBuilder<Employee> dataSourceBuilder = _serviceProvider.GetService<IDataSourceBuilder<Employee>>();
            dataSourceBuilder.SetDataSource(dataSource);
            dataSourceBuilder.SetFilePath(filePath);
            dataSourceBuilder.SetDbContext(context);
            return dataSourceBuilder.Build();
        }

        public void Delete(Employee entity)
        {
            _baseRepository.Delete(entity.Id);
        }

        public void AddEntity(Employee employee)
        {

            employee.CreatedDate = DateTime.Now;
            _baseRepository.Add(employee);
        }

        public void Add(Employee entity)
        {
            _baseRepository.Add(entity);
        }

        public IEnumerable<Employee> Get(int Id)
        {
            return _baseRepository.Get(Id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _baseRepository.GetAll();
        }

        public void Delete(int Id)
        {
            _baseRepository.Delete(Id);
        }

        public void Update(Employee employee)
        {
            _baseRepository.Update(employee);
        }
    }
}
