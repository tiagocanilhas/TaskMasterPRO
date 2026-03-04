using System.ComponentModel;
using TaskMasterPRO.Data.Services.Interfaces;

namespace TaskMasterPRO.Core
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
