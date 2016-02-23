using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace OpenUWP.Utils
{
    public static class ImageHelper
    {
        public static async Task<byte[]> DownloadImage(string uri)
        {
            var httpClient = new HttpClient();
            try
            {
                var data = await httpClient.GetByteArrayAsync(uri);

                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "/n" + uri);
                return null;
            }
        }

        public static BitmapImage GetImageSource(string imagePath)
        {
            if (String.IsNullOrEmpty(imagePath))
            {
                return null;
            }
            var img = new BitmapImage
            {
                UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute),
                DecodePixelType = DecodePixelType.Logical
            };
            return img;
        }
    }
}
