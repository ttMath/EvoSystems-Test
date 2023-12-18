using AutoMapper;
using EvoSystems.Dto;
using EvoSystems.Models;
using Microsoft.AspNetCore.Mvc;

namespace EvoSystems.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ServiceResponse<List<Department>>, ServiceResponse<List<DepartmentDto>>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<ServiceResponse<Department>, ServiceResponse<DepartmentDto>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();

            CreateMap<ServiceResponse<List<Department>>, ServiceResponse<List<DepartmentDetailDto>>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<ServiceResponse<Department>, ServiceResponse<DepartmentDetailDto>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<Department, DepartmentDetailDto>();
            CreateMap<DepartmentDetailDto, Department>();            

            CreateMap<ServiceResponse<List<Employee>>, ServiceResponse<List<EmployeeDto>>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<ServiceResponse<Employee>, ServiceResponse<EmployeeDto>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<ServiceResponse<List<Employee>>, ServiceResponse<List<EmployeeDetailDto>>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<ServiceResponse<Employee>, ServiceResponse<EmployeeDetailDto>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<Employee, EmployeeDetailDto>();
            CreateMap<EmployeeDetailDto, Employee>();

            CreateMap<ServiceResponse<Employee>, ServiceResponse<EmployeeUpdateDto>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<Employee, EmployeeUpdateDto>();
            CreateMap<EmployeeUpdateDto, Employee>();
        }
    }
}
