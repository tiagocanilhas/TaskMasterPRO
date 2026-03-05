using TaskMasterPRO.Data.Domain;
using TaskMasterPRO.Data.Repository.Interfaces;
using TaskMasterPRO.Data.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Data.Services
{
    public class CategoryServices(
        CategoryDomain categoryDomain,
        ICategoryRepository categoryRepository
        ) : ICategoryServices
    {
        public async Task<Category> CreateAsync(
            string name,
            string description,
            string color
            )
        {
            if (!categoryDomain.IsNameValid(name)) throw new ArgumentException("Invalid category name.");
            if (!categoryDomain.IsDescriptionValid(description)) throw new ArgumentException("Invalid category description.");
            if (!categoryDomain.IsColorValid(color)) throw new ArgumentException("Invalid category color.");

            Category category = new()
            {
                Name = name,
                Description = description,
                Color = color
            };

            return await categoryRepository.CreateAsync(category);

        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await categoryRepository.GetAsync(id) ?? throw new KeyNotFoundException($"Category with {id} not found.");
        }

        public async Task<Category> UpdateAsync(
            int id,
            string name,
            string description,
            string color
            )
        {
            Category category = await categoryRepository.GetAsync(id) ?? throw new KeyNotFoundException($"Category with {id} not found.");

            if (!categoryDomain.IsNameValid(name)) throw new ArgumentException("Invalid category name.");
            if (!categoryDomain.IsColorValid(color)) throw new ArgumentException("Invalid category color.");

            category.Name = name;
            category.Description = description;
            category.Color = color;

            return await categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            Category category = await categoryRepository.GetAsync(id) ?? throw new KeyNotFoundException($"Category with {id} not found.");

            await categoryRepository.DeleteAsync(category);
        }
    }
}
