using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMasterPRO.Data.Domain;
using TaskMasterPRO.Data.Repository.Interfaces;
using TaskMasterPRO.Data.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Data.Services
{
    public class TaskServices(
           TaskDomain taskDomain,
           ITaskRepository repository
        ) : ITaskServices
    {
        public async Task<Domain.Task> CreateAsync(
            string title,
            string description,
            DateTime deadline,
            bool isCompleted,
            Priority priority,
            int categoryId
            )
        {
            if (!taskDomain.IsTitleValid(title)) throw new ArgumentException("Title is not valid.");
            if (!taskDomain.IsDeadlineValid(deadline)) throw new ArgumentException("Deadline is not valid.");

            Domain.Task task = new()
            {
                Title = title,
                Description = description,
                Deadline = deadline,
                IsCompleted = isCompleted,
                Priority = priority,
                CategoryId = categoryId
            };

            return await repository.CreateAsync(task);
        }

        public async Task<List<Domain.Task>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Domain.Task> GetByIdAsync(int id)
        {
            return await repository.GetAsync(id) ?? throw new KeyNotFoundException($"Task with id {id} not found.");
        }

        public async Task<Domain.Task> UpdateAsync(
            int id, 
            string title, 
            string description,
            DateTime deadline,
            bool isCompleted,
            Priority priority, 
            int categoryId
            )
        {
            if (!taskDomain.IsTitleValid(title)) throw new ArgumentException("Title is not valid.");
            if (!taskDomain.IsDeadlineValid(deadline)) throw new ArgumentException("Deadline is not valid.");

            Domain.Task task = await repository.GetAsync(id) ?? throw new KeyNotFoundException($"Task with id {id} not found.");

            task.Title = title;
            task.Description = description;
            task.Deadline = deadline;
            task.IsCompleted = isCompleted;
            task.Priority = priority;
            task.CategoryId = categoryId;

            return await repository.UpdateAsync(task);
        }

        public async Task DeleteAsync(int id)
        {
            Domain.Task task = await repository.GetAsync(id) ?? throw new KeyNotFoundException($"Task with id {id} not found.");

            await repository.DeleteAsync(task);
        }
    }
}
