using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OpenUWP.Extentions
{
    public static class ListViewBaseExtentions
    {
        /// <summary>
        /// Scrolls the list to bring the specified data item into view. Đây là hàm đã gọi thêm hàm UpdateLayout() trước khi gọi hàm ScrollIntoView(...)
        /// </summary>
        /// <param name="currentListView"></param>
        /// <param name="index">Vị trí của phần tử sẽ scroll tới trong list</param>
        /// <returns>Return True nếu scroll thành công, ngược lại return False.</returns>
        public static void BringIntoView(this ListViewBase currentListView, object item)
        {
            currentListView.UpdateLayout();
            currentListView.ScrollIntoView(item);
        }
    }
}
