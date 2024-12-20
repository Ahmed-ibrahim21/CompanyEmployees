﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        
        public EmployeeService(IRepositoryManager repository, ILoggerManager logger,IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackchanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackchanges);
            if(company == null)
                throw new CompanyNotFoundException(companyId);
            var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId, trackchanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
            return employeesDto;
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId,Guid employeeId,bool trackchanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId,trackchanges);
            if(company == null)
                throw new CompanyNotFoundException(companyId);
            var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId,employeeId,trackchanges);
            if (employeeDb == null)
                throw new EmployeeNotFoundException(companyId);
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        } 

        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId,EmployeeForCreationDto employeeForCreation,bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId,trackChanges);
            if(company == null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
           await _repository.SaveAsync();

            var EmployeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return EmployeeToReturn;
        }

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeForCompany = await _repository.Employee.GetEmployeeAsync(companyId, id,
           trackChanges);
            if (employeeForCompany is null)
                throw new EmployeeNotFoundException(id);

            _repository.Employee.DeleteEmployee(employeeForCompany);
           await _repository.SaveAsync();
        }

        public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate,bool compTrackChanges, bool empTrackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id,
            empTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(id);
            _mapper.Map(employeeForUpdate, employeeEntity);
          await  _repository.SaveAsync();
        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var EmployeeEntity = await _repository.Employee.GetEmployeeAsync(companyId,id,empTrackChanges);
            
            if(EmployeeEntity is null)
                throw new EmployeeNotFoundException(id);
            
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(EmployeeEntity);

            return (employeeToPatch, EmployeeEntity);
        }

        public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
           await _repository.SaveAsync();
        }
    }
}
