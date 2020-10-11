using DataUtility.Infrastructure.Data.Model;
using FileModificationSystem.Infrastructure;
using FileModificationSystem.Model;
using FileModificationSystem.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileModificationSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            var services = ServiceDependencies.AddSevices();

            using (var _serviceProvider = services.BuildServiceProvider())
            {
                var employeeResolver = _serviceProvider.GetService<Func<Source, DbContext, string, IEmployeeRepository>>();

                #region XMLRepository

                Console.WriteLine($"Start =============== XMLRepository ===================== Start{Environment.NewLine}");

                // Creating employee XMLRepository implementation.
                IEmployeeRepository employeeXMLRepositoryImplementation = employeeResolver(Source.XML, null, string.Empty);
                XMLORJsonRepositoryUsage(employeeXMLRepositoryImplementation);

                Console.WriteLine($"End =============== XMLRepository ===================== End{Environment.NewLine}");

                #endregion XMLRepository

                
                #region EFRepository

                Console.WriteLine($"Start =============== EntityFrameworkRepository ===================== Start{Environment.NewLine}");

                // Pre-requisite : Use "update-database" command in your "Program Manager Console" 
                // in order to have employee.db file created to be used in EF.
                // Create employee db context
                using (var dbContext = new EmployeeDbContext())
                {
                    // Creating employee EFRepository implementation.
                    IEmployeeRepository employeeEFRepositoryImplementation = employeeResolver(Source.EntityFramework, dbContext, string.Empty);

                    EntityFrameworkRepositoryUsage(employeeEFRepositoryImplementation);

                }

                Console.WriteLine($"End =============== EntityFrameworkRepository ===================== End{Environment.NewLine}");

                #endregion EFRepository

                #region JsonRepository

                Console.WriteLine($"Start =============== JsonRepository ===================== Start{Environment.NewLine}");

                // Creating employee JsonRepository implementation.
                IEmployeeRepository employeeJSonRepositoryImplementation = employeeResolver(Source.Json, null, string.Empty);
                XMLORJsonRepositoryUsage(employeeJSonRepositoryImplementation);

                Console.WriteLine($"End =============== JsonRepository ===================== End{Environment.NewLine}");

                #endregion JsonRepository

                Console.ReadLine();
            }

        }

        private static void XMLORJsonRepositoryUsage(IEmployeeRepository empRepo)
        {
            IEnumerable<Employee> allEmployees;

            Console.WriteLine($"Start =============== Get all pre existing employees ===================== Start{Environment.NewLine}");
            allEmployees = empRepo.GetAll();
            Console.WriteLine($"Pre-Existing records {Environment.NewLine}{JsonConvert.SerializeObject(allEmployees, Formatting.Indented)}");
            Console.WriteLine($"End =============== Get all pre existing employees ===================== End{Environment.NewLine}");

            // Add new employee details
            var emp1 = new Employee()
            {
                Id = 1,
                Age = 21,
                Designation = "Designation 1",
                Name = "Employee 1",
                Address = new Address() { City = "City 1" },
                Qualification = new Qualification() { Graduation = "B.E." }
            };
            empRepo.Add(emp1);

            Console.WriteLine($"Start =============== Added Employee 1 ===================== Start{Environment.NewLine}");
            allEmployees = empRepo.GetAll();
            Console.WriteLine($"Updated result with Employee 1 - {Environment.NewLine}{JsonConvert.SerializeObject(allEmployees, Formatting.Indented)}");
            Console.WriteLine($"End =============== Added Employee 1 ===================== End{Environment.NewLine}");

            // Add new employee details
            var emp2 = new Employee()
            {
                Id = 2,
                Age = 22,
                Designation = "Designation 2",
                Name = "Employee 2",
                Address = new Address() { City = "City 2" },
                Qualification = new Qualification() { Graduation = "B.Tech." }
            };
            empRepo.Add(emp2);

            Console.WriteLine($"Start =============== Added Employee 2 ===================== Start{Environment.NewLine}");
            allEmployees = empRepo.GetAll();
            Console.WriteLine($"Updated result with Employee 2 - {Environment.NewLine}{JsonConvert.SerializeObject(allEmployees, Formatting.Indented)}");
            Console.WriteLine($"End =============== Added Employee 2 ===================== End{Environment.NewLine}");

            // Get employee1 details
            Console.WriteLine($"Start =============== Get Employee 1 with Id ===================== Start{Environment.NewLine}");
            var employee1Details = empRepo.Get(1);
            Console.WriteLine($"Employee 1 details - {Environment.NewLine}{JsonConvert.SerializeObject(employee1Details, Formatting.Indented)}");
            Console.WriteLine($"End =============== Get Employee 1 with Id ===================== End{Environment.NewLine}");

            // Update age of newly added employee 
            // Add new employee details
            var emp2update = new Employee()
            {
                Id = 2,
                Age = 24,
                Designation = "Designation 2 updated",
                Name = "Employee 2",
                Address = new Address() { City = "City 2 updated" },
                Qualification = new Qualification() { Graduation = "B.Tech." }
            };
            empRepo.Update(emp2update);
            Console.WriteLine($"Start =============== Get all employees after updating Employee 2 age from 22 to 24 ===================== Start{Environment.NewLine}");
            allEmployees = empRepo.GetAll();
            Console.WriteLine($"Updated result with Employee 2 age {Environment.NewLine}{JsonConvert.SerializeObject(allEmployees, Formatting.Indented)}");
            Console.WriteLine($"End =============== Get all employees after updating Employee 2 age from 22 to 24 ===================== End{Environment.NewLine}");

            // Delete Employee with Id equal to 1
            Console.WriteLine($"Start =============== Deleted Employee with Id equal to 1 ===================== Start{Environment.NewLine}");
            var emplyoeeID = 1;
            empRepo.Delete(emplyoeeID);
            allEmployees = empRepo.GetAll();
            Console.WriteLine($"Updated result after deleting Mohan {Environment.NewLine}{JsonConvert.SerializeObject(allEmployees, Formatting.Indented)}");
            Console.WriteLine($"End =============== Deleted Employee with Id equal to 1 ===================== End{Environment.NewLine}");
        }

        private static void EntityFrameworkRepositoryUsage(IEmployeeRepository employeeRepo)
        {
            var abc = employeeRepo.Get(5);
            Console.WriteLine($"Start =============== Get all pre existing employees ===================== Start{Environment.NewLine}");
            var emps = employeeRepo.GetAll();
            Console.WriteLine($"Pre-Existing records {Environment.NewLine}{JsonConvert.SerializeObject(emps, Formatting.Indented)}");
            Console.WriteLine($"End =============== Get all pre existing employees ===================== End{Environment.NewLine}");



            Console.WriteLine($"Start =============== Deleted all pre existing employees ===================== Start{Environment.NewLine}");

            foreach (var empobj in emps)
                employeeRepo.Delete(empobj.Id);

            emps = employeeRepo.GetAll();
            Console.WriteLine($"Pre-Existing records {Environment.NewLine}{JsonConvert.SerializeObject(emps, Formatting.Indented)}");
            Console.WriteLine($"End =============== Deleted all pre existing employees ===================== End{Environment.NewLine}");

            employeeRepo.Add(new Employee()
            {
                Qualification = new Qualification() { Graduation = "B.Tech" },
                Name = "Test Employee 1",
                Id = 1,
                Designation = "Test Designation 1",
                CreatedDate = DateTime.Now,
                Age = 21,
                Address = new Address() { City = "Ludhiana" },
            });
            Console.WriteLine($"Start =============== Added Employee 1  ===================== Start{Environment.NewLine}");
            emps = employeeRepo.GetAll();
            Console.WriteLine($"Updated result with Employee 1 - {Environment.NewLine}{JsonConvert.SerializeObject(emps, Formatting.Indented)}");
            Console.WriteLine($"End =============== Added Employee 1  ===================== End{Environment.NewLine}");


            employeeRepo.Add(new Employee()
            {
                Qualification = new Qualification() { Graduation = "B.Tech" },
                Name = "Test Employee 2",
                Id = 2,
                Designation = "Test Designation 2",
                CreatedDate = DateTime.Now,
                Age = 22,
                Address = new Address() { City = "Moga" },
            });
            Console.WriteLine($"Start =============== Added Employee 2 ===================== Start{Environment.NewLine}");
            emps = employeeRepo.GetAll();
            Console.WriteLine($"Updated result with Employee 2 - {Environment.NewLine}{JsonConvert.SerializeObject(emps, Formatting.Indented)}");
            Console.WriteLine($"End =============== Added Employee 2  ===================== End{Environment.NewLine}");

            var employeeWithID2 = employeeRepo.Get(2).FirstOrDefault();
            if (employeeWithID2 != null)
            {
                employeeWithID2.Name = "Test Employee 2 Updated";
                employeeWithID2.Designation = "Test Designation 2 Updated";
                employeeWithID2.Age = 23;
                employeeRepo.Update(employeeWithID2);
            }

            Console.WriteLine($"Start =============== Updated Employee 2 ===================== Start{Environment.NewLine}");
            emps = employeeRepo.GetAll();
            Console.WriteLine($"Updated result - {Environment.NewLine}{JsonConvert.SerializeObject(emps, Formatting.Indented)}");
            Console.WriteLine($"End =============== Updated Employee 2  ===================== End{Environment.NewLine}");
        }
    }
}
