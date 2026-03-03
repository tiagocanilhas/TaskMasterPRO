using System.ComponentModel;
using TaskMasterPRO.Services.Interfaces;

namespace TaskMasterPRO.ViewModel
{
    public class BaseViewModel(
        IDialogServices dialogServices
        ) : ObservableObject(), INotifyPropertyChanged
    {
        protected async Task ExecuteSafelyAsync(Func<Task> action, Action? onErrorRollback = null)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            { 
                dialogServices.ShowError(ex.Message, "Error");
                onErrorRollback?.Invoke();
            }
        }
    }
}
