using TaskManagerDapper.Dtos;
using TaskManagerDapper.Models;

namespace TaskManagerDapper.Services
{
    public interface ITaskLogTimeEntriesService
    {
        public Task createEntry(CreateLogEntryDto createLogEntryDto);
        public Task<IEnumerable<GetLogEntriesDto>> getLogEntriesByTaskNumber(String taskNumber);

        public Task<bool> deleteEntryByTaskNumber(String taskNumber);

        
    }
}
