using Microsoft.EntityFrameworkCore;
using TaskMasterPRO.Data;
using TaskMasterPRO.Model;
using TaskMasterPRO.Repository;
using Task = TaskMasterPRO.Model.Task;

namespace TaskMasterPRO.Tests.Repository
{
    public class TaskRepositoryTests
    {

        private readonly TaskMasterPROContext _context;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TaskMasterPROContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new(options);
            _repository = new(_context);


            PopulateDatabase();
        }


        /*  Category Table:
         *  
         *  ====================================================
         *  | Id  | Name     | Description     | Color         |
         *  |-----|----------|-----------------|---------------|
         *  |  1  | "Name"   | "Description"   | "#FFFFFF"     |
         *  ====================================================
         */

        /*  Task Table:
         *  
         *  ==================================================================================================================
         *  | Id  | CreationTime | Title          | Description          | Deadline    | IsCompleted | Priority | CategoryId |
         *  |-----|--------------|----------------|----------------------|-------------|-------------|----------|------------|
         *  |  1  | ...          | "Title"        | "Description"        |...          | false       | low      | 1          |
         *  |-----|--------------|----------------|----------------------|-------------|-------------|----------|------------|
         *  |  2  | ...          | "Another Title"| "Another Description"|...          | false       | high     | 1          |
         *  ==================================================================================================================
         */

        private void PopulateDatabase()
        {
            _context.Category.Add(new()
            {
                Name = "Name",
                Description = "Description",
                Color = "#FFFFFF"
            });

            _context.Task.AddRange(
                new()
                {
                    Title = "Title",
                    Description = "Description",
                    Deadline = DateTime.Now.AddDays(7),
                    IsCompleted = false,
                    Priority = Priority.Low,
                    CategoryId = 1
                },
                new()
                {
                    Title = "Another Title",
                    Description = "Another Description",
                    Deadline = DateTime.Now.AddDays(14),
                    IsCompleted = false,
                    Priority = Priority.High,
                    CategoryId = 1
                }
            );
            _context.SaveChangesAsync();
        }




        /*
         * Create
         */
        [Fact]
        public async void CreateTest_AddTask()
        {
            Task task = new()
            {
                Title = "New Task",
                Description = "New Description",
                Deadline = DateTime.Now.AddDays(30),
                IsCompleted = false,
                Priority = Priority.Medium,
                CategoryId = 1
            };
            Task res = await _repository.CreateAsync(task);

            Assert.NotNull(res);
            Assert.True(res.Id > 0);
        }



        /*
         * Read 
         */

        [Fact]
        public async void GetTest_GetAllTasks()
        {
            var res = await _repository.GetAllAsync();

            Assert.Equal(2, res.Count);
        }

        [Fact]
        public async void GetTest_GetTask()
        {
            int id = 1;
            var res = await _repository.GetAsync(id);

            Assert.NotNull(res);
            Assert.Equal(id, res.Id);
        }

        [Fact]
        public async void GetTest_FailedToGetTask()
        {
            int id = -1;
            var res = await _repository.GetAsync(id);

            Assert.Null(res);
        }



        /*
         * Update
         */

        [Fact]
        public async void UpdateTest_UpdateTask()
        {
            int id = 1;
            Task task = await _repository.GetAsync(id) ?? throw new Exception("Task not found");
            task.Title = "Updated Task";
            task.Priority = Priority.Medium;

            var res = await _repository.UpdateAsync(task);

            Assert.Equal("Updated Task", res.Title);
            Assert.Equal(Priority.Medium, res.Priority);
        }



        /*
         * Delete
         */

        [Fact]
        public async void DeleteTest_DeleteTask()
        {
            int id = 1;
            Task task = await _repository.GetAsync(id) ?? throw new Exception("Task not found");

            await _repository.DeleteAsync(task);
            Task? checkTask = await _repository.GetAsync(id);
            Assert.Null(checkTask);
        }
    }
}