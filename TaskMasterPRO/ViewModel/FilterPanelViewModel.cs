using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskMasterPRO.Model;

namespace TaskMasterPRO.ViewModel
{
    public partial class FilterPanelViewModel : ObservableObject
    {
        public event Action? FiltersChanged;

        public ObservableCollection<FilterItem<Category>> CategoryFilters { get; set; } = new();
        public ObservableCollection<FilterItem<Priority>> PriorityFilters { get; set; } = new();

        public ICommand ToggleFilterCommand { get; }

        public FilterPanelViewModel()
        {
            ToggleFilterCommand = new RelayCommand(ExecuteToggleFilter);
        }

        private void HandleAllLogic<T>(
            ObservableCollection<FilterItem<T>> collection,
            FilterItem<T> clickedItem
            )
        {
            if (clickedItem.DisplayName == "All")
            {
                if (clickedItem.IsSelected)
                {
                    foreach (var item in collection)
                    {
                        if (item != clickedItem) 
                            item.IsSelected = false;
                    }
                }
                else if (collection.All(x => !x.IsSelected))
                {
                    clickedItem.IsSelected = true;
                }
            }
            else
            {
                var allItem = collection.FirstOrDefault(x => x.DisplayName == "All");
                if (allItem == null) return;

                if (clickedItem.IsSelected)
                    allItem.IsSelected = false;

                if (collection.All(x => !x.IsSelected))
                    allItem.IsSelected = true;
            }
        }

        private void ExecuteToggleFilter(object clickedItem)
        {
            switch (clickedItem)
            {
                case FilterItem<Category> category:
                    HandleAllLogic(CategoryFilters, category);
                    break;

                case FilterItem<Priority> priority:
                    HandleAllLogic(PriorityFilters, priority);
                    break;

                default:
                    return;
            }

            FiltersChanged?.Invoke();
        }
    }
}
