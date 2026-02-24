using TaskMasterPRO.Model;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Repository.Interfaces
{
    public interface ITaskRepository
    {
        Task<Model.Task> CreateAsync(Model.Task task);

        Task<List<Model.Task>> GetAllAsync();

        Task<Model.Task?> GetAsync(int id);

        Task<Model.Task> UpdateAsync(Model.Task task);

        Task DeleteAsync(Model.Task task);
    }
}
