using TaskManagerDapper.Models;

namespace TaskManagerDapper.Repositories
{
    public interface ITaskLogTimeEntriesRepository
    {
        public Task<TaskLogTimeEntries> Save(TaskLogTimeEntries taskLogTimeEntries);

        public Task<IEnumerable<TaskLogTimeEntries>> FindAllByTaskNumber(String taskNumber);

        public Task<bool> DeleteByTaskNumber(long taskId);

        public Task<IEnumerable<TaskLogTimeEntries>> FindAll();
    }
}
