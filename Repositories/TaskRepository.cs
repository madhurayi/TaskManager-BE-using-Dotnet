
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using TaskManagerDapper.Constants;
using TaskManagerDapper.Dtos;
using TaskManagerDapper.Models;

namespace TaskManagerDapper.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        public readonly string _connectionString;
        public readonly IEmployeeRepository employeeRepository;
        public TaskRepository(IConfiguration configuration,IEmployeeRepository employeeRepository)
        {
            _connectionString = configuration.GetConnectionString("TaskManagerDb");
            this.employeeRepository = employeeRepository;
        }

        public async Task<EmployeeTask> Delete(EmployeeTask task)
        {
            var query = PostgresqlConstants.DeleteEmployeeTask;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(query, new { id =task.id });
            }
            return task;
        }

        public async  Task<IEnumerable<EmployeeTask>> FindAll()
        {
            var query = PostgresqlConstants.GetAllEmployeeTasks;
            using (var connection = new NpgsqlConnection(_connectionString)) { 
                var tasks=await connection.QueryAsync<EmployeeTask>(query);

                return tasks.ToList();
            }
            
            throw new NotImplementedException();
        }

        public async Task<EmployeeTask> FindById(long id)
        {
            var query = PostgresqlConstants.GetTaskByTaskId;
            using (var connection = new NpgsqlConnection(_connectionString)) { 
               return await connection.QuerySingleOrDefaultAsync<EmployeeTask>(query,new {id});
            }
            throw new NotImplementedException();
        }

        public async Task<EmployeeTask> FindByTaskNumber(string taskNumber)
        {
            var query = PostgresqlConstants.GetTaskByTaskNumber;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<EmployeeTask>(query, new { taskNumber});
            }
            throw new NotImplementedException();
        }

        public async Task<EmployeeTask> Save(EmployeeTask task)
        {
            var query = PostgresqlConstants.SaveEmployeeTask;
            object[] parameters = { new { taskNumber = task.taskNumber, title = task.title, priority = task.priority, taskDueDate=task.taskDueDate,employeeid=task.employeeid,description=task.description,status=task.status } };
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(query, parameters);
                return task;
            }
        }

        public async Task<EmployeeTask> UpdateStatusByTaskNumber(string taskNumber, string status)
        {
            var query = PostgresqlConstants.UpdateStatusByTaskNumber;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(query,new { status=status, taskNumber=taskNumber});
                return await FindByTaskNumber(taskNumber);
            }
        }

        public async Task<EmployeeTask> UpdateTaskByTaskNumber(EmployeeTask updateTask)
        {
            var query = PostgresqlConstants.UpdateTaskByTaskNumber;
            var parameters = new
            {
                title = updateTask.title,
                priority = updateTask.priority,
                taskDueDate = updateTask.taskDueDate,
                employeeid = updateTask.employeeid,
                taskNumber = updateTask.taskNumber
            };

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(query, parameters);
                return await FindByTaskNumber(updateTask.taskNumber);
            }
        }
    }
}
