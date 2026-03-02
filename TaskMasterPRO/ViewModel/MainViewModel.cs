using System.Collections.ObjectModel;
using System.Windows;
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
            _ => CanToggleTaskIsCompleted()
        );

        private async Task ToggleTaskIsCompleted(Model.Task task)
        {
            if (task == null) return;

            Console.WriteLine(task.IsCompleted);

            await taskServices.UpdateAsync(
                task.Id, 
                task.Title, 
                task.Description, 
                task.Deadline,
                task.IsCompleted,
                task.Priority, 
                task.CategoryId
            );
        }

        private bool CanToggleTaskIsCompleted()
        {
            return true;
        }



        /*
         * Delete Task Command
         */

        public RelayCommand DeleteTaskCommand => new(
            async obj => await (obj is Model.Task task ? DeleteTask(task) : Task.CompletedTask), 
            _ => CanDeleteTask()
            );

        private async Task DeleteTask(Model.Task task)
        {
            if (task == null) return;

            MessageBoxResult result = 
                MessageBox.Show(
                    $"Are you sure you want to delete the task '{task.Title}'?",
                    "Confirm Deletion", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Warning
                );

            if (result == MessageBoxResult.No) return;

            await taskServices.DeleteAsync(task.Id);

            Tasks.Remove(task);
        }

        private bool CanDeleteTask()
        {
            return true;
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
