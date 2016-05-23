using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OpenUWP.Controls.SplitViewMenu
{
    public sealed class SplitViewMenu : Control
    {
        private MenuListView _navMenuListView;
        private ToggleButton _hamburgerToggleButton;
        private Grid _applicationTitleContainer;
        private Frame _pageFrame;

        public object DisplayModeTriggerMedium
        {
            get
            {
                if (MenuHeaderTemplate != null || !IsShowHamburgerButtonWhileOpen)
                {
                    return SplitViewDisplayMode.Overlay;
                }
                else
                {
                    return SplitViewDisplayMode.CompactOverlay;
                }
            }
        }

        public object DisplayModeTriggerLarge
        {
            get
            {
                if (MenuHeaderTemplate != null || !IsShowHamburgerButtonWhileOpen)
                {
                    return SplitViewDisplayMode.Inline;
                }
                else
                {
                    return SplitViewDisplayMode.CompactInline;
                }
            }
        }

        public object IsPaneOpenTriggerLarge
        {
            get
            {
                return true;
            }
        }

        public object IsPaneOpenTriggerMedium
        {
            get
            {
                return false;
            }
        }

        #region MenuItemTemplateSelectorProperty

        internal static readonly DependencyProperty MenuItemTemplateSelectorProperty =
            DependencyProperty.Register("MenuItemTemplateSelector", typeof(DataTemplateSelector),
                typeof(SplitViewMenu), new PropertyMetadata(null));

        public DataTemplateSelector MenuItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(MenuItemTemplateSelectorProperty); }
            set { SetValue(MenuItemTemplateSelectorProperty, value); }
        }
        #endregion

        #region MenuItemContainerStyleSelectorProperty

        internal static readonly DependencyProperty MenuItemContainerStyleSelectorProperty =
            DependencyProperty.Register("MenuItemContainerStyleSelector", typeof(StyleSelector),
                typeof(SplitViewMenu), new PropertyMetadata(null));
        public StyleSelector MenuItemContainerStyleSelector
        {
            get { return (StyleSelector)GetValue(MenuItemContainerStyleSelectorProperty); }
            set { SetValue(MenuItemContainerStyleSelectorProperty, value); }
        }
        #endregion

        #region MenuItemTemplateProperty

        internal static readonly DependencyProperty MenuItemTemplateProperty =
            DependencyProperty.Register("MenuItemTemplate", typeof(DataTemplate), typeof(SplitViewMenu),
                new PropertyMetadata(null));
        public DataTemplate MenuItemTemplate
        {
            get { return (DataTemplate)GetValue(MenuItemTemplateProperty); }
            set { SetValue(MenuItemTemplateProperty, value); }
        }
        #endregion

        #region MenuItemContainerStyleProperty

        internal static readonly DependencyProperty MenuItemContainerStyleProperty = 
            DependencyProperty.Register("MenuItemContainerStyle", typeof(Style), typeof(SplitViewMenu),
                new PropertyMetadata(null));

        public Style MenuItemContainerStyle
        {
            get { return (Style)GetValue(MenuItemContainerStyleProperty); }
            set { SetValue(MenuItemContainerStyleProperty, value); }
        }
        #endregion

        #region InitialPageProperty

        internal static readonly DependencyProperty InitialPageProperty =
            DependencyProperty.Register("InitialPage", typeof(Type), typeof(SplitViewMenu),
                new PropertyMetadata(null));
        public Type InitialPage
        {
            get { return (Type)GetValue(InitialPageProperty); }
            set { SetValue(InitialPageProperty, value); }
        }
        #endregion

        #region MenuItemsProperty

        internal static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register("MenuItems", typeof(IEnumerable<ISplitViewItem>),
                typeof(SplitViewMenu),
                new PropertyMetadata(Enumerable.Empty<ISplitViewItem>(), OnMenuItemsPropertyChanged));

        public IEnumerable<ISplitViewItem> MenuItems
        {
            get { return (IEnumerable<ISplitViewItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }
        private static void OnMenuItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menu = (SplitViewMenu)d;
            if (menu._navMenuListView != null)
                menu._navMenuListView.ItemsSource = e.NewValue;
        }
        #endregion

        #region MenuHeaderTemplateProperty

        internal static readonly DependencyProperty MenuHeaderTemplateProperty =
            DependencyProperty.Register("MenuHeaderTemplate", typeof(DataTemplate), typeof(SplitViewMenu),
                new PropertyMetadata(null));

        /// <summary>
        /// Cho phép custom hiển thị Header của List MenuItem.
        /// Mặc định sẽ không có MenuHeaderTemplate (nghĩa là = null).
        /// Nếu MenuHeaderTemplate != null thì SplitViewMenu không có chế độ Compact với màn hình Medium, Large
        /// </summary>
        public DataTemplate MenuHeaderTemplate
        {
            get { return (DataTemplate)GetValue(MenuHeaderTemplateProperty); }
            set { SetValue(MenuHeaderTemplateProperty, value); }
        }

        #endregion

        #region MenuFooterTemplateProperty

        internal static readonly DependencyProperty MenuFooterTemplateProperty =
            DependencyProperty.Register("MenuFooterTemplate", typeof(DataTemplate), typeof(SplitViewMenu),
                new PropertyMetadata(null));
        public DataTemplate MenuFooterTemplate
        {
            get { return (DataTemplate)GetValue(MenuFooterTemplateProperty); }
            set { SetValue(MenuFooterTemplateProperty, value); }
        }
        #endregion

        #region IsShowHamburgerButtonWhileOpenProperty

        internal static readonly DependencyProperty IsShowHamburgerButtonWhileOpenProperty =
            DependencyProperty.Register("IsShowHamburgerButtonWhileOpen", typeof(bool), typeof(SplitViewMenu),
                new PropertyMetadata(true));

        /// <summary>
        /// Cho phép được hiển thị Humburger button khi SplitViewMenu Open hay không?
        /// Mặc định value  =  true.
        /// Nếu value = false thì SplitViewMenu không có chế độ Compact với màn hình Medium, Large.
        /// </summary>
        public bool IsShowHamburgerButtonWhileOpen
        {
            get { return (bool)GetValue(IsShowHamburgerButtonWhileOpenProperty); }
            set { SetValue(IsShowHamburgerButtonWhileOpenProperty, value); }
        }
        #endregion

        #region HeaderProperty

        internal static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(SplitViewMenu),
                new PropertyMetadata(null));
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region HeaderBackgroundProperty

        internal static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(SplitViewMenu),
                new PropertyMetadata(Colors.Transparent));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }
        #endregion

        public SplitViewMenu()
        {
            DefaultStyleKey = typeof(SplitViewMenu);
            Loaded += SplitViewMenu_Loaded;
        }

        private void SplitViewMenu_Loaded(object sender, RoutedEventArgs e)
        {
            if (InitialPage == null || _pageFrame == null)
                return;
            _pageFrame.Navigate(InitialPage);
        }

        protected override void OnApplyTemplate()
        {
            _pageFrame = GetTemplateChild("PageFrame") as Frame;
            _navMenuListView = GetTemplateChild("NavMenuList") as MenuListView;
            _hamburgerToggleButton = GetTemplateChild("TogglePaneButton") as ToggleButton;
            _applicationTitleContainer = GetTemplateChild("HeaderContainer") as Grid;

            if (_navMenuListView != null)
            {
                _navMenuListView.ItemInvoked += OnNavMenuItemInvoked;
                _navMenuListView.ContainerContentChanging += OnContainerContextChanging;
            }

            if (_pageFrame != null)
            {
                _pageFrame.Navigating += OnNavigatingToPage;
                _pageFrame.Navigated += OnNavigatedToPage;
            }

            if (_hamburgerToggleButton != null)
            {
                _hamburgerToggleButton.Checked += OnHambugerToggleButtonChecked;
                _hamburgerToggleButton.Unchecked += OnHambugerToggleButtonUnChecked;
            }
        }

        private void OnHambugerToggleButtonUnChecked(object sender, RoutedEventArgs e)
        {
            if (!IsShowHamburgerButtonWhileOpen)
            {
                _hamburgerToggleButton.Visibility = Visibility.Visible;
            }
        }

        private void OnHambugerToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            if (!IsShowHamburgerButtonWhileOpen)
            {
                _hamburgerToggleButton.Visibility = _applicationTitleContainer.Visibility = Visibility.Collapsed;
            }
        }

        private static void OnContainerContextChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item is INavigationMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((INavigationMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            var page = e.Content as Page;
            if (page != null && e.Content != null)
            {
                var control = page;
                control.Loaded += PageLoaded;
            }
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= PageLoaded;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = _pageFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back || !MenuItems.Any())
                return;
            var item = MenuItems.SingleOrDefault(p => p is INavigationMenuItem && (p as INavigationMenuItem).DestinationPage == e.SourcePageType);
            if (item == null && _pageFrame.BackStackDepth > 0)
            {
                foreach (var entry in _pageFrame.BackStack.Reverse())
                {
                    item = MenuItems.SingleOrDefault(p => (p as INavigationMenuItem)?.DestinationPage == entry.SourcePageType);
                    if (item != null)
                        break;
                }
            }

            var container = (ListViewItem)_navMenuListView.ContainerFromItem(item);
            if (container != null)
                container.IsTabStop = false;
            _navMenuListView.SetSelectedItem(container);
            if (container != null)
                container.IsTabStop = true;
        }

        private void OnNavMenuItemInvoked(object sender, ListViewItem e)
        {
            var item = ((MenuListView)sender).ItemFromContainer(e);
            if (item is INavigationMenuItem)
            {
                var navigationItem = item as INavigationMenuItem;

                if (navigationItem?.DestinationPage != null &&
                    navigationItem.DestinationPage != _pageFrame.CurrentSourcePageType)
                {
                    _pageFrame.Navigate(navigationItem.DestinationPage, navigationItem.Arguments);
                }
            }
            else
            {
                var actionItem = item as IActionMenuItem;
                actionItem?.InvokeClick();
            }
        }
    }
}