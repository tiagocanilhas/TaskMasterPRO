using TaskMasterPRO.Model;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);

        Task<List<Category>> GetAllAsync();

        Task<Category?> GetAsync(int id);

        Task<Category> UpdateAsync(Category category);

        Task DeleteAsync(Category category);
    }
}
