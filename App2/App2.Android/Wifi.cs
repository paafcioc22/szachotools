using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
//using Android.Support.V7.App;
using App2.Droid;
using App2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;


[assembly: Xamarin.Forms.Dependency(typeof(Wifi))]

namespace App2.Droid
{
    //[Android.Runtime.Register("onActivityResult", "(IILandroid/content/Intent;)V", "GetOnActivityResult_IILandroid_content_Intent_Handler")]

    class Wifi : AppCompatActivity, IWifiConn10
    {
        //public bool RequestNetwork(string ssid, string password)
        //{
        //    //MainActivity.RequestNetwork(ssid, password);
        //    var connectivityManager = Android.App.Application.Context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;
        //}

        private NetworkCallback _callback;
        //private TextView _statusText;
        private EditText _ssid;
        private EditText _passphrase;
        private bool _requested;
        string status;

        private const int AddWifiSettingsRequestCode = 4242;

        public Wifi()
        {
            _callback = new NetworkCallback
            {
                NetworkAvailable = network =>
                {
                    // we are connected!
                    //_statusText.Text = $"Request network available";
                    status = $"Request network available";
                },
                NetworkUnavailable = () =>
                {
                    //_statusText.Text = $"Request network unavailable";
                    status = $"Request network unavailable";
                }
            };
        }

        
        public string SuggestNetwork(string _ssid, string _passphrase)
        {
            var version = DeviceInfo.VersionString;
            var version2 = DeviceInfo.Version.Major;  

            if (version2 >= 11)
            {
                var intent = new Intent("android.settings.WIFI_ADD_NETWORKS");
                var bundle = new Bundle();
                bundle.PutParcelableArrayList("android.provider.extra.WIFI_NETWORK_LIST",
                    new List<IParcelable>
                    {
                    new WifiNetworkSuggestion.Builder()
                        .SetSsid(_ssid)
                        .SetWpa2Passphrase(_passphrase)
                        .Build(),
                    //new WifiNetworkSuggestion.Builder()
                    //    .SetSsid("JOART_WiFi")
                    //    .SetWpa2Passphrase(_passphrase)
                    //    .Build()
                    });

                intent.PutExtras(bundle);


                var activity = MainActivity.MainActivityInstance;
                //StartActivityForResult(intent, AddWifiSettingsRequestCode);
                activity.StartActivityForResult(intent, AddWifiSettingsRequestCode);
            }
            else if(version2 == 10)
            {
                var suggestion = new WifiNetworkSuggestion.Builder()
                    .SetSsid(_ssid)
                    .SetWpa2Passphrase(_passphrase) 
                    .Build();
                 

                var suggestions = new[] { suggestion   };

                var wifiManager = Android.App.Application.Context.GetSystemService(Context.WifiService) as WifiManager;
                var statuss = wifiManager.AddNetworkSuggestions(suggestions);
                //var statuss = wifiManager.RemoveNetworkSuggestions(suggestions);

                var statusText = statuss switch
                {
                    NetworkStatus.SuggestionsSuccess => "Sieć dodano - zatwierdź w ustawieniach",
                    NetworkStatus.SuggestionsErrorAddDuplicate => "Sieć została już dodana",
                    NetworkStatus.SuggestionsErrorAddExceedsMaxPerApp => "Suggestion Exceeds Max Per App",
                    NetworkStatus.SuggestionsErrorRemoveInvalid => "ErrorRemove",
                    NetworkStatus.SuggestionsErrorAddInvalid => "Dodanie nie powiodło się",
                    NetworkStatus.SuggestionsErrorAddNotAllowed => "Dodanie nie powiodło się",
                };
                 
                return statusText;

            }
            else
            {

            }
             
            return null;

        }

