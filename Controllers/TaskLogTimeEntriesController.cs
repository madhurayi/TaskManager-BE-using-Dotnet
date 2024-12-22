using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Controllers;
using TaskManagerDapper.Dtos;
using TaskManagerDapper.Services;

namespace TaskManagerDapper.Controllers
{
    [Route("api/v1/logentries")]
    [ApiController]
    public class TaskLogTimeEntriesController : ControllerBase
    {
        public readonly ITaskLogTimeEntriesService logTimeEntriesService;
        private readonly ILogger<EmployeeController> _logger;
        public TaskLogTimeEntriesController(ITaskLogTimeEntriesService logTimeEntriesService, ILogger<EmployeeController> logger)
        {
            this.logTimeEntriesService = logTimeEntriesService;
            this._logger = logger;
        }

        [HttpGet]
        [Route("{taskNumber}")]
        public async Task<IActionResult> GetAllEntriesByTaskNumber(string taskNumber) {
            _logger.LogInformation("Fetching log entries for TaskNumber: {@taskNumber}", taskNumber);
            try
            {
                var logEntries = await logTimeEntriesService.getLogEntriesByTaskNumber(taskNumber);
                if (logEntries is null)
                {
                    _logger.LogInformation("No log entries found for TaskNumber: {@taskNumber}", taskNumber);
                    return NotFound(("No log entries found for TaskNumber {@taskNumber}", taskNumber));
                }
                return Ok(logEntries);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while fetching log entries for TaskNumber: {@taskNumber}", taskNumber);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntry(CreateLogEntryDto createLogEntryDto)
        {
            _logger.LogInformation("Attempting to create log entry for TaskNumber: {@tasknumber}.", createLogEntryDto.taskNumber);
            try
            {
                await logTimeEntriesService.createEntry(createLogEntryDto);
                _logger.LogInformation("Successfully created log entry for TaskNumber: {@tasknumber}.", createLogEntryDto.taskNumber);
                return Ok("Created");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while creating log entry for TaskNumber: {@tasknumber}", createLogEntryDto.taskNumber);
                throw;
            }

        }
    }
}
