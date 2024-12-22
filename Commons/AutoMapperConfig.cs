using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskManagerDapper.Dtos;
using TaskManagerDapper.Models;

namespace TaskManagerDapper.Commons
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Employee,CreateEmployeeDto>().ReverseMap();
            CreateMap<EmployeeTask, CreateTaskDto>().ReverseMap().ForMember(dest=>dest.taskDueDate,opt=>opt.MapFrom(src=>src.duedate));
            CreateMap<EmployeeTask, GetTasksDto>().ReverseMap().ForMember(dest=>dest.taskDueDate, opt=>opt.MapFrom(src=>src.duedate));
            CreateMap<EmployeeTask, UpdateTaskDto>().ReverseMap().ForMember(dest=>dest.taskDueDate,opt=>opt.MapFrom(src=>src.duedate));
            CreateMap<TaskLogTimeEntries, CreateLogEntryDto>().ReverseMap()
                    .ForMember(dest => dest.logEntryUser, opt => opt.Ignore())
            .ForPath(dest => dest.logEntryUser.empId, opt => opt.Ignore()); 
            CreateMap<TaskLogTimeEntries, GetLogEntriesDto>().ReverseMap();



        }
    }
}
