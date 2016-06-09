using System;

namespace OpenUWP.Controls.SplitViewMenu
{
    public interface INavigationMenuItem : ISplitViewMenuItem
    {
        Type DestinationPage { get; }
        object Arguments { get; }
    }
}