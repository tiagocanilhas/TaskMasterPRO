using Microsoft.EntityFrameworkCore;
using TaskMasterPRO.Data;
using TaskMasterPRO.Repository.Interfaces;

namespace TaskMasterPRO.Repository
{
    public class TaskRepository(
        TaskMasterPROContext context
        ) : ITaskRepository
    {
        public async Task<Model.Task> CreateAsync(Model.Task task)
        {
            context.Task.Add(task);
            await context.SaveChangesAsync();

            return task;
        }

        public async Task<List<Model.Task>> GetAllAsync()
        {
            return await context.Task
                .ToListAsync();
        }

        public async Task<Model.Task?> GetAsync(int id)
        {
            return await context.Task
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Model.Task> UpdateAsync(Model.Task task)
        {
            context.Task.Update(task);
            await context.SaveChangesAsync();

            return task;
        }

        public async Task DeleteAsync(Model.Task task)
        {
            context.Task.Remove(task);
            await context.SaveChangesAsync();
        }
    }
}
