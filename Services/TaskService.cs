using TaskManagerDapper.Dtos;
using TaskManagerDapper.Models;
using TaskManagerDapper.Repositories;
using System;
using System.Runtime.InteropServices;
using AutoMapper;
using TaskManagerDapper.Constants;
using TaskManager.Controllers;
namespace TaskManagerDapper.Services
{
    public class TaskService : ITaskService
    {   
        public readonly ITaskRepository taskRepository;
        public readonly IEmployeeRepository employeeRepository;
        public readonly ITaskLogTimeEntriesService taskLogTimeEntriesService;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskService> _logger;
        public TaskService(ITaskRepository taskRepository,IEmployeeRepository employeeRepository,ITaskLogTimeEntriesService taskLogTimeEntriesService,IMapper mapper, ILogger<TaskService> logger) {
            this._mapper = mapper;
            this.taskRepository = taskRepository;
            this.employeeRepository = employeeRepository;
            this.taskLogTimeEntriesService = taskLogTimeEntriesService;
            this._logger = logger;
        }
        public async Task createTask(CreateTaskDto createTaskDto)
        {
            _logger.LogInformation("Starting to create task with task details: {@EmployeeTask}", createTaskDto);
            try
            {
                List<EmployeeTask> tasks = (await taskRepository.FindAll()).ToList();
                _logger.LogInformation("Successfully Fetched All Tasks");
                EmployeeTask newTask = _mapper.Map<EmployeeTask>(createTaskDto);
                newTask.status = "Yet To Start";
                Employee emp = await employeeRepository.FindByEmpId(createTaskDto.assignedto);
                if (emp is not null)
                {
                    newTask.employee = emp;
                    newTask.employeeid = emp.id;
                }
                if (tasks.Count == 0)
                {
                    newTask.taskNumber = "TASK-1";
                    _logger.LogInformation("This is the first task, assigned task number: {@TskNumber}", newTask.taskNumber);
                }
                else
                {
                    List<long> taskIds = new List<long>();
                    foreach (EmployeeTask task in tasks)
                    {
                        taskIds.Add(task.id);
                    }
                    long maxId = taskIds.Max();
                    EmployeeTask lastTask = await taskRepository.FindById(maxId);
                    String[] maxTaskIdArr = lastTask.taskNumber.Split("-");
                    int num = Convert.ToInt32(maxTaskIdArr[maxTaskIdArr.Length - 1]) + 1;
                    String maxTaskID = "TASK-" + (num);
                    newTask.taskNumber = maxTaskID;
                    _logger.LogInformation("Assigned task number-{@TASKId} based on existing tasks ", maxTaskID);
                }
                await taskRepository.Save(newTask);
                _logger.LogInformation("Successfully created task with task number: {@TaskNumber}", newTask.taskNumber);
                return;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while creating task with task details: {@EmployeeTask}", createTaskDto);
                throw;
            }
           
        }

        public async Task<bool> deleteTaskByTaskNumber(string taskNUmber)
        {
            _logger.LogInformation("Attempting to delete task with task number: {@taskNUmber}", taskNUmber);
            try
            {
                bool isLogEntryDeleted = await taskLogTimeEntriesService.deleteEntryByTaskNumber(taskNUmber);
                if (!isLogEntryDeleted)
                {
                    _logger.LogWarning("No log entries found for task number {@taskNUmber}", taskNUmber);
                }
                EmployeeTask task = await taskRepository.FindByTaskNumber(taskNUmber);
                if (task != null)
                {
                    await taskRepository.Delete(task);
                    _logger.LogInformation("Successfully deleted task with task number: {@taskNUmber}", taskNUmber);
                    return true;
                }
                _logger.LogWarning("Task with task number {@taskNUmber} not found for deletion", taskNUmber);
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task with task number: {@taskNUmber}", taskNUmber);
                throw;
            }
        }

        public async Task<IEnumerable<GetTasksDto>> getAllTasks()
        {
            _logger.LogInformation("Fetching the all tasks.");
            try
            {
                List<EmployeeTask> allTasks = (await taskRepository.FindAll()).ToList();
                _logger.LogInformation("Successfully fetched the all tasks.{tasks}",allTasks);
                List<GetTasksDto> tasksDto = new List<GetTasksDto>();
                foreach (EmployeeTask task in allTasks)
                {
                    GetTasksDto getTasksDto = _mapper.Map<GetTasksDto>(task);
                    Employee emp = await employeeRepository.FindById(task.employeeid);
                    if (emp is not null)
                    {
                        getTasksDto.empId = emp.empId;
                    }

                    tasksDto.Add(getTasksDto);
                    _logger.LogInformation("Mapping each task to employee task {@tasks}", getTasksDto);
                }

                return tasksDto;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all tasks");
                throw;
            }
        }

