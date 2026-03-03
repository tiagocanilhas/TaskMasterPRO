using System.Collections.ObjectModel;
using TaskMasterPRO.Model;
using TaskMasterPRO.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.ViewModel
{
    public class MainViewModel(
        IDialogServices dialogServices,
        ICategoryServices categoryServices,
        ITaskServices taskServices
        ) : BaseViewModel(dialogServices)
    {
        public ObservableCollection<Model.Task> Tasks { get; set; } = new();
        public ObservableCollection<Category> Categories { get; set; } = new();

        public async Task LoadTasksAsync()
        {
            var data = await taskServices.GetAllAsync();
            Tasks.Clear();
            foreach (var task in data)
            {
                Tasks.Add(task);
            }
        }

        public async Task LoadCategoriesAsync()
        {
            var data = await categoryServices.GetAllAsync();
            Categories.Clear();
            foreach (var category in data)
            {
                Categories.Add(category);
            }
        }
        /*
         * Add Task Command
         */

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

                    TaskToAdd = new();

                    Tasks.Add(newTask);
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



        /*
         * Toggle Task Is Completed Command
         */

        public RelayCommand ToggleTaskIsCompletedCommand => new(
            async obj => await (obj is Model.Task task ? ToggleTaskIsCompleted(task) : Task.CompletedTask), 
            obj => CanToggleTaskIsCompleted(obj as Model.Task)
        );

        private async Task ToggleTaskIsCompleted(Model.Task task)
        {
            await ExecuteSafelyAsync(
                action: async () => await taskServices.UpdateAsync(
                    task.Id,
                    task.Title,
                    task.Description,
                    task.Deadline,
                    task.IsCompleted,
                    task.Priority,
                    task.CategoryId
                    )
                ,
                onErrorRollback: () => task.IsCompleted = !task.IsCompleted
                );
        }

        private bool CanToggleTaskIsCompleted(Model.Task? task)
        {
            return task != null;
        }



        /*
         * Delete Task Command
         */

        public RelayCommand DeleteTaskCommand => new(
            async obj => await (obj is Model.Task task ? DeleteTask(task) : Task.CompletedTask), 
            obj => CanDeleteTask(obj as Model.Task)
            );

        private async Task DeleteTask(Model.Task task)
        {
            await ExecuteSafelyAsync(
                action: async () =>
                {
                    bool result = dialogServices.AskConfirmation(
                        $"Are you sure you want to delete the task '{task.Title}'?",
                        "Confirm Deletion"
                    );

                    if (result == false) return;

                    await taskServices.DeleteAsync(task.Id);

                    Tasks.Remove(task);
                },
                onErrorRollback: () => { }
            );
        }

        private bool CanDeleteTask(Model.Task? task)
        {
            return task != null;
        }



        /*
         * Add Category Command
         */


        private Category categoryToAdd = new();
        public Category CategoryToAdd
        {
            get => categoryToAdd;
            set
            {
                categoryToAdd = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddCategoryCommand => new(async _ => await AddCategory(), _ => CanAddCategory());

        private async Task AddCategory()
        {
            var newCategory = await categoryServices.CreateAsync(
                categoryToAdd.Name,
                categoryToAdd.Description,
                categoryToAdd.Color
            );

            CategoryToAdd = new();

            Categories.Add(newCategory);
        }

        private bool CanAddCategory()
        {
            bool hasValidName = !string.IsNullOrWhiteSpace(CategoryToAdd.Name);
            bool hasValidColor = !string.IsNullOrWhiteSpace(CategoryToAdd.Color);
            return hasValidName && hasValidColor;
        }
    }
}
