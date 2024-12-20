﻿using Entities.models;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackchanges);
        Task<EmployeeDto> GetEmployeeAsync(Guid companyId,Guid employeeId,bool trackchanges);

        Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);

        Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);

        Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);


            Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
            Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);

        Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee
        employeeEntity);
    }
}
