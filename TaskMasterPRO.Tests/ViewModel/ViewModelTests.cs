using Moq;
using TaskMasterPRO.Model;
using TaskMasterPRO.Services.Interfaces;
using TaskMasterPRO.ViewModel;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.Tests.ViewModel
{
    public class ViewModelTests
    {

        private readonly Mock<IDialogServices> _dialogServicesMock;
        private readonly Mock<ICategoryServices> _categoryServicesMock;
        private readonly Mock<ITaskServices> _taskServicesMock;
        private readonly MainViewModel _viewmodel;

        public ViewModelTests()
        {
            _dialogServicesMock = new Mock<IDialogServices>();
            _categoryServicesMock = new Mock<ICategoryServices>();
            _taskServicesMock = new Mock<ITaskServices>();
            _viewmodel = new MainViewModel(
                _dialogServicesMock.Object,
                _categoryServicesMock.Object,
                _taskServicesMock.Object
            );
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
         * Toggle Task isCompleted Command
         */

        [Fact]
        public void ToggleTaskIsCompletedCommand_TogglesTaskCompletion_WhenExecuted()
        {
            // Simulate the UI state AFTER the user clicks the checkbox.
            // The WPF Two-Way Binding updates the property to 'true' BEFORE the command executes.

            Model.Task task = new() {
            Id = 1,
                CreationTime = DateTime.Now,
                Title = "Task to Toggle",
                Description = "Task Description",
                Deadline = DateTime.Now.AddDays(7),
                IsCompleted = true,                         // Represents the state already modified by the UI click
                Priority = Priority.Medium,
                CategoryId = 1
            };

            _taskServicesMock.Setup(ser => ser.UpdateAsync(
                task.Id,
                task.Title,
                task.Description,
                task.Deadline,
                task.IsCompleted,
                task.Priority,
                task.CategoryId
                )).ReturnsAsync(task);

            _viewmodel.ToggleTaskIsCompletedCommand.Execute(task);

            _taskServicesMock.Verify(ser => ser.UpdateAsync(
                task.Id,
                task.Title,
                task.Description,
                task.Deadline,
                task.IsCompleted,
                task.Priority,
                task.CategoryId
                ), Times.Once);

            Assert.True(task.IsCompleted);
        }

        [Fact]
        public void ToggleTaskIsCompletedCommand_CanExecute_ReturnsFalse_WhenTaskIsNull()
        {
            Assert.False(_viewmodel.ToggleTaskIsCompletedCommand.CanExecute(null));

            _taskServicesMock.Verify(ser => ser.UpdateAsync(
               It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(),
               It.IsAny<DateTime>(), It.IsAny<bool>(), It.IsAny<Priority>(), It.IsAny<int>()
           ), Times.Never);
        }

        [Fact]
        public async Task ToggleTaskIsCompletedCommand_OnDatabaseFailure_RollsBackState()
        {
            var task = new Model.Task { Id = 1, IsCompleted = true };

            _taskServicesMock.Setup(ser => ser.UpdateAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<bool>(), It.IsAny<Priority>(), It.IsAny<int>()
            )).ThrowsAsync(new Exception("Database connection failed"));

            _viewmodel.ToggleTaskIsCompletedCommand.Execute(task);

            Assert.False(task.IsCompleted);
        }



        /*
         * Remove Task Command
         */

        [Fact]
        public void DeleteTaskCommand_RemovesTask_WhenExecutedAndUserConfirms()
        {
            Model.Task task = new()
            {
                Id = 1,
                CreationTime = DateTime.Now,
                Title = "Task to Delete",
                Description = "Task Description",
                Deadline = DateTime.Now.AddDays(7),
                IsCompleted = false,
                Priority = Priority.Medium,
                CategoryId = 1
            };
            _viewmodel.Tasks.Add(task);

            int oldCount = _viewmodel.Tasks.Count;

            _dialogServicesMock.Setup(d => d.AskConfirmation(It.IsAny<string>(), It.IsAny<string>())).Returns(true); // Simulate user confirming the deletion
            _taskServicesMock.Setup(ser => ser.DeleteAsync(task.Id)).Returns(Task.CompletedTask);
            _viewmodel.DeleteTaskCommand.Execute(task);

            int newCount = _viewmodel.Tasks.Count;

            Assert.True(oldCount > newCount);
        }

        [Fact]
        public void DeleteTaskCommand_RemovesTask_WhenExecutedButUserCancels()
        {
            Model.Task task = new()
            {
                Id = 1,
                CreationTime = DateTime.Now,
                Title = "Task to Delete",
                Description = "Task Description",
                Deadline = DateTime.Now.AddDays(7),
                IsCompleted = false,
                Priority = Priority.Medium,
                CategoryId = 1
            };
            _viewmodel.Tasks.Add(task);

            int oldCount = _viewmodel.Tasks.Count;

            _dialogServicesMock.Setup(d => d.AskConfirmation(It.IsAny<string>(), It.IsAny<string>())).Returns(false); // Simulate user cancelling the deletion
            _taskServicesMock.Setup(ser => ser.DeleteAsync(task.Id)).Returns(Task.CompletedTask);
            _viewmodel.DeleteTaskCommand.Execute(task);

            int newCount = _viewmodel.Tasks.Count;

            Assert.True(oldCount == newCount);
        }

        [Fact]
        public void DeleteTaskCommand_CanExecute_ReturnsFalse_WhenTaskIsNull()
        {
            Assert.False(_viewmodel.DeleteTaskCommand.CanExecute(null));
            _taskServicesMock.Verify(ser => ser.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void DeleteTaskCommand_OnDatabaseFailure_DoesNotRemoveTask()
        {
            Model.Task task = new()
            {
                Id = 1,
                CreationTime = DateTime.Now,
                Title = "Task to Delete",
                Description = "Task Description",
                Deadline = DateTime.Now.AddDays(7),
                IsCompleted = false,
                Priority = Priority.Medium,
                CategoryId = 1
            };
            _viewmodel.Tasks.Add(task);

            int oldCount = _viewmodel.Tasks.Count;

            _dialogServicesMock.Setup(d => d.AskConfirmation(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _taskServicesMock.Setup(ser => ser.DeleteAsync(task.Id)).ThrowsAsync(new Exception("Error deleting task from database"));
            _viewmodel.DeleteTaskCommand.Execute(task);

            int newCount = _viewmodel.Tasks.Count;

            Assert.True(oldCount == newCount);
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
