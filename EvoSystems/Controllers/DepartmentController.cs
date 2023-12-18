using AutoMapper;
using EvoSystems.Dto;
using EvoSystems.Models;
using EvoSystems.Repository;
using EvoSystems.Service.DepartamentService;
using EvoSystems.Service.EmployeeService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EvoSystems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly IDepartment _departmentService;
        private readonly IMapper _mapper;
        public DepartmentController(IDepartment departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<DepartmentDetailDto>>>> GetAll()
        {
            var serviceResponse = await _departmentService.GetAll();
            var departmentsDto = _mapper.Map<ServiceResponse<List<DepartmentDetailDto>>>(serviceResponse);
            return Ok(departmentsDto);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<DepartmentDto>>> Add(DepartmentDto newDepartmentDto)
        {            
            var newDepartment = _mapper.Map<Department>(newDepartmentDto);
            var serviceResponse = await _departmentService.Add(newDepartment);
            var serviceResponseDto = _mapper.Map<ServiceResponse<DepartmentDto>>(serviceResponse);
            return Ok(serviceResponseDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<DepartmentDto>>> Update(int id, DepartmentDto editedDepartmentDto)
        {
            var existingDepartment = await _departmentService.GetById(id);

            if (existingDepartment.Data == null)
            {
                return NotFound("Department not found");
            }

            _mapper.Map(editedDepartmentDto, existingDepartment.Data);
            var serviceResponse = await _departmentService.Update(existingDepartment.Data);
            var serviceResponseDto = _mapper.Map<ServiceResponse<DepartmentDto>>(serviceResponse);
            return Ok(serviceResponseDto);
        }

        [HttpGet("{id}")]        
        public async Task<ActionResult<ServiceResponse<DepartmentDetailDto>>> GetById(int id)
        {
            var serviceResponse = await _departmentService.GetById(id);
            var departmentDtoResponse = _mapper.Map<ServiceResponse<DepartmentDetailDto>>(serviceResponse);
            return Ok(departmentDtoResponse);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<DepartmentDto>>> Delete(int id)
        {
            var serviceResponse = await _departmentService.Delete(id);
            var departmentDtoResponse = _mapper.Map<ServiceResponse<DepartmentDto>>(serviceResponse);
            return Ok(departmentDtoResponse);
        }

        [HttpPut("inactiveDepartment/{id}")]
        public async Task<ActionResult<ServiceResponse<DepartmentDto>>> Inactive(int id)
        {
            var serviceResponse = await _departmentService.Inactive(id);
            var departmentDtoResponse = _mapper.Map<ServiceResponse<DepartmentDto>>(serviceResponse);
            return Ok(departmentDtoResponse);
        }
    }
}
