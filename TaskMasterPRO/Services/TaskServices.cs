using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMasterPRO.Model;
using TaskMasterPRO.Repository.Interfaces;
using TaskMasterPRO.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Services
{
    public class TaskServices(
           TaskDomain taskDomain,
           ITaskRepository repository
        ) : ITaskServices
    {
        public async Task<Model.Task> CreateAsync(
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

            Model.Task task = new()
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

        public async Task<List<Model.Task>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Model.Task> GetByIdAsync(int id)
        {
            return await repository.GetAsync(id) ?? throw new KeyNotFoundException($"Task with id {id} not found.");
        }

        public async Task<Model.Task> UpdateAsync(
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

            Model.Task task = await repository.GetAsync(id) ?? throw new KeyNotFoundException($"Task with id {id} not found.");

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
            Model.Task task = await repository.GetAsync(id) ?? throw new KeyNotFoundException($"Task with id {id} not found.");

            await repository.DeleteAsync(task);
        }
    }
}
