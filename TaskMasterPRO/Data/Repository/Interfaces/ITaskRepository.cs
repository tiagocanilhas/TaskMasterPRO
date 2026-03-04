using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Data.Repository.Interfaces
{
    public interface ITaskRepository
    {
        Task<Domain.Task> CreateAsync(Domain.Task task);

        Task<List<Domain.Task>> GetAllAsync();

        Task<Domain.Task?> GetAsync(int id);

        Task<Domain.Task> UpdateAsync(Domain.Task task);

        Task DeleteAsync(Domain.Task task);
    }
}
