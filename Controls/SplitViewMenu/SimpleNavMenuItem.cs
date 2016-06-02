using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OpenUWP.Controls.SplitViewMenu
{
    public sealed class SimpleNavMenuItem : INavigationMenuItem
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar => (char)Symbol;
        public object Arguments { get; set; }
        public Type DestinationPage { get; set; }
        public bool IsWarningLeavePage { get; set; }
    }
}