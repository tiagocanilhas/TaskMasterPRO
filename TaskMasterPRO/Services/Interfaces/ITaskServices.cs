using TaskMasterPRO.Model;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Services.Interfaces
{
    public interface ITaskServices
    {
        Task<Model.Task> CreateAsync(
            string title,
            string description,
            DateTime deadline,
            bool isCompleted,
            Priority priority,
            int categoryId
            );

        Task<List<Model.Task>> GetAllAsync();

        Task<Model.Task> GetByIdAsync(int id);

        Task<Model.Task> UpdateAsync(
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