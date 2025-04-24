using EvoSystems.Models;
using EvoSystems.Repository;

namespace EvoSystems.Service.EmployeeService
{
    public interface IEmployee : IRepository<Employee>
    {
        Task<ServiceResponse<List<Employee>>> GetEmployeesByDepartment(int id);
        Task<ServiceResponse<Employee>> GetEmployeeByRG(string RG);
    }
}
