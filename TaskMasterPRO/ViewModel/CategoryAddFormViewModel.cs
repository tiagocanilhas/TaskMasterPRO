using TaskMasterPRO.Core;
using TaskMasterPRO.Data.Domain;
using TaskMasterPRO.Data.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskMasterPRO.ViewModel
{
    public class CategoryAddFormViewModel(
        ICategoryServices categoryServices,
        IDialogServices dialogServices
        ) : BaseViewModel(dialogServices)
    {
        public event Action<Category> CategoryCreated;

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

            CategoryCreated?.Invoke(newCategory);

            CategoryToAdd = new();
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
