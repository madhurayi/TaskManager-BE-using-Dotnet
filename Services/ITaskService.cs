using TaskManagerDapper.Dtos;
using TaskManagerDapper.Models;

namespace TaskManagerDapper.Services
{
    public interface ITaskService
    {
        public Task createTask(CreateTaskDto createTaskDto);
        public Task<IEnumerable<GetTasksDto>> getAllTasks();
        public Task<Boolean> deleteTaskByTaskNumber(String taskNUmber);
        public Task<GetTasksDto> updateTaskByTaskNumber(String taskNumber, UpdateTaskDto updateTaskDto);
        public Task<GetTasksDto> updateTaskStatusByTaskNumber(String taskNumber, String status);
        //public List<GetTasksDto> getTasksByPriority(String priority);
        //public List<GetTasksDto> getTasksByStatus(String status);
        //public List<GetTasksDto> getTasksByPriorityAndStatus(String priority, String status);
        public Task<GetTasksDto> getTaskByTaskNumber(String taskNumber);
        public Task<String> getNewTaskNumber();
    }
}
