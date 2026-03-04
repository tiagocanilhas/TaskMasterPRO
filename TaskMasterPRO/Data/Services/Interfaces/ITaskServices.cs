using TaskMasterPRO.Data.Domain;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Data.Services.Interfaces
{
    public interface ITaskServices
    {
        Task<Domain.Task> CreateAsync(
            string title,
            string description,
            DateTime deadline,
            bool isCompleted,
            Priority priority,
            int categoryId
            );

        Task<List<Domain.Task>> GetAllAsync();

        Task<Domain.Task> GetByIdAsync(int id);

        Task<Domain.Task> UpdateAsync(
            int id,
            string title,
            string description,
            DateTime deadline,
            bool isCompleted,
            Priority priority,
            int categoryId
            );

        Task DeleteAsync(int id);
    }
}