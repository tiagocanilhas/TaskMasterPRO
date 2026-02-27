using System.Collections.ObjectModel;
using TaskMasterPRO.Migrations;
using TaskMasterPRO.Model;
using TaskMasterPRO.Services;
using TaskMasterPRO.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.ViewModel
{
    public class MainViewModel(
        ICategoryServices categoryServices,
        ITaskServices taskServices
        ) : BaseViewModel
    {
        public ObservableCollection<Model.Task> Tasks { get; set; } = new();

        private Model.Task taskToAdd = new();
        public Model.Task TaskToAdd
        {
            get => taskToAdd;
            set
            {
                taskToAdd = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadTasksAsync()
        {
            var data = await taskServices.GetAllAsync();
            Tasks.Clear();
            foreach(var task in data)
            {
                Tasks.Add(task);
            }
        }

        /*
         * Add Task Command
         */

        public RelayCommand AddTaskCommand => new(async _ => await AddTask(), _ => CanAddTask());

        private async Task AddTask()
        {
            var newTask = await taskServices.CreateAsync(
                taskToAdd.Title,
                "",                         // Default value for now
                DateTime.Now.AddDays(7),    // Default value for now
                false,                      // Default value for now
                taskToAdd.Priority,
                1                           // Default value for now
             );

            TaskToAdd = new();

            Tasks.Add(newTask);
        }

        private bool CanAddTask() => !string.IsNullOrWhiteSpace(TaskToAdd.Title);
        

    }
}
