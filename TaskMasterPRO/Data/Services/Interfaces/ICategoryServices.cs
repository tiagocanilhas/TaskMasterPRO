using TaskMasterPRO.Data.Domain;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Data.Services.Interfaces
{
    public interface ICategoryServices
    {
        Task<Category> CreateAsync(
            string name,
            string description,
            string color
            );

        Task<List<Category>> GetAllAsync();

        Task<Category> GetByIdAsync(int id);

        Task<Category> UpdateAsync(
            int id,
            string name,
            string description,
            string color
            );

        Task DeleteAsync(int id);
    }
}