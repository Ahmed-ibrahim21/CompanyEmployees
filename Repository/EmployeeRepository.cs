﻿using Contracts;
using Entities.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackchanges) =>
          await  FindByCondition(e => e.CompanyId.Equals(companyId), trackchanges)
                .OrderBy(e => e.Name).ToListAsync();

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackchanges) =>
           await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackchanges).SingleOrDefaultAsync();

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);

    }
}
