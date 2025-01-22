using Entities.models;
using Shared.RequestFeatures;


namespace Contracts
{
    public interface IEmployeeRepository
    {
        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,EmployeeParameters employeeParameters,bool trackchanges);
        Task<Employee> GetEmployeeAsync(Guid companyId,Guid employeeId,bool trackchanges);
        void CreateEmployeeForCompany(Guid companyId, Employee employee);

        void DeleteEmployee(Employee employee);
    }
}
