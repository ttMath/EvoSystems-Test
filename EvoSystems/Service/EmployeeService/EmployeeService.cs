using EvoSystems.DataContext;
using EvoSystems.Dto;
using EvoSystems.Models;
using EvoSystems.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EvoSystems.Service.EmployeeService
{
    public class EmployeeService : IEmployee
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly AppDBContext _context;

        public EmployeeService(AppDBContext context, IRepository<Employee> employeeRepository)
        {
            _context = context;
            _employeeRepository = employeeRepository;
        }

        public async Task<ServiceResponse<Employee>> Add(Employee employee)
        {
            ServiceResponse<Employee> serviceResponse = new ServiceResponse<Employee>();

            try
            {
                if (employee.DepartmentId == 0 || string.IsNullOrWhiteSpace(employee.Name) || string.IsNullOrWhiteSpace(employee.RG))
                {
                    serviceResponse.Message = "Data is missing or empty.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                if (employee.RG.Length > 9)
                {
                    serviceResponse.Message = "RG should not exceed 9 characters.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                Department department = await _context.Department.FindAsync(employee.DepartmentId);

                if (department == null)
                {
                    serviceResponse.Message = "Department not found.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                if (!department.IsActive)
                {
                    serviceResponse.Message = "Department is inactive. Cannot create employee.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                employee.Department = department;
                serviceResponse.Data = employee;
                serviceResponse.Message = "Employee successfully created.";

                return await _employeeRepository.Add(employee);
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<Employee>> Delete(int id)
        {
            return await _employeeRepository.Delete(id);
        }

        public async Task<ServiceResponse<List<Employee>>> GetAll()
        {
            return await _employeeRepository.GetAll();
        }

        public async Task<ServiceResponse<Employee>> GetById(int id)
        {
            return await _employeeRepository.GetById(id);
        }

        public async Task<ServiceResponse<List<Employee>>> GetEmployeesByDepartment(int departmentId)
        {
            ServiceResponse<List<Employee>> serviceResponse = new ServiceResponse<List<Employee>>();

            try
            {
                var departmentExists = await _context.Department.AnyAsync(d => d.Id == departmentId);

                if (!departmentExists)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Department with ID " + departmentId + " does not exist.";
                    return serviceResponse;
                }

                var employees = await _context.Employee
                    .Where(e => e.DepartmentId == departmentId)
                    .ToListAsync();

                if (employees.Count < 1)
                {
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Department has no employees";
                }
                else
                {
                    serviceResponse.Success = true;
                    serviceResponse.Data = employees;
                    serviceResponse.Message = "Employees retrieved successfully";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Error retrieving employees: " + ex.Message;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<Employee>> Inactive(int id)
        {
            return await _employeeRepository.Inactive(id);
        }

        public async Task<ServiceResponse<Employee>> Update(Employee employee)
        {
            return await _employeeRepository.Update(employee);
        }
    }
}
