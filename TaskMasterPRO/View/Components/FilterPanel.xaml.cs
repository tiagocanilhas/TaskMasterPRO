using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace TaskMasterPRO.View.Components
{
    public partial class FilterPanel : UserControl
    {

        private readonly Duration duration = new (TimeSpan.FromSeconds(0.3));
        private readonly IEasingFunction easing = new CubicEase() { EasingMode = EasingMode.EaseOut };

        public FilterPanel()
        {
            InitializeComponent();

            innerContent.Measure(new Size(content.MaxWidth, content.MaxHeight));
            content.Width = innerContent.DesiredSize.Width;
        }


        private void OpenPanel(object sender, RoutedEventArgs e)
        {
            if (content == null) return;

            innerContent.Measure(new Size(content.MaxWidth, content.MaxHeight));
            DoubleAnimation openAnimation = new (0, innerContent.DesiredSize.Width, duration);
            openAnimation.EasingFunction = easing;
            content.BeginAnimation(WidthProperty, openAnimation);

            Button.LayoutTransform = new RotateTransform(0);
        }

        private void ClosePanel(object sender, RoutedEventArgs e)
        {
            DoubleAnimation closeAnimation = new(0, duration);
            closeAnimation.EasingFunction = easing;
            content.BeginAnimation(WidthProperty, closeAnimation);

            Button.LayoutTransform = new RotateTransform(180);
        }
    }
}