using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace OpenUWP.Models
{
    public enum ApplicationState
    {
        Active,
        Background
    }

    public class ClientInfo
    {
        #region Singleton

        private static object syncRoot = new object();

        private static volatile ClientInfo instance;

        public static ClientInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ClientInfo();
                    }
                }
                return instance;
            }
        }

        private ClientInfo()
        {
            GetClientInfo();
        }
        #endregion

        public static ApplicationState ApplicationState = ApplicationState.Active;

        public string Agent = "Internet Explorer";

        public string Os = "W10forMobile";

        public int Channel = 1;

        public string Version;

        public string Carrier;

        public string Platform = "3";

        private string deviceId;
        public string DeviceId
        {
            get
            {
                if (String.IsNullOrEmpty(deviceId))
                    deviceId = this.GetDeviceId();
                return deviceId;
            }
        }

        public string GetDeviceId()
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

        public string Ip = "123";
        public string Jailbreak;
        public string OsVersion;
        public float Width;
        public float Weight;
        public double Latitude;
        public double Longitude;
        public string DeviceToken = "";
        public string PrivateKey = "";
        public string App = "omga";
        public string AppVersion = "";
        public string Mobo = "mobo";

        /// <summary>
        /// The resolution.
        /// hdpi
        /// ldpi
        /// mdpi
        /// xhdpi
        /// xxhdpi
        /// </summary>
        public string Resolution = "hdpi";

        private static CancellationTokenSource _cts = null;

        /// <summary>
        /// Chi dung cho iOS
        /// 1 test store
        /// 2 production store
        /// 3 production enterprise
        /// 4 production adhoc
        /// </summary>
        public string env = "3";
        public float Height { get; set; }

        public void UpdateClientInfo(dynamic clientInfo)
        {
            this.PrivateKey = clientInfo.data;
        }

        public string Language = "vi";


        private static uint _desireAccuracyInMetersValue = 5;

        public async void GetClientInfo()
        {
            string SystemFamily = string.Empty;
            string SystemVersion = string.Empty;
            string SystemArchitecture = string.Empty;
            string ApplicationName = string.Empty;
            string ApplicationVersion = string.Empty;
            string DeviceManufacturer = string.Empty;
            string DeviceModel = string.Empty;
            
            // get the system family name
            AnalyticsVersionInfo ai = AnalyticsInfo.VersionInfo;
            SystemFamily = ai.DeviceFamily;

            // get the system version number
            string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (v & 0x000000000000FFFFL);
            SystemVersion = $"{v1}.{v2}.{v3}.{v4}";

            // get the package architecure
            Package package = Package.Current;
            SystemArchitecture = package.Id.Architecture.ToString();

            // get the user friendly app name
            ApplicationName = package.DisplayName;

            // get the app version
            PackageVersion pv = package.Id.Version;
            ApplicationVersion = $"{pv.Major}.{pv.Minor}.{pv.Build}.{pv.Revision}";

            // get the device manufacturer and model name
            EasClientDeviceInformation eas = new EasClientDeviceInformation();
            DeviceManufacturer = eas.SystemManufacturer;
            DeviceModel = eas.SystemProductName;
            
            Agent = "mobo";
            Carrier = GetCarrier();
            OsVersion = SystemVersion;
            AppVersion = ApplicationVersion;
            Version = GetAppVersion();
            Jailbreak = "0";

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
                    var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                    var size = new Size(bounds.Width * scaleFactor, bounds.Height * scaleFactor);

                    Width = (float)size.Width;
                    Height = (float)size.Height;
                });

            var location = await GetCurrentLocation();
            if (location != null)
            {
                var coordinate = location.Coordinate;
                if (coordinate != null)
                    Longitude = !coordinate.Point.Position.Longitude.Equals(double.NaN) ? coordinate.Point.Position.Longitude : 106.684735;
                Latitude = !coordinate.Point.Position.Latitude.Equals(double.NaN) ? coordinate.Point.Position.Latitude : 10.785187;
            }
            else
            {
                Longitude = 106.684735;
                Latitude = 10.785187;
            }
        }

        public async static Task<Geoposition> GetCurrentLocation()
        {
            Geoposition pos = null;
            try
            {
                // Request permission to access location
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // Get cancellation token
                        _cts = new CancellationTokenSource();
                        CancellationToken token = _cts.Token;

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
                _cts = null;
            }

            return pos;
        }

        public static string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

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
    }
}