        public void OpenSettings()
        {
            var activity = MainActivity.MainActivityInstance;
            activity.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionWifiSettings));

        }

        public string RemoveSuggestNetwork(string _ssid, string _passphrase)
        {
            var version = DeviceInfo.VersionString;
            var version2 = DeviceInfo.Version.Major;



      
            if   (version2 == 10)
            {
                var suggestion = new WifiNetworkSuggestion.Builder()
                    .SetSsid(_ssid)
                    .SetWpa2Passphrase(_passphrase)
                    //.SetIsUserInteractionRequired(true)    
                    .Build();

                //var suggestion2 = new WifiNetworkSuggestion.Builder()
                //    .SetSsid("JOART_WiFi")
                //    .SetWpa2Passphrase(_passphrase)
                //    //.SetIsUserInteractionRequired(true)    
                //    .Build();

                var suggestions = new[] { suggestion };

                var wifiManager = Android.App.Application.Context.GetSystemService(Context.WifiService) as WifiManager;
            
                var statuss = wifiManager.RemoveNetworkSuggestions(suggestions);

                var statusText = statuss switch
                {
                    NetworkStatus.SuggestionsSuccess => "Sieć dodano - zatwierdź w ustawieniach",
                    NetworkStatus.SuggestionsErrorAddDuplicate => "Sieć została już dodana",
                    NetworkStatus.SuggestionsErrorAddExceedsMaxPerApp => "Suggestion Exceeds Max Per App",
                    NetworkStatus.SuggestionsErrorRemoveInvalid => "ErrorRemove",
                    NetworkStatus.SuggestionsErrorAddInvalid => "Dodanie nie powiodło się",
                    NetworkStatus.SuggestionsErrorAddNotAllowed => "Dodanie nie powiodło się",

                };

                return statusText;

            }
            else
            {

            }



            return null;

        }

        public bool RequestNetwork(string _ssid, string _passphrase)
        {


           // Android.App.Application.Context.StartActivity(intent, AddWifiSettingsRequestCode);




            //var specifier = new WifiNetworkSpecifier.Builder()
            //    .SetSsid(_ssid)
            //    .SetWpa2Passphrase(_passphrase)
            //    .Build();

            //var request = new NetworkRequest.Builder()
            //    .AddTransportType(Android.Net.TransportType.Wifi)
            //    .SetNetworkSpecifier(specifier)
            //    .AddCapability(NetCapability.Internet)
            //    .Build();

            //var connectivityManager = Android.App.Application.Context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;

            //if (_requested)
            //{
            //    connectivityManager.UnregisterNetworkCallback(_callback);
            //}

            //connectivityManager.RequestNetwork(request, _callback);
            //_requested = true;
            return _requested;
        }

  

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == AddWifiSettingsRequestCode)
            {
                if (data != null && data.HasExtra("android.provider.extra.WIFI_NETWORK_RESULT_LIST"))
                {
                    var extras =
                        data.GetIntegerArrayListExtra("android.provider.extra.WIFI_NETWORK_RESULT_LIST")
                            ?.Select(i => i.IntValue()).ToArray() ?? new int[0];

                    if (extras.Length > 0)
                    {
                        var ok = extras.Select(GetResultFromCode).All(r => r == Result.Ok);
                        //_result.Text = $"Result {ok}";
                        return;
                    }
                }

                //_result.Text = $"Result {resultCode == Result.Ok}";
            }
        }

        private static Result GetResultFromCode(int code) =>
            code switch
            {
                0 => Result.Ok,
                2 => Result.Ok,
                _ => Result.Canceled
            };
    
    private class NetworkCallback : ConnectivityManager.NetworkCallback
        {
            public Action<Network> NetworkAvailable { get; set; }
            public Action NetworkUnavailable { get; set; }

            public override void OnAvailable(Network network)
            {
                base.OnAvailable(network);
                NetworkAvailable?.Invoke(network);
            }

            public override void OnUnavailable()
            {
                base.OnUnavailable();
                NetworkUnavailable?.Invoke();
            }
        }
    }
}