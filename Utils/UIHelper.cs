using System;
using System.Diagnostics;
using System.Windows;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OpenUWP.Utils
{
    public class UIHelper
    {
        public static void SetClickEffect(object sender, bool isClick)
        {
            if (sender is UIElement)
            {
                var control = sender as UIElement;
                control.Opacity = isClick ? 0.4 : 1;
            }
        }

        public static Color PressedColor = HexToRgbConverter.Convert("#FF007ACC");

        public static void SetMouseEffectForPanel(object sender, bool isEnter) {
            if (sender is Panel)
            {
                var control = sender as Panel;
                control.Background = isEnter ? new SolidColorBrush(UIHelper.PressedColor) : new SolidColorBrush(Colors.Transparent);
            }
        }
        
        private static Stopwatch watch;
        public static void BeginTestPerformance()
        {
            if(watch == null ) watch = new Stopwatch();
            watch.Reset();
            watch.Start();
        }

        public static double StopTestPerformance()
        {
            watch.Stop();
            return watch.Elapsed.TotalSeconds;
        }

        public static T FindElementByName<T>(FrameworkElement element, string sChildName) where T : FrameworkElement
        {
            T childElement = null;
            if (element != null)
            {
                var nChildCount = VisualTreeHelper.GetChildrenCount(element);
                for (int i = 0; i < nChildCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;

                    if (child == null)
                        continue;

                    if (child is T && child.Name.Equals(sChildName))
                    {
                        childElement = (T)child;
                        break;
                    }

                    childElement = FindElementByName<T>(child, sChildName);

                    if (childElement != null)
                        break;
                }
                return childElement;
            }
            return null;
        }

    }
}
