using Microsoft.EntityFrameworkCore;
using TaskMasterPRO.Data.Database;
using TaskMasterPRO.Data.Repository.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Data.Repository
{
    public class TaskRepository(
        TaskMasterPROContext context
        ) : ITaskRepository
    {
        public async Task<Domain.Task> CreateAsync(Domain.Task task)
        {
            context.Task.Add(task);
            await context.SaveChangesAsync();

            return task;
        }

        public async Task<List<Domain.Task>> GetAllAsync()
        {
            return await context.Task
                .ToListAsync();
        }

        public async Task<Domain.Task?> GetAsync(int id)
        {
            return await context.Task
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Domain.Task> UpdateAsync(Domain.Task task)
        {
            context.Task.Update(task);
            await context.SaveChangesAsync();

            return task;
        }

        public async Task DeleteAsync(Domain.Task task)
        {
            context.Task.Remove(task);
            await context.SaveChangesAsync();
        }
    }
}
