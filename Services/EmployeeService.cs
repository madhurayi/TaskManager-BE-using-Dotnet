using TaskManagerDapper.Models;
using TaskManagerDapper.Repositories;
using TaskManagerDapper.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskManager.Controllers;

namespace TaskManagerDapper.Services
{
    public class EmployeeService : IEmployeeService
    {
        public  IEmployeeRepository repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        public EmployeeService(IEmployeeRepository repository,IMapper mapper, ILogger<EmployeeService> logger) { 
            this.repository = repository;
            this._mapper = mapper;
            this._logger = logger;
        }

        public async Task<IEnumerable<Employee>> getAllEMployees()
        {
            _logger.LogInformation("Fetching all employee records.");
            try
            {
                var employees = await repository.FindAll();
                _logger.LogInformation("Successfully fetched {@Employee} employees.",employees);
                return employees;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while fetching employees.");
                throw;
            }
        }

        public async  Task<Employee> saveEmployee(CreateEmployeeDto createEmployeeDto)
        {
            _logger.LogInformation("Adding new employee with details: {@Employee}", createEmployeeDto);
            try
            {
                List<Employee> employees = (await repository.FindAll()).ToList();
                Employee newEmployee = _mapper.Map<Employee>(createEmployeeDto);
                if (employees.Count == 0)
                {
                    newEmployee.empId = "P-1000";
                    _logger.LogDebug("Creating the first employee with ID: {@Employee}", newEmployee.empId);
                }
                else
                {
                    _logger.LogDebug("Total number of employees: {@Employee}", employees.Count);
                    Employee emp = await repository.FindById(employees.Count);
                    String[] maxEmpIdArr = emp.empId.Split("-");
                    int num = Convert.ToInt32(maxEmpIdArr[maxEmpIdArr.Length - 1]) + 1;
                    String maxEmpID = "P-" + (num);
                    newEmployee.empId = maxEmpID;
                    _logger.LogDebug("Assigned new employee ID: {@Employee}", maxEmpID);
                }
                await repository.Save(newEmployee);
                _logger.LogInformation("Successfully created new employee with ID: {@Employee}", newEmployee.empId);
                return newEmployee;
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while saving employee data: {@Employee}", createEmployeeDto);
                throw;
            }
        }
    }
}
