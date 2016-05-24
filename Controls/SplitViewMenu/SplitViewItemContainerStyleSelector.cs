using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OpenUWP.Controls.SplitViewMenu
{
    public class SplitViewItemContainerStyleSelector : StyleSelector
    {
        public Style SeparatorStyle { get; set; }
        public Style GroupStyle { get; set; }
        public Style NavigationMenuItemstyle { get; set; }
        public Style ActionMenuItemstyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            if (item is INavigationMenuItem)
            {
                return NavigationMenuItemstyle;
            }
            else if (item is IActionMenuItem)
            {
                return ActionMenuItemstyle;
            }
            else if (item is SplitViewSeparator)
            {
                return SeparatorStyle;
            }
            else if (item is SplitViewGroup)
            {
                return GroupStyle;
            }
            else
                return base.SelectStyleCore(item, container);
        }
    }
}
