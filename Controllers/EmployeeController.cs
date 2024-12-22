using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerDapper.Controllers;
using TaskManagerDapper.Dtos;
using TaskManagerDapper.Models;
using TaskManagerDapper.Services;

namespace TaskManager.Controllers
{
    [Route("api/v1/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;

        public readonly IEmployeeService employeeService;
        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger) { 
            _logger = logger;
            _logger.LogTrace("Employee Controller started");
            _logger.LogInformation("Employee Controller info started");
            this.employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees() {
            _logger.LogInformation("Received request to fetch all employees.");
            try
            {
                var employees = await employeeService.getAllEMployees();
                if (employees.ToList().Count == 0)
                {
                    _logger.LogInformation("No employees found in the Employee table.");
                    return NotFound("No employees found in the Employee table.");
                }
                _logger.LogInformation("Successfully fetched employee records.");
                return Ok(employees);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while fetching employees.");
                throw;
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto createEmployeeDto) {
            _logger.LogInformation("Received request to create employee with details: {@Employee}",createEmployeeDto);
            if (createEmployeeDto == null || (createEmployeeDto.empName).Trim() == "")
            {
                _logger.LogError("Employee creation failed. Invalid employee details provided.");
                return BadRequest("Employee details are invalid. Please provide valid data.");
            }
            try
            {
                var emp = await employeeService.saveEmployee(createEmployeeDto);
                _logger.LogInformation("Successfully created employee with ID: {@Employee}.", emp.empId);
                return Ok("Created");
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while creating the employee.");
                throw;
            }
            
        }
    }
}
