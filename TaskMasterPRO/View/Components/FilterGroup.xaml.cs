using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace TaskMasterPRO.View.Components
{
    public partial class FilterGroup : UserControl
    {
        /*
         * Header
         */

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
            "Header",
            typeof(string),
            typeof(FilterGroup),
            new PropertyMetadata(string.Empty));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set { SetValue(HeaderProperty, value); }
        }



        /*
         *  ItemsSource
         */

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(FilterGroup),
            new PropertyMetadata(null));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set { SetValue(ItemsSourceProperty, value); }
        }



        /*
         * DisplayMemberPath
         */

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(
                "DisplayMemberPath",
                typeof(string),
                typeof(FilterGroup),
                new PropertyMetadata(string.Empty));

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public FilterGroup()
        {
            InitializeComponent();
        }
    }
}
