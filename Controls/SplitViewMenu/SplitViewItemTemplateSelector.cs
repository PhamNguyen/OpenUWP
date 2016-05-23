using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OpenUWP.Controls.SplitViewMenu
{
    public class SplitViewItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GroupTemplate { get; set; }
        public DataTemplate SeparatorTemplate { get; set; }
        public DataTemplate NavigationMenuItemTemplate { get; set; }
        public DataTemplate ActionMenuItemTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is INavigationMenuItem)
            {
                return NavigationMenuItemTemplate;
            }
            else if (item is IActionMenuItem)
            {
                return ActionMenuItemTemplate;
            }
            else if (item is SplitViewSeparator)
            {
                return SeparatorTemplate;
            }
            else
            {
                return GroupTemplate;
            }
        }
    }
}
