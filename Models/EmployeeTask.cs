using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagerDapper.Models
{
    public class EmployeeTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string taskNumber { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string priority { get; set; }
        public DateTime taskDueDate { get; set; }

        [ForeignKey("employeeid")]
        public long employeeid { get; set; }
        public virtual Employee employee { get; set; }

        public List<TaskLogTimeEntries> logEntries { get; set; }
        public override string ToString()
        {
            // If employee is not null, print the employee's name or ID (you can adjust as per your needs)
            string employeeInfo = employee != null ? $"Employee: {employee.id}" : "Employee: Not Assigned";

            // Return a string that includes relevant information about the task
            return $"Task ID: {id}, Task Number: {taskNumber}, Title: {title}, Status: {status}, Priority: {priority}, Due Date: {taskDueDate}, {employeeInfo}";
        }
    }
}
