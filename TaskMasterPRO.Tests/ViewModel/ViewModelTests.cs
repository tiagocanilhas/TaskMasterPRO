using Moq;
using TaskMasterPRO.Model;
using TaskMasterPRO.Services.Interfaces;
using TaskMasterPRO.ViewModel;

namespace TaskMasterPRO.Tests.ViewModel
{
    public class ViewModelTests
    {

        private readonly Mock<ICategoryServices> _categoryServicesMock;
        private readonly Mock<ITaskServices> _taskServicesMock;
        private readonly MainViewModel _viewmodel;

        public ViewModelTests()
        {
            _categoryServicesMock = new Mock<ICategoryServices>();
            _taskServicesMock = new Mock<ITaskServices>();
            _viewmodel = new MainViewModel(_categoryServicesMock.Object, _taskServicesMock.Object);
        }

        /*
         * Tasks loaded
         */

        [Fact]
        public async void LoadTasks_LoadsTasks_WhenExecuted()
        {
            List<Model.Task> tasks = new()
            {
                new Model.Task
                {
                    Id = 1,
                    CreationTime = DateTime.Now,
                    Title = "Task 1",
                    Description = "Description 1",
                    Deadline = DateTime.Now.AddDays(7),
                    IsCompleted = false,
                    Priority = Priority.Medium,
                    CategoryId = 1
                },
                new Model.Task
                {
                    Id = 2,
                    CreationTime = DateTime.Now,
                    Title = "Task 2",
                    Description = "Description 2",
                    Deadline = DateTime.Now.AddDays(14),
                    IsCompleted = false,
                    Priority = Priority.High,
                    CategoryId = 1
                }
            };
            _taskServicesMock.Setup(ser => ser.GetAllAsync()).ReturnsAsync(tasks);

            await _viewmodel.LoadTasksAsync();

            Assert.Equal(tasks.Count, _viewmodel.Tasks.Count);
        }



        /*
         * Add Task Command
         */

        [Fact]
        public void AddTaskCommand_AddsTask_WhenExecuted()
        {
            Model.Task task = new()
            {
                Id = 1,
                CreationTime = DateTime.Now,
                Title = "New Task",
                Description = "Task Description",
                Deadline = DateTime.Now.AddDays(7),
                IsCompleted = false,
                Priority = Priority.Medium,
                CategoryId = 1
            };

            int oldCount = _viewmodel.Tasks.Count;

            _taskServicesMock.Setup(ser => ser.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<bool>(),
                It.IsAny<Priority>(),
                It.IsAny<int>()
            )).ReturnsAsync(task);

            _viewmodel.AddTaskCommand.Execute(null);

            int newCount = _viewmodel.Tasks.Count;

            Assert.True(oldCount < newCount);
        }

        [Fact]
        public void AddTaskCommand_TaskToAddIsReset_WhenExecuted()
        {
            Model.Task task = new()
            {
                Id = 1,
                CreationTime = DateTime.Now,
                Title = "New Task",
                Description = "Task Description",
                Deadline = DateTime.Now.AddDays(7),
                IsCompleted = false,
                Priority = Priority.Medium,
                CategoryId = 1
            };
            _taskServicesMock.Setup(ser => ser.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<bool>(),
                It.IsAny<Priority>(),
                It.IsAny<int>()
            )).ReturnsAsync(task);

            _viewmodel.TaskToAdd.Title = "New Task";
            _viewmodel.AddTaskCommand.Execute(null);

