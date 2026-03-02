using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TaskMasterPRO.View.Components
{
    public partial class TaskCard : UserControl
    {
        /*
         *  OnIsCompletedClickCommand
         */

        public static readonly DependencyProperty OnIsCompletedClickCommandProperty =
            DependencyProperty.Register(
                "OnIsCompletedClickCommand",    // Name of the property
                typeof(ICommand),               // Type of the property
                typeof(TaskCard),               // Owner type (the class that defines the property)
                new PropertyMetadata(null));    // Default value for the property

        public ICommand OnIsCompletedClickCommand
        {
            get => (ICommand)GetValue(OnIsCompletedClickCommandProperty);
            set { SetValue(OnIsCompletedClickCommandProperty, value); }
        }

        /*
         * OnDeleteClickCommand
         */

        public static readonly DependencyProperty OnDeleteClickCommandProperty =
            DependencyProperty.Register(
                "OnDeleteClickCommand",
                typeof(ICommand),
                typeof(TaskCard),
                new PropertyMetadata(null));

        public ICommand OnDeleteClickCommand
        {
            get => (ICommand)GetValue(OnDeleteClickCommandProperty);
            set { SetValue(OnDeleteClickCommandProperty, value); }
        }

        public TaskCard()
        {
            InitializeComponent();
        }
    }
}
