using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Services
{
    public interface IWifiConnector
    {
        bool ConnectToWifi(string ssid, string password);
        bool IsConnectedToWifi(string ssid);
    }
}
