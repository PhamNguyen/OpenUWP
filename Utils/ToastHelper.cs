using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Notifications;

namespace OpenUWP.Utils
{
    public static class ToastHelper
    {
        public static ToastNotification PopToast(string title, string content, DateTimeOffset? expirationTime)
        {
            return PopToast(title, content, null, null, expirationTime);
        }

        public static ToastNotification PopToast(string title, string content, string tag, string group, DateTimeOffset? expirationTime)
        {
            string xml = $@"<toast activationType='foreground'>
                                            <visual>
                                                <binding template='ToastGeneric'>
                                                </binding>
                                            </visual>
                                        </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var binding = doc.SelectSingleNode("//binding");

            var el = doc.CreateElement("text");
            el.InnerText = title;

            binding.AppendChild(el);

            el = doc.CreateElement("text");
            el.InnerText = content;
            binding.AppendChild(el);

            return PopCustomToast(doc, tag, group, expirationTime);
        }

        public static ToastNotification PopCustomToast(string xml, DateTimeOffset? expirationTime)
        {
            return PopCustomToast(xml, null, null, expirationTime);
        }

        public static ToastNotification PopCustomToast(string xml, string tag, string group, DateTimeOffset? expirationTime)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            
            return PopCustomToast(doc, tag, group, expirationTime);
        }

        [DefaultOverloadAttribute]
        public static ToastNotification PopCustomToast(XmlDocument doc, string tag, string group, DateTimeOffset? expirationTime)
        {
            var toast = new ToastNotification(doc);
            toast.ExpirationTime = expirationTime;

            if (tag != null)
                toast.Tag = tag;

            if (group != null)
                toast.Group = group;

            ToastNotificationManager.CreateToastNotifier().Show(toast);

            return toast;
        }

        public static string ToString(ValueSet valueSet)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var pair in valueSet)
            {
                if (builder.Length != 0)
                    builder.Append('\n');

                builder.Append(pair.Key);
                builder.Append(": ");
                builder.Append(pair.Value);
            }

            return builder.ToString();
        }

        public static void PopSilentToast(string content, string title = "", string logoDirectLink = "")
        {
            string xml;

            if (!string.IsNullOrEmpty(logoDirectLink))
            {
                xml = $@"<toast>
                                <visual>
                                    <binding template='ToastGeneric'>
                                        <text>{title}</text>
                                        <text>{content}</text>
                                        <image src='{logoDirectLink}' placement='appLogoOverride' hint-crop='circle'/>
                                    </binding>
                                </visual>
                                <audio silent='true'/>
                            </toast>";
            }
            else
            {
                xml = $@"<toast>
                                <visual>
                                    <binding template='ToastGeneric'>
                                        <text>{title}</text>
                                        <text>{content}</text>
                                    </binding>
                                </visual>
                                <audio silent='true'/>
                            </toast>";
            }

            PopCustomToast(xml, DateTime.Now.AddSeconds(5));
        }
    }
}
