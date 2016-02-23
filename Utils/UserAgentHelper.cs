using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml.Controls;

namespace OpenUWP.Utils
{
    public static class UserAgentHelper
    {
        private const string Html =
            @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"">
 
        <html>
        <head>
        <script language=""javascript"" type=""text/javascript"">
            function notifyUA() {
               window.external.notify(navigator.userAgent);
            }
        </script>
        </head>
        <body onload=""notifyUA();""></body>
        </html>";

        //public static void GetUserAgent(Panel rootElement, Action<string> callback)
        //{
        //    var browser = new WebView
        //    {
        //        IsScriptEnabled = true,
        //        Visibility = Visibility.Collapsed
        //    };
        //    browser.Loaded += (sender, args) => browser.NavigateToString(Html);
        //    browser.ScriptNotify += (sender, args) =>
        //    {
        //        string userAgent = args.Value;
        //        rootElement.Children.Remove(browser);
        //        callback(userAgent);
        //    };
        //    rootElement.Children.Add(browser);
        //}
    }
}
