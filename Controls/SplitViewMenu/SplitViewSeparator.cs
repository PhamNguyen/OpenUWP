using Windows.UI.Xaml;

namespace OpenUWP.Controls.SplitViewMenu
{
    public class SplitViewSeparator : ISplitViewItem
    {
        public Thickness Margin { get; set; } = new Thickness(8);

        public double Height { get; set; } = 0.8;
    }
}
