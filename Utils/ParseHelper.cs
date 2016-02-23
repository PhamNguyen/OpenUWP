using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;


namespace OpenUWP.Utils
{
    public enum GameMediaLinkType
    {
        Cover,
        Screenshot,
        Icon,
        Thumbnail
    }
    public enum FeedServerImageSize
    {
        t1 = 0, //138, 138
        t2 = 1, //210, 210
        t3 = 2 //290, 290
    }

    public enum CoverServerImageResolution
    {
        ldpi, //97,67
        mdpi, //130,90
        hdpi, //195,135
        xhdpi, //260,180
        xxdpi //390,270
    }

    public class ParseHelper
    {
        public const int MaxCountOfDescription = 20;


        public static string GoogleApiKey = String.Empty;
        public static T TryGetValue<T>(dynamic data)
        {
            T result = default(T);

            var type = typeof(T);

            if (data != null)
            {
                string _data = data.ToString();
                try
                {
                    if (string.IsNullOrEmpty(_data))
                    {
                        return result;
                    }
                    var typeDat = data.ToString().ToLower();

                    switch (type.ToString())
                    {
                        case "System.Int32":
                            return int.Parse(typeDat);

                        case "System.Double":
                            return double.Parse(typeDat);

                        case "System.Int64":
                            return long.Parse(typeDat);

                        case "System.Single":
                            return float.Parse(typeDat);

                        case "System.Boolean":
                            {

                                result = Convert.ToBoolean(typeDat);
                            }
                            break;
                        case "System.String":
                            {

                                result = data.ToString() ;
                            }
                            break;
                        default:
                            {
                                result = (T)data;
                            }
                            break;
                    }
                }
                catch (Exception)
                {
                    result = (T)data;
                }
            }
            return result;
        }


        public static double StreamToKilobytes(Stream stream)
        {
            return Math.Round(stream.Length / 1024.0, 2);
        }

        public static double StreamToMegabytes(Stream stream)
        {
            return Math.Round(stream.Length / 1024.0 * 1024, 2);
        }

        public static string ShortenText(string input)
        {
            var stt = input;
            if (String.IsNullOrEmpty(stt))
                return stt;

            stt = stt.TrimEnd().TrimStart();

            if (String.IsNullOrEmpty(stt))
                return "";

            if (stt.Count() > 30)
                stt = stt.Substring(0, 30);
            else
                return "\"" + stt + "\"";

            var arrstt = stt.Split(' ');
            stt = "";
            for (int i = 0; i < arrstt.Count() - 1; i++)
            {
                stt += arrstt[i] + " ";
            }
            stt = stt.TrimEnd() + "...";
            stt = "\"" + stt + "\"";
            return stt;
        }

        public static String GetJsonString(string input)
        {
            if (!String.IsNullOrEmpty(input) && input.Contains("{") && input.Contains("}"))
            {
                string finalString = input.Replace("\\", String.Empty).Replace("\\{\"data\":\"\\{", "{").Replace("\\}\",\"type\":\"1\"\\}", "}");

                int indexOf = finalString.IndexOf('{');

                int lastIndexOf = finalString.LastIndexOf('}');

                int length = finalString.Length;

                return finalString.Substring(indexOf, lastIndexOf - indexOf + 1);
            }

            return input.Replace("\\", String.Empty);
        }

        public static byte[] StreamToByteArrayParser(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static void CopyObject<T>(T from, T to)
        {
            Type tto = typeof(T);
            PropertyInfo[] pFrom = tto.GetProperties();
            foreach (PropertyInfo p in pFrom)
            {
                if (p.CanWrite)
                {
                    PropertyInfo tmp = tto.GetProperty(p.Name);
                    if (tmp == null) continue;
                    tmp.SetValue(to, p.GetValue(from, null), null);
                }
            }
        }

        public static string SecondToCorrectFormatTime(double seconds = 0.0)
        {
            string timeString = "00:00";
            if (seconds == 0.0)
                return timeString;

            var second = (int)seconds % 60;
            var minutes = (int)seconds / 60;

            timeString = String.Format("{0}:{1}", minutes.ToString("D2"), second.ToString("D2"));

            return timeString;
        }

        public static string GetLinkImageLocation(double latitude, double longitude)
        {
            return String.Format("http://maps.googleapis.com/maps/api/staticmap?zoom=17&size=480x320&markers=size:mid|color:red|{0},{1}&key={2}", latitude, longitude, GoogleApiKey);
        }

        public static bool DetectUrlFromString(string text, out string urlText)
        {
            List<string> list = new List<string>();
            Regex urlRx = new Regex(@"(?:(?:http|https):\/\/)?([-a-zA-Z0-9.]{2,256}\.[a-z]{2,4})\b(?:\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?");

            MatchCollection matches = urlRx.Matches(text);
            foreach (Match match in matches)
            {
                list.Add(match.Value);
            }

            if (list.Any())
            {
                urlText = list[0];
                return true;
            }

            urlText = String.Empty;
            return false;
        }

        public static string CorrectTimeFormat(int value)
        {
            if (value == 0)
            {
                return "00:00";
            }

            var minutes = value / 60;
            var seconds = value % 60;
            return String.Format("{0}:{1}", minutes.ToString("D2"), seconds.ToString("D2"));
        }

        public static Uri CreateUriLink(string text)
        {
            try
            {
                if (!text.StartsWith("http"))
                {
                    return new Uri("http://" + text, UriKind.RelativeOrAbsolute);
                }
                return new Uri(text, UriKind.RelativeOrAbsolute);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                return null;
            }
        }

        public static string GetShorterContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return String.Empty;
            }

            var stringBuilder = new StringBuilder();
            string result = "";

            var arrayContent = content.Split(' ');

            if (arrayContent.Count() < MaxCountOfDescription)
            {
                result = content;
            }
            else
            {
                for (int i = 0; i < MaxCountOfDescription; i++)
                {
                    stringBuilder.Append(arrayContent[i]);
                    stringBuilder.Append(" ");
                }
                result = stringBuilder.ToString().TrimEnd();
                result = String.Format("{0}...", result);
            }
            return result;
        }        
        
        public static bool IsValidEmail(string inputEmail)
        {
            const string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var re = new Regex(strRegex);

            return re.IsMatch(inputEmail);
        }

    }
}
