using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Controllers;
using TaskManagerDapper.Dtos;
using TaskManagerDapper.Models;
using TaskManagerDapper.Repositories;
using TaskManagerDapper.Services;

namespace TaskManagerDapper.Controllers
{
    [Route("api/v1/task")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        public readonly ITaskService taskService;
        public readonly ITaskRepository taskRepository;
        private readonly ILogger<EmployeeController> _logger;
        public TaskController(ITaskService taskService,ITaskRepository taskRepository, ILogger<EmployeeController> logger)
        {
            this.taskService = taskService;
            this.taskRepository = taskRepository;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks() {
            _logger.LogInformation("Received request to fetch all tasks.");
            try
            {
                var tasks=await taskService.getAllTasks();
                _logger.LogInformation("Successfully fetched all tasks.");
                return Ok(tasks);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while fetching all tasks.");
                throw;
            }
           
        }

        [HttpGet]
        [Route("{taskNumber}")]
        public async Task<IActionResult> GetTaskByTaskNumber(String taskNumber) {
            _logger.LogInformation("Received request to fetch task by task number: {@EmployeeTask}", taskNumber);
            try
            {
                var task = await taskService.getTaskByTaskNumber(taskNumber);
                if (task is null)
                {
                    _logger.LogWarning("Task with task number {@EmployeeTask} not found.", taskNumber);
                    return NotFound("TaskNumber not found");

                }
                return Ok(task);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching task with task number: {@EmployeeTask}",taskNumber);
                throw;
            }

        }

        [HttpPatch]
        [Route("changestatus/{taskNumber}")]
        public async Task<IActionResult> UpdateTaskStatusByTaskNumber(string taskNumber,[FromBody] string status)
        {
            _logger.LogInformation("Received request to update status for task number: {@taskNumber} with status: {@status}", taskNumber,status);
            try
            {
                string status1 = status.Replace("\"", "");
                var task = await taskService.updateTaskStatusByTaskNumber(taskNumber, status1);
                if (task is null)
                {
                    _logger.LogWarning("Task with task number {@taskNumber} not found for status update.", taskNumber);
                    return NotFound("Task not found");

                }
                _logger.LogInformation("Successfully updated status for task number: {@taskNumber}.", taskNumber);
                return Ok(task);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating status for task number: {@taskNumber}", taskNumber);
                throw;
            }

        }

        [HttpPatch]
        [Route("{taskNumber}")]
        public async Task<IActionResult> UpdateTaskByTaskNumber(string taskNumber, UpdateTaskDto updateTaskDto)
        {
            _logger.LogInformation($"Received request to update task with task number: {taskNumber}");
            try
            {
                var task = await taskService.updateTaskByTaskNumber(taskNumber, updateTaskDto);
                if (task is null)
                {
                    _logger.LogWarning($"Task with task number {taskNumber} not found for update.");
                    return NotFound("Task not found");

                }
                _logger.LogInformation($"Successfully updated task with task number: {taskNumber}.");
                return Ok(task);
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"Error occurred while updating task with task number: {taskNumber}");
                throw;
            }

        }

        [HttpGet]
        [Route("newTaskNumber")]
        public async Task<IActionResult> GetNewTaskNumber()
        {
            _logger.LogInformation("Received request to get a new task number.");
            try
            {
                var newTaskNumber = await taskService.getNewTaskNumber();
                _logger.LogInformation($"Generated new task number: {newTaskNumber}");
                return Ok(newTaskNumber);

            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while fetching new task number.");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDto createTaskDto) {
            _logger.LogInformation($"Received request to create task with details: {createTaskDto}");
            try
            {
                 await taskService.createTask(createTaskDto);
                _logger.LogInformation($"Successfully created task with details: {createTaskDto}");
                return Ok("Created");
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"Error occurred while creating task with details: {createTaskDto}");
                throw;
            }

        }

        [HttpDelete]
        //[Route("{taskNumber}")]
        public async Task<IActionResult> DeleteTask(String taskNumber) {
            _logger.LogInformation($"Received request to delete task with task number: {taskNumber}");
            try
            {
                bool isDeleted = await taskService.deleteTaskByTaskNumber(taskNumber);
                if (isDeleted)
                {
                    _logger.LogInformation($"Successfully deleted task with task number: {taskNumber}");
                    return Ok("Deleted");
                }
                _logger.LogWarning($"Task with task number {taskNumber} not found for deletion.");
                return NotFound("Task not found");
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"Error occurred while deleting task with task number: {taskNumber}");
                throw;
            }


        }
       

    }
}
