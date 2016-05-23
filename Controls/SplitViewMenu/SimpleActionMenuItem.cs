using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OpenUWP.Controls.SplitViewMenu
{
    public sealed class SimpleActionMenuItem : IActionMenuItem
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar => (char)Symbol;

        public event RoutedEventHandler Click;

        public void InvokeClick()
        {
            Click?.Invoke(this, null);
        }
    }
}
