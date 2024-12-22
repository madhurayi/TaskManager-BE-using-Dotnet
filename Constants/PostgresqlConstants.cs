namespace TaskManagerDapper.Constants
{
    public interface PostgresqlConstants
    {
        public const string GetAllEmployees = "SELECT * FROM Employee";

        public const string GetByEmployeeId = "SELECT * FROM Employee WHERE \"empId\" = @empId";

        public const string GetByEmployeeKey="SELECT * FROM Employee WHERE id = @id";

        public const string SaveEmployee = "INSERT INTO Employee(\"empName\",\"empId\") VALUES(@empName,@empId)";

        public const string DeleteEmployeeTask="DELETE FROM EmployeeTask where id=@id";

        public const string GetAllEmployeeTasks = "SELECT * FROM EmployeeTask";

        public const string GetTaskByTaskId="SELECT * FROM EmployeeTask WHERE id=@id";

        public const string GetTaskByTaskNumber= "SELECT * FROM EmployeeTask WHERE \"taskNumber\"=@taskNumber";

        public const string SaveEmployeeTask= "INSERT INTO EmployeeTask(\"taskNumber\",title,description,status,priority,\"taskDueDate\",employeeid) " +
                                               "VALUES(@taskNumber,@title,@description,@status,@priority,@taskDueDate,@employeeid)";

        public const string UpdateStatusByTaskNumber= "UPDATE EmployeeTask SET status=@status where \"taskNumber\" =@taskNumber";

        public const string UpdateTaskByTaskNumber= "UPDATE EmployeeTask " +
                                                    "SET title= @title," +
                                                    "priority= @priority," +
                                                    "\"taskDueDate\"= @taskDueDate," +
                                                    "employeeid= @employeeid " +
                                                    "where \"taskNumber\" = @taskNumber";

        public const string DeleteLogEntriesByTaskNumber = "DELETE FROM \"taskLogTimeEntries\" WHERE employeetaskid=@id";

        public const string GetAllLogEntries = "SELECT * FROM \"taskLogTimeEntries\"";

        public const string SaveLogEntry = "INSERT INTO \"taskLogTimeEntries\" (employeetaskid,\"logDate\",\"logNote\",\"logTime\",employeeid) " +
                                            "VALUES(@employeetaskid,@logDate,@logNote,@logTime,@employeeid)";

        public const string GetLogEntriesByEmployeeTaskId = "SELECT * FROM \"taskLogTimeEntries\" where employeetaskid=@id";
    }
}
