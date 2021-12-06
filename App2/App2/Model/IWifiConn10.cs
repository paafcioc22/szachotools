using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Model
{
    public interface IWifiConn10
    {
        bool RequestNetwork(string ssid, string password);
        void OpenSettings();
        string SuggestNetwork(string _ssid, string _passphrase);
        string RemoveSuggestNetwork(string _ssid, string _passphrase);
    }
}
