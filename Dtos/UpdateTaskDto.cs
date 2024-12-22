using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagerDapper.Dtos
{
    public class UpdateTaskDto
    {
        public string title { get; set; }
        public DateTime duedate { get; set; }
        public string empId { get; set; }
        public string priority { get; set; }
    }
}
