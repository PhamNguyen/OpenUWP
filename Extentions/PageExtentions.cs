using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OpenUWP.Extentions
{
    public static class PageExtentions
    {
        public static bool Navigate<T>(this Page currentPage, object data = null)
        {
            T page = default(T);
            if (page is Page)
            {
                if (data != null)
                    return currentPage.Frame.Navigate(typeof(T), data);
                else
                    return currentPage.Frame.Navigate(typeof(T));
            }
            return false;
        }
    }
}
