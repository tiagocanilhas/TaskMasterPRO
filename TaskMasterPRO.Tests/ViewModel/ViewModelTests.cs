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
        public void CanAddTask_ReturnsTrue_WhenTitleIsNotBlank()
        {
            _viewmodel.TaskToAdd.Title = "New Task";
            Assert.True(_viewmodel.AddTaskCommand.CanExecute(null));
        }

        [Fact]
        public void CanAddTask_ReturnsFalse_WhenTitleIsBlank()
        {
            _viewmodel.TaskToAdd.Title = "";
            Assert.False(_viewmodel.AddTaskCommand.CanExecute(null));
        }
    }
}
