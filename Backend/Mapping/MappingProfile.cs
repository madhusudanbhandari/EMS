using AutoMapper;
using Backend.Dtos.Employee;
using Backend.Models;

namespace Backend.Mappings;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Employee,EmployeeResponeDto>()
        .ForMember(
            dest=>dest.DepartmentName,
            opt=>opt.MapFrom(src=>src.Department!.Name)
        );

        CreateMap<CreateEmployeeDto,Employee>();
        CreateMap<UpdateEmployeeDto,Employee>();
    }
}