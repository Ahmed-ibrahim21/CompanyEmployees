﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public EmployeesController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
        {
            var employees = await _service.EmployeeService.GetEmployeesAsync(companyId, trackchanges:false);
            return Ok(employees);
        }

        [HttpGet("{employeeId:guid}",Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId,Guid employeeId) 
        {
            var employee = await _service.EmployeeService.GetEmployeeAsync(companyId, employeeId, trackchanges:false);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee)
        {
            if (employee == null)
                return BadRequest("EmployeeForCreationDto object is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var employeeToReturn = await _service.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee,trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany",new {companyId=companyId,employeeId = employeeToReturn.id},employeeToReturn); 
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
           await _service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, trackChanges:
            false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employee)
        {
            if (employee is null)
                return BadRequest("EmployeeForUpdateDto object is null");
            
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

           await _service.EmployeeService.UpdateEmployeeForCompanyAsync(companyId, id, employee,
            compTrackChanges: false, empTrackChanges: true);
            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PatiallyUpdateEmployeeForCompany(Guid companyId,Guid id,[FromBody] JsonPatchDocument<EmployeeForUpdateDto> PatchDoc)
        {
            if (PatchDoc is null)
                return BadRequest("PatchDoc object sent from client is null.");

            var result = await _service.EmployeeService.GetEmployeeForPatchAsync(companyId, id,compTrackChanges: false,empTrackChanges: true);

            PatchDoc.ApplyTo(result.employeeToPatch,ModelState);
            
            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

          await  _service.EmployeeService.SaveChangesForPatchAsync(result.employeeToPatch, result.employeeEntity);

            return NoContent();
        }
    }
}
