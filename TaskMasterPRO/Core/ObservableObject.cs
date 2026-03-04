using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TaskMasterPRO.Data.Services.Interfaces;

namespace TaskMasterPRO.Core
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