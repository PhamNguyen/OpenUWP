using Windows.UI.Xaml;

namespace OpenUWP.Controls.SplitViewMenu
{
    public interface IActionMenuItem : ISplitViewMenuItem
    {
        event RoutedEventHandler Click;
        void InvokeClick();
    }
}
