using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagerDapper.Dtos
{
    public class CreateTaskDto
    {
        public string title {  get; set; }
        public string description { get; set; }
        public string priority { get; set; }
        public DateTime duedate { get; set; }
        public string assignedto { get; set; }
    }
}
