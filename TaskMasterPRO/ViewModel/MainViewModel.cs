using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using TaskMasterPRO.Core;
using TaskMasterPRO.Data.Domain;
using TaskMasterPRO.Data.Services.Interfaces;
using TaskMasterPRO.ViewModel.Components.Filter;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.ViewModel
{
    public class MainViewModel(
        IDialogServices dialogServices,
        ICategoryServices categoryServices,
        ITaskServices taskServices
        ) : BaseViewModel(dialogServices)
    {
        public ObservableCollection<Data.Domain.Task> Tasks { get; set; } = new();
        public ICollectionView? TasksView { get; private set; }

        public ObservableCollection<Category> Categories { get; set; } = new();

        public FilterPanelViewModel FilterPanel { get; } = new();

        private bool FilterTaskPredicate(object obj)
        {
            if (obj is not Data.Domain.Task task)
                return false;

            var selectedCategories = FilterPanel.CategoryFilters.Where(f => f.IsSelected).ToList();

            bool categoryMatch =
                selectedCategories.Any(f => f.DisplayName == "All") ||
                selectedCategories.Any(f => f.Item?.Id == task.CategoryId);

            var selectedPriorities = FilterPanel.PriorityFilters.Where(f => f.IsSelected).ToList();

            bool priorityMatch =
                selectedPriorities.Any(f => f.DisplayName == "All") ||
                selectedPriorities.Any(f => f.Item == task.Priority);

            bool searchMatch =
                string.IsNullOrWhiteSpace(FilterPanel.SearchText) ||
                task.Title.Contains(FilterPanel.SearchText, StringComparison.OrdinalIgnoreCase);

            return categoryMatch && priorityMatch && searchMatch;
        }

        /*
         * Load Content
         */

        public async Task LoadContent()
        {
            await LoadTasksAsync();
            await LoadCategoriesAsync();
            SetPriorityFilters();

            TasksView = CollectionViewSource.GetDefaultView(Tasks);
            TasksView.Filter = FilterTaskPredicate;
            FilterPanel.FiltersChanged -= () => TasksView.Refresh();
            FilterPanel.FiltersChanged += () => TasksView.Refresh();
        }

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

            FilterPanel.CategoryFilters.Clear();
            FilterPanel.CategoryFilters.Add(new FilterItem<Category>()
            {
                DisplayName = "All",
                IsSelected = true
            });

            foreach (var category in data)
            {
                Categories.Add(category);

                FilterPanel.CategoryFilters.Add(new FilterItem<Category>
                {
                    DisplayName = category.Name,
                    Item = category,
                    IsSelected = false
                });
            }
        }

        private void SetPriorityFilters()
        {
            FilterPanel.PriorityFilters.Clear();

            FilterPanel.PriorityFilters.Add(new FilterItem<Priority>
            {
                DisplayName = "All",
                IsSelected = true,
                Item = default
            });

            foreach (Priority p in Enum.GetValues(typeof(Priority)))
            {
                FilterPanel.PriorityFilters.Add(new FilterItem<Priority>
                {
                    DisplayName = p.ToString(),
                    IsSelected = false,
                    Item = p
                });
            }

        }

        /*
         * Add Task Command
         */

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
            async obj => await (obj is Data.Domain.Task task ? ToggleTaskIsCompleted(task) : Task.CompletedTask),
            obj => CanToggleTaskIsCompleted((obj as Data.Domain.Task))
        );

        private async Task ToggleTaskIsCompleted(Data.Domain.Task task)
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

        private bool CanToggleTaskIsCompleted(Data.Domain.Task? task)
        {
            return task != null;
        }



        /*
         * Delete Task Command
         */

        public RelayCommand DeleteTaskCommand => new(
            async obj => await (obj is Data.Domain.Task task ? DeleteTask(task) : Task.CompletedTask),
            obj => CanDeleteTask((obj as Data.Domain.Task))
            );

        private async Task DeleteTask(Data.Domain.Task task)
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

        private bool CanDeleteTask(Data.Domain.Task? task)
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
            FilterPanel.CategoryFilters.Add(new FilterItem<Category>
            {
                DisplayName = newCategory.Name,
                Item = newCategory,
                IsSelected = false
            });
        }

        private bool CanAddCategory()
        {
            bool hasValidName = !string.IsNullOrWhiteSpace(CategoryToAdd.Name);
            bool hasValidDescription = !string.IsNullOrWhiteSpace(CategoryToAdd.Description);
            bool hasValidColor = !string.IsNullOrWhiteSpace(CategoryToAdd.Color);
            return hasValidName && hasValidDescription && hasValidColor;
        }
    }
}
