using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagerDapper.Models
{
    public class TaskLogTimeEntries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [ForeignKey("employeetaskid")]
        public long employeetaskid { get; set; }
        public EmployeeTask task { get; set; }
        public DateTime logDate {  get; set; }
        public string logNote { get; set; }
        public string logTime { get; set; }

        [ForeignKey("employeeid")]
        public long employeeid { get; set; }
        public Employee logEntryUser { get; set; }
    }
}
