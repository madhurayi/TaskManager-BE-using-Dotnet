using Dapper;
using Npgsql;
using System.Collections;
using System.Threading.Tasks;
using TaskManagerDapper.Constants;
using TaskManagerDapper.Models;

namespace TaskManagerDapper.Repositories
{
    public class TaskLogTimeEntriesRepository : ITaskLogTimeEntriesRepository
    {
        public readonly ITaskRepository taskRepository;
        public readonly string _connectionString;
        public TaskLogTimeEntriesRepository(ITaskRepository taskRepository, IConfiguration configuration)
        {
               this.taskRepository = taskRepository;
               _connectionString = configuration.GetConnectionString("TaskManagerDb");
        }

        public async Task<bool> DeleteByTaskNumber(long taskId )
        {
            var query = PostgresqlConstants.DeleteLogEntriesByTaskNumber;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(query, new { id = taskId });
                return true;
            }
            
        }

        public async Task<IEnumerable<TaskLogTimeEntries>> FindAll()
        {
            var query = PostgresqlConstants.GetAllLogEntries;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var entries=await connection.QueryAsync<TaskLogTimeEntries>(query);
                return entries.ToList();
            }

        }

        public async Task<IEnumerable<TaskLogTimeEntries>> FindAllByTaskNumber(string taskNumber)
        {
            var query = PostgresqlConstants.GetLogEntriesByEmployeeTaskId;
            EmployeeTask task= await taskRepository.FindByTaskNumber(taskNumber);
            long taskId = task.id;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var entries = await connection.QueryAsync<TaskLogTimeEntries>(query,new {id=taskId});
                return entries.ToList();
            }
        }

        public async Task<TaskLogTimeEntries> Save(TaskLogTimeEntries taskLogTimeEntries)
        {
            var query = PostgresqlConstants.SaveLogEntry;
            Object[] parameters = { new {employeetaskid=taskLogTimeEntries.employeetaskid, logDate=taskLogTimeEntries.logDate,logNote=taskLogTimeEntries.logNote,logTime=taskLogTimeEntries.logTime,employeeid=taskLogTimeEntries.employeeid} };
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(query, parameters);
            }
            return taskLogTimeEntries;

        }
    }
}
