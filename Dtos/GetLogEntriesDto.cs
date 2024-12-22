using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.InteropServices;

namespace TaskManagerDapper.Dtos
{
    public class GetLogEntriesDto
    {
        public long id {  get; set; }
        public string taskNumber { get; set; }
        public DateTime logDate {  get; set; }
        public string logTime { get; set; }
        public string logNote { get; set; }
        public string logUser { get; set; }
    }
}
