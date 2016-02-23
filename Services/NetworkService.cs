using Windows.Networking.Connectivity;

namespace OpenUWP.Services
{
    public static class NetworkService
    {
        public static bool HasInternet()
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }
    }
}
