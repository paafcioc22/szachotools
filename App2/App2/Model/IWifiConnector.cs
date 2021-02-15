using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model
{
    public interface IWifiConnector
    {
        bool ConnectToWifi(string ssid, string password);
        bool IsConnectedToWifi(string ssid);
    }
}
