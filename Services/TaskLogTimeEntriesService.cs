using System.Collections;
using AutoMapper;
using TaskManager.Controllers;
using TaskManagerDapper.Dtos;
using TaskManagerDapper.Models;
using TaskManagerDapper.Repositories;

namespace TaskManagerDapper.Services
{
    public class TaskLogTimeEntriesService : ITaskLogTimeEntriesService
    {
        public readonly ITaskLogTimeEntriesRepository taskLogTimeEntriesRepository;
        public readonly ITaskRepository taskRepository;
        public readonly IEmployeeRepository employeeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;
        public TaskLogTimeEntriesService(ITaskLogTimeEntriesRepository taskLogTimeEntriesRepository , ITaskRepository taskRepository, IEmployeeRepository employeeRepository,IMapper mapper, ILogger<EmployeeController> logger)
        {
            this.taskRepository = taskRepository;
            this.employeeRepository = employeeRepository;
            this.taskLogTimeEntriesRepository = taskLogTimeEntriesRepository;
            this._mapper = mapper;
            this._logger = logger;
        }
        public async Task createEntry(CreateLogEntryDto createLogEntryDto)
        {
            _logger.LogInformation("Attempting to create log entry for TaskNumber: {@taskNumber} by Employee: {@logEntryUserId}", createLogEntryDto.taskNumber, createLogEntryDto.logEntryUserId);
            try
            {
                EmployeeTask task = await taskRepository.FindByTaskNumber(createLogEntryDto.taskNumber);
                if (task is null)
                {
                    _logger.LogError("TaskNumber { @taskNumber} not found. Cannot create log entry.", createLogEntryDto.taskNumber);
                    return;
                }
                Employee emp = await employeeRepository.FindByEmpId(createLogEntryDto.logEntryUserId);
                TaskLogTimeEntries newEntry = _mapper.Map<TaskLogTimeEntries>(createLogEntryDto);
                if (emp is not null)
                {
                    newEntry.employeeid = emp.id;
                }
                else
                {
                    _logger.LogWarning("Employee with ID {@logEntryUserId} not found for TaskNumber {@taskNumber}. Log entry created without employee association.", createLogEntryDto.logEntryUserId, createLogEntryDto.taskNumber);
                }
                newEntry.employeetaskid = task.id;
                await taskLogTimeEntriesRepository.Save(newEntry);
                _logger.LogInformation("Log entry for TaskNumber {@taskNumber} successfully created.", createLogEntryDto.taskNumber);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while creating long entry with entry details: {@createLogEntryDto}", createLogEntryDto);
                throw;
            }
        }

        public async Task<bool> deleteEntryByTaskNumber(string taskNumber)
        {
            _logger.LogInformation("Attempting to delete log entries for TaskNumber: {@taskNumber}", taskNumber);
            try
            {
                EmployeeTask task = await taskRepository.FindByTaskNumber(taskNumber);
                if (task is null)
                {
                    _logger.LogInformation("TaskNumber {@taskNumber} not found. No log entries to delete.", taskNumber);
                    return false;
                }
                long taskId = task.id;
                await taskLogTimeEntriesRepository.DeleteByTaskNumber(taskId);
                _logger.LogInformation("Successfully deleted log entries for TaskNumber {@taskNumber}.", taskNumber);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting long entry with task number: {@taskNumber}", taskNumber);
                throw;
            }
        }


        public async Task<IEnumerable<GetLogEntriesDto>> getLogEntriesByTaskNumber(string taskNumber)
        {
            _logger.LogInformation("Fetching log entries for TaskNumber: {@taskNumber}", taskNumber);

            try
            {
                List<TaskLogTimeEntries> logEntries = (await taskLogTimeEntriesRepository.FindAllByTaskNumber(taskNumber)).ToList();
                if (logEntries is null)
                {
                    _logger.LogInformation("No log entries found for TaskNumber {@taskNumber}.", taskNumber);
                    return null;
                }
                EmployeeTask task = await taskRepository.FindByTaskNumber(taskNumber);
                Employee employee = await employeeRepository.FindById(task.employeeid);
                String emp = employee.empId;
                List<GetLogEntriesDto> logEntriesDto = new List<GetLogEntriesDto>();
                foreach (TaskLogTimeEntries entry in logEntries)
                {
                    GetLogEntriesDto entryDto = _mapper.Map<GetLogEntriesDto>(entry);
                    entryDto.taskNumber = taskNumber;
                    entryDto.logUser = emp;
                    logEntriesDto.Add(entryDto);
                }
                _logger.LogInformation("Successfully fetched {@Count} log entries for TaskNumber {@taskNumber}.", logEntriesDto.Count, taskNumber);
                return logEntriesDto;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while getting long entries with task number: {@taskNumber}", taskNumber);
                throw;
            }
        }
    }
}
