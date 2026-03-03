namespace TaskMasterPRO.ViewModel
{
    public class FilterItem<T> : ObservableObject
    {
        public string DisplayName { get; set; }
        public T Item { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
