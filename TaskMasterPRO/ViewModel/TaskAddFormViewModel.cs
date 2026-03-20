using TaskMasterPRO.Core;
using TaskMasterPRO.Data.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.ViewModel
{
    public class TaskAddFormViewModel(
        ITaskServices taskServices,
        IDialogServices dialogServices
        ) : BaseViewModel(dialogServices)
    {
        public event Action<Data.Domain.Task>? TaskCreated;

        private Data.Domain.Task taskToAdd = new();
        public Data.Domain.Task TaskToAdd
        {
            get => taskToAdd;
            set
            {
                taskToAdd = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddTaskCommand => new(async _ => await AddTask(), _ => CanAddTask());

        private async Task AddTask()
        {
            await ExecuteSafelyAsync(
                action: async () =>
                {
                    var newTask = await taskServices.CreateAsync(
                        taskToAdd.Title,
                        taskToAdd.Description,
                        taskToAdd.Deadline,
                        false,
                        taskToAdd.Priority,
                        taskToAdd.CategoryId
                     );

                    TaskCreated?.Invoke(newTask);

                    TaskToAdd = new();
                },
                onErrorRollback: () => { }
            );
        }

        private bool CanAddTask()
        {
            bool hasValidTitle = !string.IsNullOrWhiteSpace(TaskToAdd.Title);
            bool hasValidDeadline = TaskToAdd.Deadline > DateTime.Now;
            bool hasValidCategory = TaskToAdd.CategoryId > 0;
            return hasValidTitle && hasValidDeadline && hasValidCategory;
        }
    }
}
