using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App2.Services
{
    public class NetworkMonitor  
    {
        private readonly IAppVersionProvider _alertService;
        public NetworkMonitor() 
        {            
            _alertService= DependencyService.Get<IAppVersionProvider>();
        }

        public void StartMonitoring()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        public void StopMonitoring()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }


        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            var profiles = e.ConnectionProfiles;

            if (access == NetworkAccess.Internet && profiles.Contains(ConnectionProfile.WiFi))
            {
                // Połączono z internetem przez WiFi
                Console.WriteLine("Connected to WiFi with internet access.");
                //_alertService.ShowShort("Połączono z WiFi i internetem");
            }
            else
            {
                // Brak połączenia z internetem przez WiFi
                Console.WriteLine("Not connected to WiFi or no internet access.");
                _alertService.ShowShort("Brak  połączenia z WiFi lub brak internetu");
            }
        }
    }

 
}
