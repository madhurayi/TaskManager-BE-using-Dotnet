using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace TaskManagerDapper.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public String empName { get; set; }
        public String empId { get; set; }
        
        public List<EmployeeTask> employeeTasks { get; set; }
        //public virtual ICollection<EmployeeTask> EmployeeTasks { get; set; }

        public List<TaskLogTimeEntries> employeeLogEntries { get; set; }

    }
}
