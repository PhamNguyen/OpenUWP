using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Geolocation;
using Windows.Foundation.Metadata;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage.Streams;
using Windows.System.Profile;

namespace OpenUWP.Services
{
    public enum DistanceType
    {
        Miles,
        Kilometers,
    }

    public class PlatformService
    {
        public static object Instance { get; set; }

        public static void WriteLog(params object[] data)
        {
#if DEBUG
            if (data != null)
            {
                foreach (var item in data)
                {
                    if (item != null)
                    {
                        string logContent = item.ToString();

                        Debug.WriteLine(logContent);
                        Debug.WriteLine("\n");
                    }
                }
            }
#endif
        }


        // Replaces non-ASCII with escape sequences;
        public static string EscapeUnicode(string input)
        {
            var sb = new StringBuilder(input.Length);
            foreach (char ch in input)
            {
                if (ch <= 0x7f)
                    sb.Append(ch);
                else
                    sb.AppendFormat(CultureInfo.InvariantCulture, "\\u{0:x4}", (int)ch);
            }
            return sb.ToString();
        }

        public static string UrlEncode(string url)
        {
            return WebUtility.UrlEncode(url);
        }

        public static string UrlDecode(string url)
        {
            return WebUtility.UrlDecode(url);
        }

        public async static Task<Geoposition> GetCurrentLocation()
        {
            Geoposition pos = null;
            uint _desireAccuracyInMetersValue = 5;

            // Get cancellation token
            var cancel = new CancellationTokenSource();
            try
            {
                // Request permission to access location
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        CancellationToken token = cancel.Token;

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = _desireAccuracyInMetersValue };

                        // Carry out the operation
                        pos = await geolocator.GetGeopositionAsync().AsTask(token);
                        break;
                    case GeolocationAccessStatus.Denied:
                    case GeolocationAccessStatus.Unspecified:
                        break;
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
                cancel = null;
            }

            return pos;
        }

        public static double GetDistance(Geopoint pos1, Geopoint pos2, DistanceType type = DistanceType.Kilometers)
        {
            double R = (type == DistanceType.Miles) ? 3960 : 6371;
            double dLat = toRadian(pos2.Position.Longitude - pos1.Position.Latitude);
            double dLon = toRadian(pos2.Position.Longitude - pos1.Position.Longitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(toRadian(pos1.Position.Latitude)) * Math.Cos(toRadian(pos2.Position.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return d;
        }

        private static double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }

        public static string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

        }

        public static string GetDeviceId()
        {
            // get the unique device id for the publisher per device

            if (ApiInformation.IsTypePresent(typeof(HardwareIdentification).ToString()))
            {
                HardwareToken token = HardwareIdentification.GetPackageSpecificToken(null);
                IBuffer hardwareId = token.Id;

                HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
                IBuffer hashed = hasher.HashData(hardwareId);

                string uniqueDeviceId = CryptographicBuffer.EncodeToHexString(hashed);
                return uniqueDeviceId;
            }

            return string.Empty;
        }

        public static string GetCarrier()
        {
            var result = NetworkInformation.GetConnectionProfiles();
            string CarrierName = "Mobo";
            foreach (var connectionProfile in result)
            {
                if (connectionProfile.IsWwanConnectionProfile)
                {
                    foreach (var networkName in connectionProfile.GetNetworkNames())
                    {
                        CarrierName = networkName;//Get mobile operator Name  
                        break;
                    }
                }
            }

            return CarrierName;
        }

        public static async void ShowMessageBox(string content, string title)
        {
            await new Windows.UI.Popups.MessageDialog(content, title).ShowAsync();
        }
    }
}
