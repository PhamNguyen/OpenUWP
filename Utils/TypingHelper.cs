using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel.DataTransfer;

namespace OpenUWP.Utils
{
    public class TypingHelper
    {
        /// <summary>
        /// Hàm lưu text vào clipboard
        /// </summary>
        /// <param name="content">Nội dung TEXT cần save vào clipboard</param>
        public static void SaveToClipboard(string content)
        {
            DataPackage pack = new DataPackage();
            pack.SetText(content);
            Clipboard.SetContent(pack);
        }
    }
}
