using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TaskMasterPRO.Data;
using TaskMasterPRO.ViewModel;

namespace TaskMasterPRO
{
    public partial class MainWindow : Window
    {

        private readonly Duration duration = new(TimeSpan.FromSeconds(0.3));
        private readonly IEasingFunction easing = new CubicEase() { EasingMode = EasingMode.EaseOut };

        public MainWindow()
        {
            InitializeComponent();

            content.Width = 0;
        }

        private void OnViewChanged(object sender, DataTransferEventArgs e)
        {
            if (content == null) return;

            if (innerContent.Content == null) // No content, closing the panel
            {
                DoubleAnimation closeAnimation = new(0, duration) { EasingFunction = easing };
                content.BeginAnimation(WidthProperty, closeAnimation);
            }
            else // New content, opening the panel
            {
                innerContent.Measure(new Size(content.MaxWidth, content.MaxHeight));
                double targetWidth = innerContent.DesiredSize.Width;
                DoubleAnimation openAnimation = new(targetWidth, duration);
                openAnimation.EasingFunction = easing;
                content.BeginAnimation(WidthProperty, openAnimation);
            }
        }
        
    };

}