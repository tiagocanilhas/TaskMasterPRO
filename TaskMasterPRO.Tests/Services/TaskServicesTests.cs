using Moq;
using TaskMasterPRO.Model;
using TaskMasterPRO.Repository.Interfaces;
using TaskMasterPRO.Services;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Tests.Services
{
    public class TaskServicesTests
    {
        private readonly Mock<ITaskRepository> _repoMock;
        private readonly TaskDomain _domain;
        private readonly TaskServices _service;

        public TaskServicesTests()
        {
            _repoMock = new Mock<ITaskRepository>();
            _domain = new TaskDomain();
            _service = new TaskServices(_domain, _repoMock.Object);
        }


        /*
         * Create
         */

        [Fact]
        public async void CreateAsync_Success()
        {
            Category category = new()
            {
                Id = 1,
                Name = "Work",
                Description = "Tasks related to work",
                Color = "#FF5733"
            };

            string title = "Test Task";
            string description = "This is a test task.";
            DateTime deadline = DateTime.Now.AddDays(1);
            bool isCompleted = false;
            Priority priority = Priority.Medium;
            int categoryId = 1;

            Model.Task task = new()
            {
                Id = 1,
                CreationTime = DateTime.Now,
                Title = title,
                Description = description,
                Deadline = deadline,
                IsCompleted = isCompleted,
                Priority = priority,
                CategoryId = categoryId,
                Category = category
            };

            _repoMock.Setup(repo => repo.CreateAsync(It.IsAny<Model.Task>())).ReturnsAsync(task);

            var res = await _service.CreateAsync(title, description, deadline, isCompleted, priority, categoryId);
        }

        [Fact]
        public async void CreateAsync_Failure_BlankTitle()
        {
            string title = "";
            string description = "This is a test task.";
            DateTime deadline = DateTime.Now.AddDays(1);
            bool isCompleted = false;
            Priority priority = Priority.Medium;
            int categoryId = 1;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CreateAsync(title, description, deadline, isCompleted, priority, categoryId)
            );
        }

        [Fact]
        public async void CreateAsync_Failure_PastDeadline()
        {
            string title = "Test Task";
            string description = "This is a test task.";
            DateTime deadline = DateTime.Now.AddDays(-1);
            bool isCompleted = false;
            Priority priority = Priority.Medium;
            int categoryId = 1;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.CreateAsync(title, description, deadline, isCompleted, priority, categoryId)
            );
        }

        /*
         * Read
         */

        [Fact]
        public async void GetAllAsync_Success()
        {
            Category category = new()
            {
                Id = 1,
                Name = "Work",
                Description = "Tasks related to work",
                Color = "#FF5733"
            };

            List<Model.Task> tasks =
            [
                new Model.Task
                {
                    Id = 1,
                    CreationTime = DateTime.Now,
                    Title = "Test Task 1",
                    Description = "This is the first test task.",
                    Deadline = DateTime.Now.AddDays(1),
                    IsCompleted = false,
                    Priority = Priority.Medium,
                    CategoryId = 1,
                    Category = category
                },
                new Model.Task
                {
                    Id = 2,
                    CreationTime = DateTime.Now,
                    Title = "Test Task 2",
                    Description = "This is the second test task.",
                    Deadline = DateTime.Now.AddDays(2),
                    IsCompleted = false,
                    Priority = Priority.High,
                    CategoryId = 1,
                    Category = category
                }
            ];

            _repoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);

            var res = await _service.GetAllAsync();

            Assert.NotNull(res);
            Assert.Equal(2, res.Count);
        }

        [Fact]
        public async void GetByIdAsync_Success()
        {
            int id = 1;
            Category category = new()
            {
                Id = 1,
                Name = "Work",
                Description = "Tasks related to work",
                Color = "#FF5733"
            };

            Model.Task task = new()
            {
                Id = id,
                CreationTime = DateTime.Now,
                Title = "Test Task",
                Description = "This is a test task.",
                Deadline = DateTime.Now.AddDays(1),
                IsCompleted = false,
                Priority = Priority.Medium,
                CategoryId = 1,
                Category = category
            };

            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(task);

            var res = await _service.GetByIdAsync(id);

            Assert.NotNull(res);
            Assert.Equal(id, res.Id);
            Assert.Equal("Test Task", res.Title);
        }

        [Fact]
        public async void GetByIdAsync_Failure_NotFound()
        {
            int id = 1;
            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync((Model.Task)null!);
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.GetByIdAsync(id)
            );
        }



        /*
         * Update
         */

        [Fact]
        public async void UpdateAsync_Success()
        {
            Category category = new()
            {
                Id = 1,
                Name = "Work",
                Description = "Tasks related to work",
                Color = "#FF5733"
            };
            Category category2 = new()
            {
                Id = 2,
                Name = "Personal",
                Description = "Personal tasks",
                Color = "#33FF57"
            };


            int id = 1;
            Model.Task task = new()
            {
                Id = id,
                CreationTime = DateTime.Now,
                Title = "Test Task",
                Description = "This is a test task.",
                Deadline = DateTime.Now.AddDays(1),
                IsCompleted = false,
                Priority = Priority.Medium,
                CategoryId = 1,
                Category = category
            };

            string title = "Updated Task";
            string description = "Updated description";
            DateTime deadline = DateTime.Now.AddDays(2);
            bool isCompleted = true;
            Priority priority = Priority.High;
            int categoryId = 2;


            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(task);
            _repoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Model.Task>())).ReturnsAsync(task);
            var res = await _service.UpdateAsync(id, title, description, deadline, isCompleted, priority, categoryId);

            Assert.NotNull(res);
            Assert.Equal(id, res.Id);
            Assert.Equal(title, res.Title);
            Assert.Equal(description, res.Description);
            Assert.Equal(deadline, res.Deadline);
            Assert.Equal(priority, res.Priority);
            Assert.Equal(isCompleted, res.IsCompleted);
            Assert.Equal(categoryId, res.CategoryId);
        }

        [Fact]
        public async void UpdateAsync_Failure_NotFound()
        {
            int id = 1;
            string title = "Updated Task";
            string description = "Updated description";
            DateTime deadline = DateTime.Now.AddDays(2);
            bool isCompleted = true;
            Priority priority = Priority.High;
            int categoryId = 2;

            _repoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Model.Task>())).ReturnsAsync((Model.Task)null!);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.UpdateAsync(id, title, description, deadline, isCompleted, priority, categoryId)
            );
        }

        [Fact]
        public async void UpdateAsync_Failure_BlankTitle()
        {
            int id = 1;
            string title = "";
            string description = "Updated description";
            DateTime deadline = DateTime.Now.AddDays(2);
            bool isCompleted = true;
            Priority priority = Priority.High;
            int categoryId = 2;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.UpdateAsync(id, title, description, deadline, isCompleted, priority, categoryId)
            );
        }

        [Fact]
        public async void UpdateAsync_Failure_PastDeadline()
        {
            int id = 1;
            string title = "Updated Task";
            string description = "Updated description";
            DateTime deadline = DateTime.Now.AddDays(-1);
            bool isCompleted = true;
            Priority priority = Priority.High;
            int categoryId = 2;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service.UpdateAsync(id, title, description, deadline, isCompleted, priority, categoryId)
            );
        }



        /*
         * Delete
         */

        [Fact]
        public async void DeleteAsync_Success()
        {
            int id = 1;
            Model.Task task = new()
            {
                Id = id,
                CreationTime = DateTime.Now,
                Title = "Test Task",
                Description = "This is a test task.",
                Deadline = DateTime.Now.AddDays(1),
                IsCompleted = false,
                Priority = Priority.Medium,
                CategoryId = 1
            };

            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync(task);
            _repoMock.Setup(repo => repo.DeleteAsync(task)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(id);

            _repoMock.Verify(repo => repo.DeleteAsync(task), Times.Once);
        }

        [Fact]
        public async void DeleteAsync_Failure_NotFound()
        {
            int id = 1;
            _repoMock.Setup(repo => repo.GetAsync(id)).ReturnsAsync((Model.Task)null!);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _service.DeleteAsync(id)
            );
        } 
    }
}
