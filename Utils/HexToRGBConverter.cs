﻿
using Windows.UI;

namespace OpenUWP.Utils
{
    public static class HexToRgbConverter
    {
        public static Color Convert(string hex)
        {
            try
            {
                if (!string.IsNullOrEmpty(hex))
                {
                    //remove the # at the front
                    hex = hex.Replace("#", "");

                    byte a = 255;
                    byte r = 255;
                    byte g = 255;
                    byte b = 255;

                    var start = 0;

                    //handle ARGB strings (8 characters long)
                    if (hex.Length == 8)
                    {
                        a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                        start = 2;
                    }

                    //convert RGB characters to bytes
                    r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

                    return Color.FromArgb(a, r, g, b); 
                }
            }
            catch
            {
                
            }

            return Colors.Transparent;
        }
    }
}
