using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TaskMasterPRO.Services.Interfaces;

namespace TaskMasterPRO.ViewModel
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}