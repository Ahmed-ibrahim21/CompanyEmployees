using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record class CompanyForCreationDto(string Name,string address,string Country,
        IEnumerable<EmployeeForCreationDto> Employees);
}
