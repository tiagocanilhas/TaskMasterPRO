using Microsoft.EntityFrameworkCore;
using TaskMasterPRO.Data.Database;
using TaskMasterPRO.Data.Domain;
using TaskMasterPRO.Data.Repository.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Data.Repository
{
    public class CategoryRepository(
        TaskMasterPROContext context
        ) : ICategoryRepository
    {
        public async Task<Category> CreateAsync(Category category)
        {
            await context.Category.AddAsync(category);
            await context.SaveChangesAsync();

            return category;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await context.Category
                .ToListAsync();
        }

        public async Task<Category?> GetAsync(int id)
        {
            return await context.Category
                .FirstOrDefaultAsync(cat => cat.Id == id);
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            context.Category.Update(category);
            await context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteAsync(Category category)
        {
            context.Category.Remove(category);
            await context.SaveChangesAsync();
        }
    }
}
