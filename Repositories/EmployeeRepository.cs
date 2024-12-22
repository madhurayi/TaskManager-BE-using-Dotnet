using System.Data;
using Dapper;
using Npgsql;
using TaskManagerDapper.Constants;
using TaskManagerDapper.Models;

namespace TaskManagerDapper.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public readonly string _connectionString;
        
        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TaskManagerDb");
        }

        public async Task<IEnumerable<Employee>> FindAll()
        {
            var query = PostgresqlConstants.GetAllEmployees;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var employees= await connection.QueryAsync<Employee>(query);
                return employees.ToList();
            }

        }

        public async Task<Employee> FindByEmpId(string empId)
        {
            var query = PostgresqlConstants.GetByEmployeeId;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Employee>(query, new { empId });
            }
        }

        public async Task<Employee> FindById(long id)
        {
            var query = PostgresqlConstants.GetByEmployeeKey;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Employee>(query, new { id });
            }
        }

        public async Task<Employee> Save(Employee employee)
        {
            var query = PostgresqlConstants.SaveEmployee;

            object[] parameters = { new { empName=employee.empName, empId=employee.empId } };

            using (var connection = new NpgsqlConnection(_connectionString))
            {

                //await connection.ExecuteAsync(query, parameters);
                await connection.ExecuteAsync(query, parameters);
                return employee;
            }
        }
    }
}
