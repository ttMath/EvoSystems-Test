using EvoSystems.DataContext;
using EvoSystems.Models;
using EvoSystems.Persistence.Repository;
using EvoSystems.Repository;
using EvoSystems.Service.EmployeeService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace EvoSystems.Service.DepartamentService
{
    public class DepartmentService : IDepartment
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IEmployee _employeeService;

        public DepartmentService(IRepository<Department> departmentRepository, IEmployee employeeService)
        {
            _departmentRepository = departmentRepository;
            _employeeService = employeeService;
        }

        public async Task<ServiceResponse<Department>> Add(Department department)
        {
            ServiceResponse<Department> serviceResponse = new ServiceResponse<Department>();

            try
            {
                if (department == null || string.IsNullOrWhiteSpace(department.Name) || string.IsNullOrWhiteSpace(department.Abbreviation))
                {
                    serviceResponse.Message = "Name or Abbreviation cannot be null or empty.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                if (department.Abbreviation.Length > 3)
                {
                    serviceResponse.Message = "Abbreviation should not exceed 3 characters.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                return await _departmentRepository.Add(department);
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
                return serviceResponse;
            }
        }


        public async Task<ServiceResponse<Department>> Delete(int id)
        {
            var employees = await _employeeService.GetEmployeesByDepartment(id);

            if (employees.Data != null && employees.Data.Count > 0)
            {
                return new ServiceResponse<Department>
                {
                    Success = false,
                    Message = "Cannot delete department. Employees are associated with this department."
                };
            }
            else
            {
                return await _departmentRepository.Delete(id);
            }
        }

        public async Task<ServiceResponse<List<Department>>> GetAll()
        {
            return await _departmentRepository.GetAll();
        }

        public async Task<ServiceResponse<Department>> GetById(int id)
        {
            return await _departmentRepository.GetById(id);
        }

        public async Task<ServiceResponse<Department>> Inactive(int id)
        {
            var departmentResponse = await _departmentRepository.Inactive(id);

            if (departmentResponse.Success)
            {
                var employeesResponse = await _employeeService.GetEmployeesByDepartment(id);

                if (employeesResponse.Success && employeesResponse.Data != null)
                {
                    foreach (var employee in employeesResponse.Data)
                    {
                        if (employee != null)
                        {
                            await _employeeService.Inactive(employee.Id);
                        }
                        else
                        {
                            return departmentResponse;
                        }
                    }
                }
            }
            return departmentResponse;
        }
        public async Task<ServiceResponse<Department>> Update(Department department)
        {
            return await _departmentRepository.Update(department);
        }
    }
}
