using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagerDapper.Dtos
{
    public class CreateLogEntryDto
    {
        public DateTime logDate {  get; set; }
        public string logNote { get; set; }
        public string logEntryUserId { get; set; }
        public string logTime { get; set; }
        public string taskNumber  { get; set; }
    }
}
