using System.Collections.ObjectModel;
using TaskMasterPRO.Model;
using TaskMasterPRO.Services.Interfaces;

namespace TaskMasterPRO.ViewModel
{
    public class MainViewModel(
        ITaskServices taskServices,
        ICategoryServices categoryServices
        ) : BaseViewModel
    {

        public ObservableCollection<Model.Task> Tasks { get; set; }

        /*
         * Add Task Command
         */

        public RelayCommand AddTaskCommand => new(execute => AddTask(), canExecute => CanAddTask());

        private void AddTask()
        { 
            // TODO: Implement Add Task Logic
        }

        private bool CanAddTask()
        {
            return true;
        }

    }
}
