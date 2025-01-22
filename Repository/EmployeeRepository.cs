using Contracts;
using Entities.models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;


namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,EmployeeParameters employeeParameters, bool trackchanges)
        {
            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId),trackchanges).OrderBy(e => e.Name).ToListAsync();
            return PagedList<Employee>.ToPagedList(employees,employeeParameters.PageNumber,employeeParameters.PageSize);
        }

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