        public async Task<string> getNewTaskNumber()
        {
            _logger.LogInformation("Fetching the new task number.");
            try
            {
                List<EmployeeTask> allTasks = (await taskRepository.FindAll()).ToList();
                List<long> list = new List<long>();
                if (allTasks.Count == 0)
                {
                    _logger.LogInformation("No tasks found, assigning task number: TASK-1");
                    return "TASK-1";
                }
                foreach (EmployeeTask employeeTask in allTasks)
                {
                    list.Add(employeeTask.id);
                }
                long maxId = list.Max();
                EmployeeTask task = await taskRepository.FindById(maxId);

                String[] maxTaskIdArr = task.taskNumber.Split("-");
                int num = Convert.ToInt32(maxTaskIdArr[maxTaskIdArr.Length - 1]) + 1;
                String maxTaskID = "TASK-" + (num);
                _logger.LogInformation($"Assigned new task number: {maxTaskID}");
                return maxTaskID;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while fetching new task number.");
                throw;
            }
        }

        public async Task<GetTasksDto> getTaskByTaskNumber(string taskNumber)
        {
            _logger.LogInformation("Fetching all tasks with task number {@taskNumber}", taskNumber);
            try
            {
                EmployeeTask task = await taskRepository.FindByTaskNumber(taskNumber);
                if (task is null)
                {
                    _logger.LogWarning("Task with task number {@taskNumber} not found for update", taskNumber);
                    return null;
                }
                GetTasksDto getTasksDto = _mapper.Map<GetTasksDto>(task);
                Employee emp = await employeeRepository.FindById(task.employeeid);
                if (emp is not null)
                {
                    getTasksDto.empId = emp.empId;
                }
                return getTasksDto;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching tasks with task number {@taskNumber}", taskNumber);
                throw;
            }
        }

        public async Task<GetTasksDto> updateTaskByTaskNumber(string taskNumber, UpdateTaskDto updateTaskDto)
        {
            _logger.LogInformation("Starting to update task with task number {@taskNumber}", taskNumber);
            try
            {
                Employee emp = await employeeRepository.FindByEmpId(updateTaskDto.empId);
                EmployeeTask updateTask = _mapper.Map<EmployeeTask>(updateTaskDto);
                if (emp is not null) updateTask.employeeid = emp.id;
                updateTask.taskNumber = taskNumber;

                EmployeeTask task = await taskRepository.UpdateTaskByTaskNumber(updateTask);
                if (task is null)
                {
                    _logger.LogWarning("Task with task number {@taskNumber} not found for update", taskNumber);
                    return null;
                }
                _logger.LogInformation("Successfully updated task with task number {@taskNumber}", taskNumber);
                GetTasksDto getTask = _mapper.Map<GetTasksDto>(task);
                getTask.empId = updateTaskDto.empId;
                return getTask;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while updating task with task number {@taskNumber}", taskNumber);
                throw;
            }
        }

        public async Task<GetTasksDto> updateTaskStatusByTaskNumber(string taskNumber, string status)
        {
            _logger.LogInformation("Starting to update status with task number {@taskNumber}", taskNumber);
            try
            {
                EmployeeTask task = await taskRepository.UpdateStatusByTaskNumber(taskNumber, status);
                if (task is null)
                {
                    _logger.LogInformation("Employeetask is not present for the taskNumber {@taskNumber}", taskNumber);
                    return null;
                }
                _logger.LogInformation("Successfully updated status with task number {@taskNumber}", taskNumber);
                GetTasksDto getTasksDto = _mapper.Map<GetTasksDto>(task);
                Employee emp = await employeeRepository.FindById(task.employeeid);
                if (emp is not null)
                {
                    getTasksDto.empId = emp.empId;
                }
                return getTasksDto;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while updating status with task number {@taskNumber}", taskNumber);
                throw;
            }
        }
    }
}
