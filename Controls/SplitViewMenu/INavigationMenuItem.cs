using System;

namespace OpenUWP.Controls.SplitViewMenu
{
    public interface INavigationMenuItem : ISplitViewMenuItem
    {
        Type DestinationPage { get; }
        object Arguments { get; }
        /// <summary>
        /// Cho phép hiển thị cảnh báo trước khi rời page hiện tại hay không? VD: Bạn có muốn rời khỏi trang này không?
        /// </summary>
        bool IsWarningLeavePage { get; }
    }
}