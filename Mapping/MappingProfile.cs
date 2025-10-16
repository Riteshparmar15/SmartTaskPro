using AutoMapper;
using SmartTaskPro.DTOs;
using SmartTaskPro.Models;

namespace SmartTaskPro.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserDto>().ForMember(d => d.Role, o => o.MapFrom(s => s.Role.ToString()));
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<TaskItem, TaskDto>().ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<UpdateTaskDto, TaskItem>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
