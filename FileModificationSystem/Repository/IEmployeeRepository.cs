using FileModificationSystem.Model;
using System.Collections.Generic;

namespace FileModificationSystem.Repository
{
    public interface IEmployeeRepository
    {
        void Add(Employee entity);

        IEnumerable<Employee> Get(int Id);

        IEnumerable<Employee> GetAll();

        void Delete(int Id);

        void Update(Employee entity);
    }
}
