using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;

namespace TaskManagerDapper.Dtos
{
    public class GetTasksDto
    {
        public long id {  get; set; }
        public string tasknumber { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string priority { get; set; }
        public DateTime duedate { get; set; }
        public string empId { get; set; }
    }
}
