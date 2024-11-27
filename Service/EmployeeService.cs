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

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackchanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackchanges);
            if(company == null)
                throw new CompanyNotFoundException(companyId);
            var employeesFromDb = _repository.Employee.GetEmployees(companyId, trackchanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
            return employeesDto;
        }

        public EmployeeDto GetEmployee(Guid companyId,Guid employeeId,bool trackchanges)
        {
            var company = _repository.Company.GetCompany(companyId,trackchanges);
            if(company == null)
                throw new CompanyNotFoundException(companyId);
            var employeeDb = _repository.Employee.GetEmployee(companyId,employeeId,trackchanges);
            if (employeeDb == null)
                throw new EmployeeNotFoundException(companyId);
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        } 

        public EmployeeDto CreateEmployeeForCompany(Guid companyId,EmployeeForCreationDto employeeForCreation,bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId,trackChanges);
            if(company == null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.Save();

            var EmployeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return EmployeeToReturn;
        }

        public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeForCompany = _repository.Employee.GetEmployee(companyId, id,
           trackChanges);
            if (employeeForCompany is null)
                throw new EmployeeNotFoundException(id);

            _repository.Employee.DeleteEmployee(employeeForCompany);
            _repository.Save();
        }

        public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate,
bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, compTrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            var employeeEntity = _repository.Employee.GetEmployee(companyId, id,
            empTrackChanges);
            if (employeeEntity is null)
                throw new EmployeeNotFoundException(id);
            _mapper.Map(employeeForUpdate, employeeEntity);
            _repository.Save();
        }
    }
}