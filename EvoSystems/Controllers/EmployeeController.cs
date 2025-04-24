using AutoMapper;
using EvoSystems.Authentication.Interfaces;
using EvoSystems.Dto;
using EvoSystems.Models;
using EvoSystems.Repository;
using EvoSystems.Service.DepartamentService;
using EvoSystems.Service.EmployeeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace EvoSystems.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
     
        private readonly IEmployee _employeeService;
        private readonly IMapper _mapper;
        private readonly ITokenManager _tokenManager;
        public EmployeeController(IEmployee employeeService, IMapper mapper, ITokenManager tokenManager)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<EmployeeDetailDto>>>> GetAll()
        {
            var serviceResponse = await _employeeService.GetAll();
            var employeesDto = _mapper.Map<ServiceResponse<List<EmployeeDetailDto>>>(serviceResponse);
            return Ok(employeesDto);
        }

        [HttpGet("Department/{departmentId}")]
        public async Task<ActionResult<ServiceResponse<List<EmployeeDetailDto>>>> GetEmployeesByDepartment(int departmentId)
        {
            var serviceResponse = await _employeeService.GetEmployeesByDepartment(departmentId);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse); 
            }
            var employeesDto = _mapper.Map<ServiceResponse<List<EmployeeDetailDto>>>(serviceResponse);
            return Ok(employeesDto); 
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<EmployeeDto>>> Add(EmployeeDto newEmployeeDto)
        {
            var newEmployee = _mapper.Map<Employee>(newEmployeeDto);
            var serviceResponse = await _employeeService.Add(newEmployee);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            var serviceResponseDto = _mapper.Map<ServiceResponse<EmployeeDto>>(serviceResponse);
            return Ok(serviceResponseDto);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<EmployeeUpdateDto>>> Update(int id, EmployeeUpdateDto editedEmployeeDto)
        {
            var existingEmployee = await _employeeService.GetById(id);

            if (existingEmployee.Data == null)
            {
                return NotFound("Employee not found");
            }

            _mapper.Map(editedEmployeeDto, existingEmployee.Data);
            var serviceResponse = await _employeeService.Update(existingEmployee.Data);
            var serviceResponseDto = _mapper.Map<ServiceResponse<EmployeeUpdateDto>>(serviceResponse);
            return Ok(serviceResponseDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<EmployeeDetailDto>>> GetById(int id)
        {
            var serviceResponse = await _employeeService.GetById(id);
            var employeeDtoResponse = _mapper.Map<ServiceResponse<EmployeeDetailDto>>(serviceResponse);
            return Ok(employeeDtoResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.RG)) return NotFound();

            var serviceResponse = await _employeeService.GetEmployeeByRG(request.RG);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            var serviceResponseDto = _mapper.Map<ServiceResponse<EmployeeDetailDto>>(serviceResponse);

            var token = _tokenManager.GenerateToken(serviceResponseDto.Data!);
            var refreshToken = _tokenManager.GenerateRefreshToken(serviceResponseDto.Data!);

            return Ok(new LoginResponseDTO(token, refreshToken));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefrashTokenRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest();

            var isValidTokenResult = await _tokenManager.ValidateTokenAsync(request.RefreshToken);

            if (!isValidTokenResult.isValid)
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(isValidTokenResult.RG))
                return Unauthorized();

            var serviceResponse = await _employeeService.GetEmployeeByRG(isValidTokenResult.RG);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }
            var serviceResponseDto = _mapper.Map<ServiceResponse<EmployeeDetailDto>>(serviceResponse);

            var token = _tokenManager.GenerateToken(serviceResponseDto.Data!);
            var refreshToken = _tokenManager.GenerateRefreshToken(serviceResponseDto.Data!);

            return Ok(new LoginResponseDTO(token, refreshToken));
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<EmployeeDto>>> Delete(int id)
        {
            var serviceResponse = await _employeeService.Delete(id);
            var employeeDtoResponse = _mapper.Map<ServiceResponse<EmployeeDto>>(serviceResponse);
            return Ok(employeeDtoResponse);
        }

        [HttpPut("inactiveEmployee/{id}")]
        public async Task<ActionResult<ServiceResponse<Employee>>> Inactive(int id)
        {
            var serviceResponse = await _employeeService.Inactive(id);
            var employeeDtoResponse = _mapper.Map<ServiceResponse<EmployeeDto>>(serviceResponse);
            return Ok(employeeDtoResponse);
        }

        [Authorize]
        [HttpPost("uploadPicture")]
        public async Task<IActionResult> UploadPicture()
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded or file is empty");

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

                var fullPath = Path.Combine(pathToSave, uniqueFileName);
                var dbPath = Path.Combine(folderName, uniqueFileName);


                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { dbPath });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
     
    }
}
