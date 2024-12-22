using TaskManagerDapper.Dtos;
using TaskManagerDapper.Models;

namespace TaskManagerDapper.Repositories
{
    public interface ITaskRepository
    {
        Task<EmployeeTask> Save(EmployeeTask task);
        Task<IEnumerable<EmployeeTask>> FindAll();

        Task<EmployeeTask> FindByTaskNumber(String taskNumber);

        Task<EmployeeTask> FindById(long id);

        Task<EmployeeTask> Delete(EmployeeTask task);

        Task<EmployeeTask> UpdateStatusByTaskNumber(String taskNumber,String status);
        Task<EmployeeTask> UpdateTaskByTaskNumber(EmployeeTask task);
    }
}
