using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
 
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App2.Droid;
using App2.Model;
using Xamarin.Forms;

[assembly: Dependency(typeof(WiFiConnector))]
namespace App2.Droid
{
    class WiFiConnector : IWifiConnector
    {
        [Obsolete]
        public bool ConnectToWifi(string ssid, string password)
        {
            string networkSSID = ssid;
            string networkPass = password;


            var formattedSsid = $"\"{ssid}\"";
            var formattedPassword = $"\"{password}\"";

            WifiConfiguration wifiConfig = new WifiConfiguration();
            wifiConfig.Ssid = formattedSsid;// string.Format("{0}", networkSSID);
            wifiConfig.PreSharedKey = formattedPassword;// string.Format("{0}", networkPass);
            //wifiConfig.AllowedKeyManagement.Set((int)KeyManagementType.None);

            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);


           // Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionWifiSettings));
            if (!wifiManager.IsWifiEnabled)
            wifiManager.SetWifiEnabled(true);

            int netId = wifiManager.AddNetwork(wifiConfig);

            if ((wifiManager.ConnectionInfo.SSID != wifiConfig.Ssid) || wifiConfig.NetworkId <= 0)
            {
                wifiManager.Disconnect();
                Thread.Sleep(1000);
                wifiManager.EnableNetwork(netId, true);
                Thread.Sleep(2000);
                //wifiManager.Reconnect();

            }

            if (wifiManager.Reconnect())
            {
                return true;
            }
            else
            {
                return false;
            }

             Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionWifiSettings));

        }

        public bool IsConnectedToWifi(string ssid2)
        {
            
            
            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);

            string ssid = string.Format("\"{0}\"", ssid2);
            string ss1 = wifiManager.ConnectionInfo.SSID;

            Task.Delay(1000);

            if (wifiManager.ConnectionInfo.SSID == ssid&& wifiManager.IsWifiEnabled&&
                wifiManager.ConnectionInfo.NetworkId>0)
                {
                    return true;
                }
                else return false; 




        }



         
    }
}