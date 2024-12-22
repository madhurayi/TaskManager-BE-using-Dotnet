
using TaskManagerDapper.Models;

namespace TaskManagerDapper.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> Save(Employee employee);
        Task<IEnumerable<Employee>> FindAll();
        Task<Employee> FindByEmpId(String empId);
        Task<Employee> FindById(long id);

    }
}