            Assert.True(string.IsNullOrEmpty(_viewmodel.TaskToAdd.Title));
        }

        [Fact]
        public void CanAddTask_ReturnsTrue_WhenRequiredFieldsAreFilled()
        {
            _viewmodel.TaskToAdd.Title = "New Task";
            _viewmodel.TaskToAdd.Deadline = DateTime.Now.AddDays(7);
            _viewmodel.TaskToAdd.CategoryId = 1;
            Assert.True(_viewmodel.AddTaskCommand.CanExecute(null));
        }

        [Fact]
        public void CanAddTask_ReturnsFalse_WhenTitleIsBlank()
        {
            _viewmodel.TaskToAdd.Title = "";
            _viewmodel.TaskToAdd.Deadline = DateTime.Now.AddDays(7);
            _viewmodel.TaskToAdd.CategoryId = 1;
            Assert.False(_viewmodel.AddTaskCommand.CanExecute(null));
        }

        [Fact]
        public void CanAddTask_ReturnsFalse_WhenDeadlineIsInThePast()
        {
            _viewmodel.TaskToAdd.Title = "New Task";
            _viewmodel.TaskToAdd.Deadline = DateTime.Now.AddDays(-1);
            _viewmodel.TaskToAdd.CategoryId = 1;
            Assert.False(_viewmodel.AddTaskCommand.CanExecute(null));
        }

        [Fact]
        public void CanAddTask_ReturnsFalse_WhenCategoryIsNotSelected()
        {
            _viewmodel.TaskToAdd.Title = "New Task";
            _viewmodel.TaskToAdd.Deadline = DateTime.Now.AddDays(7);
            Assert.False(_viewmodel.AddTaskCommand.CanExecute(null));
        }






        /*
         * Load Categories
         */

        [Fact]
        public async void LoadCategories_LoadsCategories_WhenExecuted()
        {
            List<Category> categories = new()
            {
                new Category
                {
                    Id = 1,
                    Name = "Category 1",
                    Color = "#FF0000"
                },
                new Category
                {
                    Id = 2,
                    Name = "Category 2",
                    Color = "#00FF00"
                }
            };
            _categoryServicesMock.Setup(ser => ser.GetAllAsync()).ReturnsAsync(categories);
            await _viewmodel.LoadCategoriesAsync();

            Assert.Equal(categories.Count, _viewmodel.Categories.Count);
        }



        /*
         * Add Category Command
         */

        [Fact]
        public void AddCategoryCommand_AddsCategory_WhenExecuted()
        {
            Category category = new()
            {
                Id = 1,
                Name = "New Category",
                Color = "#FFFFFF",
            };

            int oldCount = _viewmodel.Categories.Count;

            _categoryServicesMock.Setup(ser => ser.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).ReturnsAsync(category);
            _viewmodel.AddCategoryCommand.Execute(null);

            int newCount = _viewmodel.Categories.Count;

            Assert.True(oldCount < newCount);
        }

        [Fact]
        public void AddCategoryCommand_CategoryToAddIsReset_WhenExecuted()
        {
            Category category = new()
            {
                Id = 1,
                Name = "New Category",
                Color = "#FFFFFF",
            };
            _categoryServicesMock.Setup(ser => ser.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).ReturnsAsync(category);

            _viewmodel.CategoryToAdd.Name = "New Category";

            _viewmodel.AddCategoryCommand.Execute(null);

            Assert.True(string.IsNullOrEmpty(_viewmodel.CategoryToAdd.Name));
        }

        [Fact]
        public void CanAddCategory_ReturnsTrue_WhenRequiredFieldsAreFilled()
        {
            _viewmodel.CategoryToAdd.Name = "New Category";
            _viewmodel.CategoryToAdd.Color = "#FFFFFF";
            Assert.True(_viewmodel.AddCategoryCommand.CanExecute(null));
        }

        [Fact]
        public void CanAddCategory_ReturnsFalse_WhenNameIsBlank()
        {
            _viewmodel.CategoryToAdd.Name = "";
            _viewmodel.CategoryToAdd.Color = "#FFFFFF";
            Assert.False(_viewmodel.AddCategoryCommand.CanExecute(null));
        }

        [Fact]
        public void CanAddCategory_ReturnsFalse_WhenColorIsBlank()
        {
            _viewmodel.CategoryToAdd.Name = "New Category";
            _viewmodel.CategoryToAdd.Color = "";
            Assert.False(_viewmodel.AddCategoryCommand.CanExecute(null));
        }
    }
}
