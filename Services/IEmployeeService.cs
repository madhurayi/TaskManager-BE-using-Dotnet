using TaskManagerDapper.Dtos;
using TaskManagerDapper.Models;

namespace TaskManagerDapper.Services
{
    public interface IEmployeeService
    {
        public  Task<Employee> saveEmployee(CreateEmployeeDto createEmployeeDto);
        public  Task<IEnumerable<Employee>> getAllEMployees();

    }
}
