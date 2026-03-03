using System.Windows;
using TaskMasterPRO.Services.Interfaces;

namespace TaskMasterPRO.Services
{
    public class DialogServices : IDialogServices
    {
        public bool AskConfirmation(string message, string title) => 
            MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        

        public void ShowError(string message, string title) => 
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}