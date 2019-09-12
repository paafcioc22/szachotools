using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public void ConnectToWifi(string ssid, string password)
        {
            string networkSSID = ssid;
            string networkPass = password;

            WifiConfiguration wifiConfig = new WifiConfiguration();
            wifiConfig.Ssid = string.Format("\"{0}\"", networkSSID);
            wifiConfig.PreSharedKey = string.Format("\"{0}\"", networkPass);

            WifiManager wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
             

                if (!wifiManager.IsWifiEnabled)
            wifiManager.SetWifiEnabled(true);


            if ((wifiManager.ConnectionInfo.SSID != wifiConfig.Ssid) || wifiConfig.NetworkId<=0 )
            {
                int netId = wifiManager.AddNetwork(wifiConfig);
                 //   wifiManager.Disconnect();
                wifiManager.EnableNetwork(netId, true);
                   // wifiManager.Reconnect();

                
            }
            
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